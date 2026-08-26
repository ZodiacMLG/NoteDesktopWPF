using Microsoft.Win32;
using NoteWindow.ViewModel.Infrastructure;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media;

namespace NoteWindow.ViewModel.ViewModel
{
    public class ProfileUserControlViewModel : INotifyPropertyChanged
    {
        private ImageSource _avatar;
        public string FilePath { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand DownloadAvatarCommand { get; set; }

        public ProfileUserControlViewModel()
        {
            DownloadAvatarCommand = new RelayCommand(DownloadAvatarExecute, CanDownloadAvatarExecute);
        }

        public ImageSource Avatar
        {
            get => _avatar;
            set
            {
                _avatar = value;
                OnPropertyChanged(nameof(Avatar));
            }
        }

        private void DownloadAvatarExecute(object parameter)
        {
            var dialog = new OpenFileDialog();
            dialog.FileName = "Document"; // Default file name
            dialog.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif"; // Filter files by extension

            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dialog.FileName;
            }
        }

        private bool CanDownloadAvatarExecute(object parameter)
        {
            return true;
        }

    }
}
