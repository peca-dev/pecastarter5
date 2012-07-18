﻿using System.Windows;
using Progressive.PecaStarter5.ViewModel;
using Progressive.PecaStarter5.Views;
using Progressive.PecaStarter5.Models;

namespace Progressive.PecaStarter5
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var viewModel = new MainWindowViewModel(ExternalResource.YellowPagesList, ExternalResource.Configuration);
            viewModel.PropertyChanged += (sender, e1) => ExternalResource.Configuration = viewModel.Configuration;
            MainWindow = new MainWindow() { DataContext = viewModel };
            MainWindow.Show();

            this.DispatcherUnhandledException += (sender, dispatcherUnhandledExceptionEventArgs) =>
            {
                if (MessageBox.Show(
                    "未解決のエラーが発生しました。（" + dispatcherUnhandledExceptionEventArgs.Exception.Message + "）プログラムを終了します。",
                    "PecaStarter", MessageBoxButton.OKCancel, MessageBoxImage.Error)
                    != MessageBoxResult.OK)
                {
                    dispatcherUnhandledExceptionEventArgs.Handled = true;
                }
            };
        }
    }
}
