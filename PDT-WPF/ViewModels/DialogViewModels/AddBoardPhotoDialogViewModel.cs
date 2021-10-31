using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using PDT_WPF.Models.Data;
using PDT_WPF.Services.Api;
using PDT_WPF.Views.Utils;
using System;
using System.Threading.Tasks;

namespace PDT_WPF.ViewModels.DialogViewModels
{
    public class AddBoardPhotoDialogViewModel : ViewModelBase
    {
        public RelayCommand SelectFileCmd { get; set; }
        public RelayCommand OkCmd { get; set; }
        public RelayCommand CancelCmd { get; set; }

        private bool _isLoadong;
        public bool IsLoading
        {
            get => _isLoadong;
            set => Set(ref _isLoadong, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        private string _link;
        public string Link
        {
            get => _link;
            set => Set(ref _link, value);
        }

        private string _photoPath;
        public string PhotoPath
        {
            get => _photoPath;
            set => Set(ref _photoPath, value);
        }

        private void SelectFile()
        {
            var ofd = new OpenFileDialog { Filter = "图片|*jpg;*.jepg;*.png;*.bmp;*.gif" };
            if (ofd.ShowDialog() == true)
            {
                PhotoPath = ofd.FileName;
            }
        }

        private async void AddBoardPhotoAsync(Action<object> callback)
        {
            try
            {
                IsLoading = true;
                callback(await Task.Run(() => PdtV2.AddBoardPhoto(Name, Link, PhotoPath)));
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
            AddBoardPhotoAsync(obj =>
            {
                try
                {
                    if (obj is PdtV2.AddBoardPhotoResponse result)
                    {
                        if (result.code == Services.Http.HttpStatus.OK)
                        {
                            Messenger.Default.Send(true, MessageTokens.ADD_BOARD_PHOTO_RESULT);
                        }
                        else
                        {
                            throw new Exception(result.errors.Link);
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
            Messenger.Default.Send(false, MessageTokens.ADD_BOARD_PHOTO_RESULT);
        }

        public AddBoardPhotoDialogViewModel()
        {
            SelectFileCmd = new RelayCommand(SelectFile);
            OkCmd = new RelayCommand(Ok);
            CancelCmd = new RelayCommand(Cancel);
        }
    }
}
