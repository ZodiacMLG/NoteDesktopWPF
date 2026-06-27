using NoteWindow.ViewModel.Infrastructure;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace NoteWindow.ViewModel.Model
{
    public class NoteViewModel : INotifyPropertyChanged
    {
        private string _title;
        private string _content;
        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged(nameof(Title));
                }
            }
        }
        public string Content
        {
            get => _content;
            set
            {
                if (_content != value)
                {
                    _content = value;
                    OnPropertyChanged(nameof(Content));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand SubmitCommand { get; }

        public NoteViewModel()
        {
            SubmitCommand = new RelayCommand(SubmitExecute, CanSumbitExecute);
        }

        private void SubmitExecute(object parameter)
        {
            // Логика нажатия кнопки
        }

        private bool CanSumbitExecute(object parameter)
        {
            // Логика доступности кнопки (пока что всегда - true)
            return true;
        }
    }
}
