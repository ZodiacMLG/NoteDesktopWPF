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
            JsonNoteRepository jsonNoteRepository = new();
            DataContext = new MainViewModel(jsonNoteRepository);
        }
    }
}