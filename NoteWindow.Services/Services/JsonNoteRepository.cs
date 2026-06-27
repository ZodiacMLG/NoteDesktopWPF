using NoteWindow.Model.Model;
using NoteWindow.Services.Interfaces;
using System.Text.Json;

namespace NoteWindow.Services.Services
{
    public class JsonNoteRepository : INoteRepository
    {
        private readonly string _savePath;

        public JsonNoteRepository()
        {
            _savePath = Path.Combine("Files/");
        }

        public Task<IEnumerable<Note>> GetAllNotesAsync()
        {

            return null;
        }

        public Task SaveNoteAsync(Note note)
        {
            try
            {
                if (!Directory.Exists(_savePath))
                {
                    Directory.CreateDirectory(_savePath);
                }

                var json = JsonSerializer.Serialize(note);

                File.WriteAllTextAsync(_savePath, json);

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Task DeleteNoteAsync(Guid Id)
        {
            return null;
        }
    }
}
