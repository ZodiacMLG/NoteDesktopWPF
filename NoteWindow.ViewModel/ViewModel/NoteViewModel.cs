using NoteWindow.Model.Model;
using NoteWindow.Services.Interfaces;
using NoteWindow.ViewModel.Infrastructure;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
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
        public ICommand DeleteCommand { get; }

        public NoteViewModel(INoteRepository noteRepository)
        {
            Notes = new ObservableCollection<Note>();
            _noteRepository = noteRepository;
            SubmitCommand = new RelayCommand(SubmitExecute, CanSumbitExecute);
            DeleteCommand = new RelayCommand(DeleteExecute, CanDeleteExecute);
            LoadNotesAsync();
        }

        public Note SelectedNote
        {
            get => _selectedNote;
            set
            {
                _selectedNote = value;
                if (_selectedNote != null)
                {
                    Title = _selectedNote.Title;
                    Content = _selectedNote.Content;
                }
                OnPropertyChanged(nameof(SelectedNote));
            }
        }

        private async void SubmitExecute(object parameter)
        {
            try
            {
                var lines = _content?.Split('\n');
                var title = lines?.FirstOrDefault() ?? "Без названия";

                if (_selectedNote != null)
                {
                    _selectedNote.Title = title;
                    _selectedNote.Content = _content;
                    Note note = _selectedNote;
                    _selectedNote.ModifiedDate = DateTime.Now;

                    await _noteRepository.SaveNoteAsync(note);
                    MessageBox.Show("Сохранение выполнено.");
                }
                else
                {
                    // Логика нажатия кнопки
                    Note note = new()
                    {
                        Title = title,
                        Content = _content
                    };

                    await _noteRepository.SaveNoteAsync(note);

                    Notes.Add(note);
                    MessageBox.Show("Сохранение выполнено.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения: " + ex.Message);
            } 
        }

        private bool CanSumbitExecute(object parameter)
        {
            // Логика доступности кнопки (пока что всегда - true)
            return true;
        }

        private async void DeleteExecute(object parameter)
        {
            try
            {
                Note note = _selectedNote;
                await _noteRepository.DeleteNoteAsync(note.Id);
                Content = "";
                SelectedNote = null;
                MessageBox.Show("Удаление выполнено.");
                Notes.Remove(note);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка удаления: " + ex.Message);
            }
        }
        private bool CanDeleteExecute(object parameter)
        {
            if (_selectedNote != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private async Task LoadNotesAsync()
        {
            try
            {
                var notes = await _noteRepository.GetAllNotesAsync();

                foreach (var note in notes)
                {
                    Notes.Add(note);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                Task.FromException(ex);
            }
            
        }
    }
}
