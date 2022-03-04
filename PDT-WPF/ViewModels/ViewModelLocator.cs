/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:PDT_WPF"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using PDT_WPF.ViewModels.DialogViewModels;
using PDT_WPF.ViewModels.PageViewModels;

namespace PDT_WPF.ViewModels
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<LoginViewModel>();
            SimpleIoc.Default.Register<HomePageViewModel>();
            SimpleIoc.Default.Register<ProjectLibraryPageViewModel>();
            SimpleIoc.Default.Register<TalentPoolPageViewModel>();
            SimpleIoc.Default.Register<ForumPageViewModel>();
            SimpleIoc.Default.Register<MessagePageViewModel>();
            SimpleIoc.Default.Register<PersonalCenterPageViewModel>();
            SimpleIoc.Default.Register<AdminPageViewModel>();
            SimpleIoc.Default.Register<TalkTagApplicationManagerViewModel>();
            SimpleIoc.Default.Register<ForumExamineToolViewModel>();
        }

        public MainViewModel Main
        {
            get => ServiceLocator.Current.GetInstance<MainViewModel>();
        }

        public LoginViewModel Login
        {
            get => ServiceLocator.Current.GetInstance<LoginViewModel>();
        }

        public HomePageViewModel HomePage
        {
            get => ServiceLocator.Current.GetInstance<HomePageViewModel>();
        }

        public ProjectLibraryPageViewModel ProjectLibraryPage
        {
            get => ServiceLocator.Current.GetInstance<ProjectLibraryPageViewModel>();
        }

        public TalentPoolPageViewModel TalentPoolPage
        {
            get => ServiceLocator.Current.GetInstance<TalentPoolPageViewModel>();
        }

        public ForumPageViewModel ForumPage
        {
            get => ServiceLocator.Current.GetInstance<ForumPageViewModel>();
        }

        public MessagePageViewModel MessagePage
        {
            get => ServiceLocator.Current.GetInstance<MessagePageViewModel>();
        }

        public PersonalCenterPageViewModel PersonalCenterPage
        {
            get => ServiceLocator.Current.GetInstance<PersonalCenterPageViewModel>();
        }

        public AdminPageViewModel AdminPage
        {
            get => ServiceLocator.Current.GetInstance<AdminPageViewModel>();
        }

        public TalkTagApplicationManagerViewModel TalkTagApplicationManager
        {
            get => ServiceLocator.Current.GetInstance<TalkTagApplicationManagerViewModel>();
        }

        public ForumExamineToolViewModel ForumExamineTool
        {
            get => ServiceLocator.Current.GetInstance<ForumExamineToolViewModel>();
        }

        public AddBoardPhotoDialogViewModel AddBoardPhotoDialog
        {
            get => new AddBoardPhotoDialogViewModel();
        }

        public AddCompetitionSectionDialogViewModel AddCompetitionSectionDialog
        {
            get => new AddCompetitionSectionDialogViewModel();
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}