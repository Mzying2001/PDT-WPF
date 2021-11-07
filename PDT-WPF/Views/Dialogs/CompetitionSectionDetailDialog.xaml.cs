using PDT_WPF.Models;
using System.Windows;

namespace PDT_WPF.Views.Dialogs
{
    /// <summary>
    /// CompetitionSectionDetailDialog.xaml 的交互逻辑
    /// </summary>
    public partial class CompetitionSectionDetailDialog : Window
    {


        public CompetitionSection Competition
        {
            get { return (CompetitionSection)GetValue(CompetitionProperty); }
            set { SetValue(CompetitionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Competition.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CompetitionProperty =
            DependencyProperty.Register("Competition", typeof(CompetitionSection), typeof(CompetitionSectionDetailDialog), new PropertyMetadata(null));


        public CompetitionSectionDetailDialog()
        {
            InitializeComponent();
        }

        public static void ShowDialog(CompetitionSection competitionSection)
        {
            new CompetitionSectionDetailDialog { Competition = competitionSection }.ShowDialog();
        }
    }
}
