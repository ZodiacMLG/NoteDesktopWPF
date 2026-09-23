using NoteWindow.Model.Model;
using NoteWindow.Services.Interfaces;
using NoteWindow.ViewModel.Infrastructure;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace NoteWindow.ViewModel.ViewModel.Cards
{
    public class StudyCardsViewModel : INotifyPropertyChanged
    {
        public bool IsEditMode
        {
            get => _isEditMode;
            set { _isEditMode = value; OnPropertyChanged(nameof(IsEditMode)); }
        }

        public bool HasCards
        {
            get => _hasCards;
            set
            {
                _hasCards = value;
                OnPropertyChanged(nameof(HasCards));
                OnPropertyChanged(nameof(CardsVisibility));
                OnPropertyChanged(nameof(EmptyMessageVisibility));
            }
        }
        public Card CurrentCard
        {
            get => _currentCard;
            set
            {
                if (value != null)
                {
                    _currentCard = value;
                    OnPropertyChanged(nameof(CurrentCard));
                }
            }
        }
        public ICommand NextCommand { get; }
        public ICommand PreviousCommand { get; }
        public ICommand MarkAsLearnedCommand { get; }
        public Visibility CardsVisibility => HasCards ? Visibility.Visible : Visibility.Collapsed;
        public Visibility EmptyMessageVisibility => HasCards ? Visibility.Collapsed : Visibility.Visible;

        private readonly ICardRepository _cardRepository;
        private bool _isProcessing = false;
        private bool _isEditMode;
        private Card _currentCard;
        private ObservableCollection<Card> _cards { get; }
        private int _currentIndex;
        private bool _hasCards;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public StudyCardsViewModel(ICardRepository cardRepository)
        {
            _cards = new ObservableCollection<Card>();
            _cardRepository = cardRepository;

            NextCommand     = new RelayCommand(NextExecute, CanNextExecute);
            PreviousCommand = new RelayCommand(PreviousExecute, CanPreviousExecute);
            MarkAsLearnedCommand = new RelayCommand(MarkAsLearnedExecute, CanMarkAsLearnedExecute);

            LoadCardsAsync();
        }

        private void NextExecute(object parameter)
        {
            if (_currentIndex < _cards.Count - 1)
            {
                _currentIndex++;
            }
            else
            {
                _currentIndex = 0;
            }
            CurrentCard = _cards[_currentIndex];
        }

        private bool CanNextExecute(object parameter)
        {
            if (HasCards)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void PreviousExecute(object parameter)
        {
            if (_currentIndex > 0)
            {
                _currentIndex--;
            }
            CurrentCard = _cards[_currentIndex];
        }

        private bool CanPreviousExecute(object parameter)
        {
            if (_currentIndex > 0 && HasCards)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private async void MarkAsLearnedExecute(object parameter)
        {
            try
            {
                _isProcessing = true;
                CommandManager.InvalidateRequerySuggested();
                if (CurrentCard != null)
                {
                    bool successMove = await _cardRepository.MoveToReviewedFolder(CurrentCard.Id);

                    if (successMove)
                    {
                        _cards.Remove(CurrentCard);
                    }

                    if (_cards.Count == 0)
                    {
                        HasCards = false;
                        return;
                    }
                    else
                    {
                        if (_currentIndex > _cards.Count - 1)
                        {
                            _currentIndex = 0;
                        }
                        CurrentCard = _cards[_currentIndex];
                    }
                }
            }
            finally
            {
                _isProcessing = false;
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private bool CanMarkAsLearnedExecute(object parameter)
        {
            if (_isProcessing && HasCards)
            {
                return false;
            }
            else
            {
                return true;
            }
            
        }

        private async Task LoadCardsAsync()
        {
            try
            {
                IEnumerable<Card> cards = await _cardRepository.GetNewCardsAsync();

                if (!cards.Any())
                {
                    HasCards = false;
                }
                else
                {
                    foreach (Card card in cards)
                    {
                        _cards.Add(card);
                    }
                    HasCards = true;
                    CurrentCard = _cards.First();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                await Task.FromException(ex);
            }
        }
    }
}
