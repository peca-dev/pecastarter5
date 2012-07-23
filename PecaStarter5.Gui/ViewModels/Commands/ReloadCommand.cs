﻿using System;
using System.Linq;
using System.Windows.Input;
using Progressive.PecaStarter5.ViewModels.Pages;
using Progressive.Peercast4Net;
using System.Threading.Tasks;

namespace Progressive.PecaStarter5.ViewModels.Commands
{
    class ReloadCommand : ICommand
    {
        private readonly RelayListViewModel parent;
        private readonly Peercast peercast;

        public ReloadCommand(RelayListViewModel parent, Peercast peercast)
        {
            this.parent = parent;
            this.peercast = peercast;
        }

        #region ICommand メンバー

        public bool CanExecute(object parameter)
        {
            return !parent.IsLoading;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            parent.IsLoading = true;
            OnCanExecuteChanged();
            peercast.GetChannelsAsync().ContinueWith((x) =>
            {
                parent.Channels = x.Result.ToList();
                parent.IsLoading = false;
                OnCanExecuteChanged();
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        #endregion

        private void OnCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, new EventArgs());
        }
    }
}
