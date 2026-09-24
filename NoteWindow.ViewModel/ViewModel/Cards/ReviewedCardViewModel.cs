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
using System.Windows.Input;

namespace NoteWindow.ViewModel.ViewModel.Cards
{
    public class ReviewedCardViewModel : INotifyPropertyChanged
    {
        private Card _currentCard;
        private bool _hasCards;
        private readonly ICardRepository _cardRepository;
        private ObservableCollection<Card> _cards { get; }

        public bool HasCards
        {
            get => _hasCards;
            set
            {
                _hasCards = value;
                OnPropertyChanged(nameof(HasCards));
                //OnPropertyChanged(nameof(CardsVisibility));
                //OnPropertyChanged(nameof(EmptyMessageVisibility));
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
            MarkAsLearnedCommand = new RelayCommand(MarkAsLearnedExecute, CanMarkAsLearnedExecute);
        }

        public void NextExecute(object parameter)
        {

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

        public void MarkAsLearnedExecute(object parameter)
        {

        }

        public bool CanMarkAsLearnedExecute(object paramater)
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
    }
}
