namespace NoteWindow.Model.Model
{
    public class Note
    {
        public string Title { get; set; }
        public string Content {  get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Guid Id { get; set; }
        public Note() 
        {
            Id = Guid.NewGuid();
            CreatedDate = DateTime.Now;
            ModifiedDate = DateTime.Now;
        }
    }
}
