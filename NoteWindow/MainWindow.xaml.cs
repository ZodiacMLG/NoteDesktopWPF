using NoteWindow.Services.Services;
using NoteWindow.ViewModel.Model;
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
            NoteViewModel noteViewModel = new(jsonNoteRepository);
            DataContext = noteViewModel;
        }
    }
}