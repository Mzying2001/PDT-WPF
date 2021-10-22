using System;
using System.Windows;

namespace PDT_WPF.Views.Utils
{
    public static class MessageBoxHelper
    {
        public static void ShowMessage(string message, string title = "信息")
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static void ShowError(string message, string title = "错误")
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static void ShowError(Exception e, string title = "错误")
        {
            ShowError(e.Message, title);
        }

        public static bool ShowQuestion(string message, string title = "提示")
        {
            var result = MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return result == MessageBoxResult.Yes;
        }
    }
}
