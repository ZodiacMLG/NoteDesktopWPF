using NoteWindow.Services.Services;
using NoteWindow.ViewModel.Infrastructure;
using NoteWindow.ViewModel.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace NoteWindow.ViewModel.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private object _currentView;
        private JsonNoteRepository _noteRepository;
        private NoteViewModel _noteViewModel;
        private ProfileViewModel _profileViewModel;

        public NoteViewModel NoteViewModel => _noteViewModel;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        public ICommand ShowProfileCommand { get; }
        public ICommand ShowNotesCommand { get; }

        public MainViewModel(JsonNoteRepository noteRepository)
        {
            _noteViewModel = new NoteViewModel(noteRepository);
            _profileViewModel = new ProfileViewModel();
            CurrentView = _noteViewModel;

            ShowProfileCommand = new RelayCommand(ShowProfileExecute, CanShowProfileExecute);
            ShowNotesCommand = new RelayCommand(ShowNotesExecute, CanShowNotesExecute);
        }

        private async void ShowProfileExecute(object parameter)
        {
            CurrentView = _profileViewModel;
        }
        private bool CanShowProfileExecute(object parameter)
        {
            return true;
        }
        private async void ShowNotesExecute(object parameter)
        {
            CurrentView = _noteViewModel;
        }
        private bool CanShowNotesExecute(object parameter)
        {
            return true;
        }
    }
}
