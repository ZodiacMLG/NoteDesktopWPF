using NoteWindow.Model.Model;
using NoteWindow.Services.Interfaces;
using NoteWindow.ViewModel.Infrastructure;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace NoteWindow.ViewModel.Model
{
    public class NoteViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Note> Notes { get; }
        private INoteRepository _noteRepository;
        private string _title;
        private string _content;
        private Note _selectedNote;

        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged(nameof(Title));
                }
            }
        }
        public string Content
        {
            get => _content;
            set
            {
                if (_content != value)
                {
                    _content = value;
                    OnPropertyChanged(nameof(Content));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand SubmitCommand { get; }

        public NoteViewModel(INoteRepository noteRepository)
        {
            Notes = new ObservableCollection<Note>();
            _noteRepository = noteRepository;
            SubmitCommand = new RelayCommand(SubmitExecute, CanSumbitExecute);
            LoadNotesAsync();
        }

        public Note SelectedNote
        {
            get => _selectedNote;
            set
            {
                _selectedNote = value;
                _selectedNote.Title = _title;
                _selectedNote.Content = _content;
                OnPropertyChanged(nameof(SelectedNote));
            }
        }

        private async void SubmitExecute(object parameter)
        {
            var lines = _content?.Split('\n');
            // Логика нажатия кнопки
            Note note = new()
            {
                Title = lines?.FirstOrDefault() ?? "Без названия",
                Content = _content
            };

            if (_selectedNote != null)
            {
                await _noteRepository.SaveNoteAsync(note);
            }
            else if (_selectedNote == null)
            {

            }
            
        }

        private bool CanSumbitExecute(object parameter)
        {
            // Логика доступности кнопки (пока что всегда - true)
            return true;
        }

        private async Task LoadNotesAsync()
        {
            var notes = await _noteRepository.GetAllNotesAsync();

            foreach (var note in notes)
            {
                Notes.Add(note);
            }

            return;
        }
    }
}
