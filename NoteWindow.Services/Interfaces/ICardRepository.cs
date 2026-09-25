using NoteWindow.Model.Model;

namespace NoteWindow.Services.Interfaces
{
    public interface ICardRepository
    {
        Task SaveCardAsync(Card card);
        Task <IEnumerable<Card>> GetNewCardsAsync();
        Task <IEnumerable<Card>> GetReviewedCardsAsync();
        /// <summary>
        /// Перемещение "новой" карточки в раздел "изученные"
        /// </summary>
        /// <param name="cardId"></param>
        /// <returns></returns>
        Task<bool> MoveToReviewedFolder(Guid cardId);
        /// <summary>
        /// Физическое удаление изученных карточек
        /// </summary>
        /// <param name="cardId"></param>
        /// <returns></returns>
        Task<bool> DeleteReviewedCard(Guid cardId);
    }
}
