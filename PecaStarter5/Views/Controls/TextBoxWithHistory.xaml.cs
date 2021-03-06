﻿using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace Progressive.PecaStarter5.Views.Controls
{
    /// <summary>
    /// TextBoxWithHistory.xaml の相互作用ロジック
    /// </summary>
    public partial class TextBoxWithHistory : UserControl
    {
        public TextBoxWithHistory()
        {
            InitializeComponent();
        }

        private void ComboBoxItem_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ComboBoxItem;
            switch (e.ChangedButton)
            {
                case MouseButton.Middle:
                    var command = (DataContext as dynamic).Command as ICommand;
                    if (command.CanExecute(DataContext))
                    {
                        command.Execute(Tuple.Create(DataContext, item.Content));
                    }
                    break;
            }
        }
    }
}
