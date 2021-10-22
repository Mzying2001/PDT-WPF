using System.Windows;
using System.Windows.Controls;

namespace PDT_WPF.Views.Utils
{
    /// <summary>
    /// 增加Password扩展属性
    /// </summary>
    public static class PasswordBoxHelper
    {
        public static string GetPassword(DependencyObject obj)
        {
            return (string)obj.GetValue(PasswordProperty);
        }

        public static void SetPassword(DependencyObject obj, string value)
        {
            obj.SetValue(PasswordProperty, value);
        }

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.RegisterAttached("Password", typeof(string), typeof(PasswordBoxHelper), new PropertyMetadata("", OnPasswordPropertyChanged));

        private static void OnPasswordPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox box = sender as PasswordBox;
            string password = (string)e.NewValue;
            if (box != null && box.Password != password)
            {
                box.Password = password;
            }
        }
    }
}