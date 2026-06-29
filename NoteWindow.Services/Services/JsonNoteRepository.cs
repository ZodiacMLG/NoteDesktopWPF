using Microsoft.Extensions.Logging;
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
            _savePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files");
            if (!Directory.Exists(_savePath))
            {
                Directory.CreateDirectory(_savePath);
            }
        }

        public async Task<IEnumerable<Note>> GetAllNotesAsync()
        {
            try
            {
                string[] files = Directory.GetFiles(_savePath, "*.json");
                List<Note> note = new(files.Length);
                string json;

                foreach (string file in files)
                {
                    json = await File.ReadAllTextAsync(file);
                    note.Add(JsonSerializer.Deserialize<Note>(json));
                }

                return note!;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task SaveNoteAsync(Note note)
        {
            if (!Directory.Exists(_savePath))
            {
                Directory.CreateDirectory(_savePath);
            }

            string json = JsonSerializer.Serialize(note);

            await File.WriteAllTextAsync(Path.Combine(_savePath, note.Id.ToString() + ".json"), json);

            return;
            
        }

        public Task DeleteNoteAsync(Guid Id)
        {
            try
            {
                if(File.Exists(Path.Combine(_savePath, Id.ToString() + ".json")))
                {
                    File.Delete(Path.Combine(_savePath, Id.ToString() + ".json"));
                }
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                return Task.FromException(ex);
            }
        }
    }
}
