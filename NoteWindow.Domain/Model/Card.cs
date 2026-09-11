using NoteWindow.Model.Enums;

namespace NoteWindow.Model.Model
{
    public class Card
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public CardStatus Status { get; set; }
        public DateTime CreatedTime { get; set; }

        public Card()
        {
            Id = Guid.NewGuid();
            CreatedTime = DateTime.Now;

            Status = CardStatus.New;
        }
    }
}
