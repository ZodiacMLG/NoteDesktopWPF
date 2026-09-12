using NoteWindow.Model.Model;
using NoteWindow.Services.Interfaces;
using NoteWindow.ViewModel.Infrastructure;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace NoteWindow.ViewModel.ViewModel.Cards
{
    public class StudyCardsViewModel : INotifyPropertyChanged
    {
        public ICardRepository _cardRepository;
        public bool IsEditMode;
        public bool HasCards
        {
            get => _hasCards;
            set
            {
                _hasCards = value;
                OnPropertyChanged(nameof(HasCards));
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
            return true;
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
            if (_currentIndex > 0)
            {
                return true;
            }
            else
            {
                return false;
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
