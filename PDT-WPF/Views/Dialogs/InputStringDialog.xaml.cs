using System;
using System.Windows;

namespace PDT_WPF.Views.Dialogs
{
    /// <summary>
    /// InputStringDialog.xaml 的交互逻辑
    /// </summary>
    public partial class InputStringDialog : Window
    {
        private bool _confirmed = false;
        private Action<bool, string> _callback;


        public string Input
        {
            get { return (string)GetValue(InputProperty); }
            set { SetValue(InputProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Input.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InputProperty =
            DependencyProperty.Register("Input", typeof(string), typeof(InputStringDialog), new PropertyMetadata(null));


        public InputStringDialog()
        {
            InitializeComponent();
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _callback?.Invoke(_confirmed, Input);
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            _confirmed = true;
            Close();
        }

        public static void ShowDialog(Action<bool, string> callback, string title = null, string defaultText = null)
        {
            new InputStringDialog()
            {
                _callback = callback,
                Title = title ?? "请输入文本",
                Input = defaultText
            }.ShowDialog();
        }
    }
}
