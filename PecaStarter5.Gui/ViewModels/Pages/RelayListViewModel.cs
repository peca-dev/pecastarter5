﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Progressive.Commons.ViewModels;
using Progressive.PecaStarter5.Models;
using Progressive.PecaStarter5.ViewModels.Commands;
using Progressive.Peercast4Net;

namespace Progressive.PecaStarter5.ViewModels.Pages
{
    public class RelayListViewModel : ViewModelBase
    {
        public event UnhandledExceptionEventHandler ExceptionThrown;

        public RelayListViewModel(Peercast peercast, IEnumerable<IYellowPages> yellowPagesList)
        {
            YellowPagesList = yellowPagesList;
            ReloadCommand = new ReloadCommand(this, peercast);
            OpenCommand = new OpenCommand(this, peercast, yellowPagesList);
        }

        private bool isLoading = false;
        public bool IsLoading
        {
            get { return isLoading; }
            set
            {
                isLoading = value;
                OnPropertyChanged("IsLoading");
            }
        }

        private IList<IChannel> channels = new List<IChannel>();
        public IList<IChannel> Channels
        {
            get { return channels; }
            set
            {
                channels = value;
                OnPropertyChanged("Channels");
                SelectedChannel = null;
            }
        }

        private IChannel selectedChannel;
        public IChannel SelectedChannel
        {
            get { return selectedChannel; }
            set
            {
                if (!SetProperty("SelectedChannel", ref selectedChannel, value))
                    return;
                OpenCommand.OnCanExecuteChanged();
            }
        }

        public IEnumerable<IYellowPages> YellowPagesList { get; private set; }

        private IYellowPages selectedYellowPages;
        public IYellowPages SelectedYellowPages
        {
            get { return selectedYellowPages; }
            set
            {
                if (!SetProperty("SelectedYellowPages", ref selectedYellowPages, value))
                    return;
                OpenCommand.OnCanExecuteChanged();
            }
        }

        public ICommand ReloadCommand { get; private set; }
        public OpenCommand OpenCommand { get; private set; }

        public event EventHandler<SelectedEventArgs> ChannelSelected;

        internal void OnChannelSelected()
        {
            if (ChannelSelected != null)
                ChannelSelected(this, new SelectedEventArgs(SelectedChannel, SelectedYellowPages));
        }

        internal void OnExceptionThrown(Exception ex)
        {
            if (ExceptionThrown != null)
                ExceptionThrown(this, new UnhandledExceptionEventArgs(ex, false));
        }
    }

    public class SelectedEventArgs : EventArgs
    {
        public IChannel Channel { get; private set; }
        public IYellowPages YellowPages { get; private set; }

        public SelectedEventArgs(IChannel channel, IYellowPages yellowPages)
        {
            Channel = channel;
            YellowPages = yellowPages;
        }
    }
}
