using NoteWindow.Services.Interfaces;
using NoteWindow.Services.Services;
using NoteWindow.ViewModel.ViewModel;
using System.Windows;

namespace NoteWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            INoteRepository jsonNoteRepository = new JsonNoteRepository();
            ICardRepository cardRepository = new JsonCardRepository();
            DataContext = new MainViewModel(jsonNoteRepository, cardRepository);
        }
    }
}