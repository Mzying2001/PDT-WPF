using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using PDT_WPF.Models;
using PDT_WPF.Models.Data;
using PDT_WPF.Services.Api;
using PDT_WPF.Utils;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace PDT_WPF.ViewModels.DialogViewModels
{
    public class AddBoardPhotoDialogViewModel : ViewModelBase
    {
        public RelayCommand SelectFileCmd { get; set; }
        public RelayCommand<BoardPhoto.JumpType> OkCmd { get; set; }
        public RelayCommand CancelCmd { get; set; }

        public ObservableCollection<EnumDescription<BoardPhoto.JumpType>> JumpTypes { get; }
            = new ObservableCollection<EnumDescription<BoardPhoto.JumpType>>
            {
                new EnumDescription<BoardPhoto.JumpType>(BoardPhoto.JumpType.JumpLink, BoardPhoto.GetJumpTypeName(BoardPhoto.JumpType.JumpLink)),
                new EnumDescription<BoardPhoto.JumpType>(BoardPhoto.JumpType.JumpMiniProgram, BoardPhoto.GetJumpTypeName(BoardPhoto.JumpType.JumpMiniProgram)),
                new EnumDescription<BoardPhoto.JumpType>(BoardPhoto.JumpType.NoJump, BoardPhoto.GetJumpTypeName(BoardPhoto.JumpType.NoJump)),
            };

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

        private async void AddBoardPhotoAsync(BoardPhoto.JumpType jumpType, Action<object> callback)
        {
            try
            {
                IsLoading = true;
                callback(await Task.Run(() => PdtV2.AddBoardPhoto(Name, Link ?? string.Empty, PhotoPath, jumpType)));
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

        private void Ok(BoardPhoto.JumpType jumpType)
        {
            AddBoardPhotoAsync(jumpType, obj =>
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
            Messenger.Default.Send(false, MessageTokens.ADD_BOARD_PHOTO_RESULT);
        }

        public AddBoardPhotoDialogViewModel()
        {
            SelectFileCmd = new RelayCommand(SelectFile);
            OkCmd = new RelayCommand<BoardPhoto.JumpType>(Ok);
            CancelCmd = new RelayCommand(Cancel);
        }
    }
}
