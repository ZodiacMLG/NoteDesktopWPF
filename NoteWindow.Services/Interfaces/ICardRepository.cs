using NoteWindow.Model.Model;

namespace NoteWindow.Services.Interfaces
{
    public interface ICardRepository
    {
        Task SaveCardAsync(Card card);
        Task <IEnumerable<Card>> GetNewCardsAsync();
        Task <IEnumerable<Card>> GetReviewedCardsAsync();
        Task<bool> MoveToReviewedFolder(Guid cardId);
    }
}
