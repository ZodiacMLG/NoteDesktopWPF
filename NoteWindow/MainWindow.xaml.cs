using Microsoft.Win32;
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

        private void ButtonProfile_Click(object sender, RoutedEventArgs e)
        {
            //string profileTitle = "Профиль пользователя";
            //if (this.Title != profileTitle)
            //{
            //    MessageBox.Show($"Сейчас название программы: {this.Title}");
            //    this.Title = profileTitle;
            //}
            //else
            //{
            //    MessageBox.Show($"Сейчас название программы: {this.Title}");
            //    this.Title = "Заметки";
            //}
        }
    }
}