using NoteWindow.Model.Model;
using NoteWindow.Services.Interfaces;
using NoteWindow.ViewModel.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace NoteWindow.ViewModel.ViewModel.Cards
{
    public class ReviewedCardViewModel : INotifyPropertyChanged
    {
        // Event для кнопки "Вернуться назад"
        public event EventHandler? RequestBackToMenu;

        private Card _currentCard;
        private bool _hasCards;
        private readonly ICardRepository _cardRepository;
        private int _currentIndex;
        private bool _isProcessing = false;
        private ObservableCollection<Card> _cards { get; }

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
        //public ICommand PreviousCommand { get; }
        public ICommand MarkAsLearnedCommand { get; }
        public ICommand BackMenuCommand { get; }
        public Visibility CardsVisibility => HasCards ? Visibility.Visible : Visibility.Collapsed;
        public Visibility EmptyMessageVisibility => HasCards ? Visibility.Collapsed : Visibility.Visible;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ReviewedCardViewModel(ICardRepository cardRepository)
        {
            _cards = new ObservableCollection<Card>();
            _cardRepository = cardRepository;

            NextCommand = new RelayCommand(NextExecute, CanNextExecute);
            //PreviousCommand = new RelayCommand(PreviousExecute, CanPreviousExecute);
            MarkAsLearnedCommand = new RelayCommand(MarkAsLearnedReviewedExecute, CanMarkAsLearnedReviewedExecute);
            BackMenuCommand = new RelayCommand(BackMenuExecute, CanBackMenuExecute);
            

            LoadCardsAsync();
        }

        public void NextExecute(object parameter)
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
        public bool CanNextExecute(object parameter)
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

        public async void MarkAsLearnedReviewedExecute(object parameter)
        {
            try
            {
                _isProcessing = true;
                CommandManager.InvalidateRequerySuggested();
                if (CurrentCard != null)
                {
                    bool successMove = await _cardRepository.DeleteReviewedCard(CurrentCard.Id);

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

        public bool CanMarkAsLearnedReviewedExecute(object paramater)
        {
            if (_isProcessing || !HasCards)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void BackMenuExecute(object parameter)
        {
            RequestBackToMenu?.Invoke(this, EventArgs.Empty);
        }

        private bool CanBackMenuExecute(object parameter)
        {
            return true;
        }

        private async Task LoadCardsAsync()
        {
            try
            {
                IEnumerable<Card> cards = await _cardRepository.GetReviewedCardsAsync();

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
