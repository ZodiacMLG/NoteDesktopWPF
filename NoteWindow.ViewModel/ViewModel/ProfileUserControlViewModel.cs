using Microsoft.Win32;
using NoteWindow.ViewModel.Infrastructure;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace NoteWindow.ViewModel.ViewModel
{
    public class ProfileUserControlViewModel : INotifyPropertyChanged
    {
        private ImageSource _avatar;
        private string _avatarSavePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                                      "images/avatar",
                                                      "avatar.jpg"
                                                      );
        private string _directorySavePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                                      "images/avatar");
        private string _filePath { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand DownloadAvatarCommand { get; set; }

        public ProfileUserControlViewModel()
        {
            if (!Directory.Exists(_directorySavePath))
            {
                Directory.CreateDirectory(_directorySavePath);
            }
            DownloadAvatarCommand = new RelayCommand(DownloadAvatarExecute, CanDownloadAvatarExecute);
            LoadAvatarFromLocalStorage();
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
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.FileName = "Document"; // Default file name
            dialog.Filter = "JPEG Files (*.jpeg;*.png;*.jpg;*.gif)|*.jpeg;*.png;*.jpg;*.gif"; // Filter files by extension

            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dialog.FileName;
                _filePath = dialog.FileName;
                Avatar = new BitmapImage(new Uri(_filePath));

                SaveBitmapImage((BitmapImage)Avatar, _avatarSavePath);
            }
        }

        private bool CanDownloadAvatarExecute(object parameter)
        {
            return true;
        }

        private void SaveBitmapImage(BitmapImage bitmapImage, string filePath)
        {
            BitmapEncoder encoder = new JpegBitmapEncoder();

            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                encoder.Save(fileStream);
            }
        }

        private void LoadAvatarFromLocalStorage()
        {
            if (File.Exists(_avatarSavePath)) 
            {
                Avatar = LoadBitmapFromFile();
            }
        }

        private BitmapImage LoadBitmapFromFile()
        {
            BitmapImage bitmap = new BitmapImage();

            bitmap.BeginInit();
            bitmap.UriSource = new Uri(_avatarSavePath);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            bitmap.Freeze();

            return bitmap;
        }
    }
}