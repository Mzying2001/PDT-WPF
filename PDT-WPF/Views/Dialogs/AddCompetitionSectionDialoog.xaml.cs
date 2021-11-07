using GalaSoft.MvvmLight.Messaging;
using PDT_WPF.Models.Data;
using System;
using System.Windows;

namespace PDT_WPF.Views.Dialogs
{
    /// <summary>
    /// AddCompetitionSectionDialoog.xaml 的交互逻辑
    /// </summary>
    public partial class AddCompetitionSectionDialoog : Window
    {
        private bool _result;
        private Action<bool> _callback;

        public AddCompetitionSectionDialoog()
        {
            InitializeComponent();

            Messenger.Default.Register<bool>(this, MessageTokens.ADD_COMPETITION_SECTION_RESULT, ProcessDialogResult);
            Closing += AddCompetitionSectionDialoog_Closing;
        }

        private void AddCompetitionSectionDialoog_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Messenger.Default.Unregister<bool>(this);
            _callback(_result);
        }

        /// <summary>
        /// 处理结果
        /// </summary>
        /// <param name="result"></param>
        private void ProcessDialogResult(bool result)
        {
            _result = result;
            Close();
        }

        /// <summary>
        /// 弹出窗口
        /// </summary>
        public static void ShowDialog(Action<bool> callback)
        {
            new AddCompetitionSectionDialoog { _callback = callback }.ShowDialog();
        }
    }
}
