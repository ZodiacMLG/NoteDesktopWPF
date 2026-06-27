using System.Windows;

namespace NoteWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int i = 0;
        public MainWindow()
        {
            InitializeComponent();
            i = ListNotes.Items.Count;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            ListNotes.Items.Add($"{i}");
            i++;
        }
    }
}