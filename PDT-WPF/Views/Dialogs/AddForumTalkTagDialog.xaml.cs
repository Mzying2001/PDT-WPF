using System;
using System.Windows;

namespace PDT_WPF.Views.Dialogs
{
    /// <summary>
    /// AddForumTalkTagDialog.xaml 的交互逻辑
    /// </summary>
    public partial class AddForumTalkTagDialog : Window
    {
        private bool _confirmed = false;
        private Action<bool, string, string> _callback;


        public string TalkTag
        {
            get { return (string)GetValue(TalkTagProperty); }
            set { SetValue(TalkTagProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TalkTag.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TalkTagProperty =
            DependencyProperty.Register("TalkTag", typeof(string), typeof(AddForumTalkTagDialog), new PropertyMetadata(null));


        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Description.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(string), typeof(AddForumTalkTagDialog), new PropertyMetadata(null));


        public AddForumTalkTagDialog()
        {
            InitializeComponent();
            talkTagBox.Focus();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            _confirmed = true;
            Close();
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _callback?.Invoke(_confirmed, TalkTag, Description);
        }

        public static void ShowDialog(Action<bool, string, string> callback)
        {
            new AddForumTalkTagDialog { _callback = callback }.ShowDialog();
        }
    }
}
