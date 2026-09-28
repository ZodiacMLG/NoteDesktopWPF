using NoteWindow.Services.Interfaces;
using NoteWindow.ViewModel.Infrastructure;
using NoteWindow.ViewModel.ViewModel.Cards;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace NoteWindow.ViewModel.ViewModel
{
    public class DictionaryUserControlViewModel : INotifyPropertyChanged
    {

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
                OnPropertyChanged(nameof(MenuVisibility));
            }
        }

        public Visibility MenuVisibility => CurrentView == null ? Visibility.Visible : Visibility.Collapsed;
        public ICommand ShowNewCardsCommand { get; }
        public ICommand ShowReviewedCardsCommand { get; }

        private object? _currentView;
        private readonly StudyCardsViewModel _studyCardsViewModel;
        private readonly ICardRepository _cardRepository;

        public DictionaryUserControlViewModel(ICardRepository cardRepository)
        {
            _cardRepository = cardRepository;

            ShowNewCardsCommand = new RelayCommand(ShowNewCardsExecute, CanShowNewCardsExecute);
            ShowReviewedCardsCommand = new RelayCommand(ShowReviewedCardsExecute, CanShowReviewedCardsExecute);
        }

        private void ShowNewCardsExecute(object parameter)
        {
            StudyCardsViewModel studyVm = new StudyCardsViewModel(_cardRepository);
            studyVm.RequestBackToMenu += (s, e) => CurrentView = null;
            CurrentView = studyVm;
        }

        private bool CanShowNewCardsExecute(object parameter)
        {
            return true;
        }

        private void ShowReviewedCardsExecute(object parameter)
        {
            ReviewedCardViewModel revievedVm = new ReviewedCardViewModel(_cardRepository);
            revievedVm.RequestBackToMenu += (s, e) => CurrentView = null;
            CurrentView = revievedVm;
        }

        private bool CanShowReviewedCardsExecute(object parameter)
        {
            return true;
        }
    }
}
