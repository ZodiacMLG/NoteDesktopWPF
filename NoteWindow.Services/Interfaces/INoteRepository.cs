using NoteWindow.Model.Model;

namespace NoteWindow.Services.Interfaces
{
    public interface INoteRepository
    {
        Task<IEnumerable<Note>> GetAllNotesAsync();
        Task SaveNoteAsync(Note note);
        Task DeleteNoteAsync(Guid Id);
        Task SaveCardAsync(string text);
    }
}
