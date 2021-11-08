using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using PDT_WPF.Models;
using PDT_WPF.Models.Data;
using PDT_WPF.Services.Api;
using PDT_WPF.Views.Utils;
using System;
using System.Threading.Tasks;

namespace PDT_WPF.ViewModels.DialogViewModels
{
    public class AddCompetitionSectionDialogViewModel : ViewModelBase
    {
        public RelayCommand OkCmd { get; set; }
        public RelayCommand CancelCmd { get; set; }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set => Set(ref _isLoading, value);
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        private string _information;
        public string Information
        {
            get => _information;
            set => Set(ref _information, value);
        }

        private async void AddCompetitionSectionAsync(Action<object> callback)
        {
            try
            {
                IsLoading = true;
                callback(await Task.Run(() => PdtV2.AddCompetitionSection(Title, Information)));
            }
            catch (Exception e)
            {
                callback(e.Message);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void Ok()
        {
            AddCompetitionSectionAsync(obj =>
            {
                try
                {
                    if (obj is PdtV2.AddCompetitionSectionResponse result)
                    {
                        if (result.code == Services.Http.HttpStatus.OK)
                        {
                            Messenger.Default.Send(true, MessageTokens.ADD_COMPETITION_SECTION_RESULT);
                        }
                        else
                        {
                            throw new Exception($"{result.mesg}\n{ErrorFormater.GetString(result.errors)}".Trim());
                        }
                    }
                    else
                    {
                        throw new Exception(obj.ToString());
                    }
                }
                catch (Exception e)
                {
                    MessageBoxHelper.ShowError(e);
                }
            });
        }

        private void Cancel()
        {
            Messenger.Default.Send(false, MessageTokens.ADD_COMPETITION_SECTION_RESULT);
        }

        public AddCompetitionSectionDialogViewModel()
        {
            OkCmd = new RelayCommand(Ok);
            CancelCmd = new RelayCommand(Cancel);
        }
    }
}
