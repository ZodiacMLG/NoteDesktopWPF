using NoteWindow.Services.Interfaces;
using NoteWindow.Services.Services;
using NoteWindow.ViewModel.Infrastructure;
using NoteWindow.ViewModel.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;

namespace NoteWindow.ViewModel.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private object? _currentView;
        private INoteRepository _noteRepository;
        private ICardRepository _cardRepository;
        private NoteViewModel _noteViewModel;
        private DictionaryUserControlViewModel _dictionaryUserControlViewModel;
        private ProfileUserControlViewModel _profileUserControlViewModel;

        public NoteViewModel NoteViewModel => _noteViewModel;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public object? CurrentView
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
        public ICommand ShowDictionaryCommand { get; }

        public MainViewModel(INoteRepository noteRepository,
                             ICardRepository cardRepository)
        {
            _noteRepository = noteRepository;
            _cardRepository = cardRepository;
            _noteViewModel = new NoteViewModel(noteRepository, cardRepository);
            _dictionaryUserControlViewModel = new DictionaryUserControlViewModel();
            _profileUserControlViewModel = new ProfileUserControlViewModel();
            
            CurrentView = _noteViewModel;

            ShowProfileCommand = new RelayCommand(ShowProfileExecute, CanShowProfileExecute);
            ShowNotesCommand = new RelayCommand(ShowNotesExecute, CanShowNotesExecute);
            ShowDictionaryCommand = new RelayCommand(ShowDictionaryExecute, CanShowDictionaryExecute);
        }

        private void ShowProfileExecute(object parameter)
        {
            if (CurrentView == _profileUserControlViewModel)
            {
                CurrentView = _noteViewModel;
            }
            else
            {
                CurrentView = _profileUserControlViewModel;
            }
        }
        private bool CanShowProfileExecute(object parameter)
        {
            return true;
        }
        private void ShowDictionaryExecute(object parameter)
        {
            if (CurrentView == _dictionaryUserControlViewModel)
            {
                CurrentView = _noteViewModel;
            }
            else
            {
                CurrentView = _dictionaryUserControlViewModel;
            }
        }
        private bool CanShowDictionaryExecute(object parameter)
        {
            return true;
        }

        private void ShowNotesExecute(object parameter)
        {
            CurrentView = _noteViewModel;
        }
        private bool CanShowNotesExecute(object parameter)
        {
            return true;
        }
        
    }
}
