namespace PGHub.Domain.Entities
{
    public class Post
    {
        public Post()
        {
            Attachments = new List<Attachment>();
        }
        public Guid Id { get; set; }
        public Guid AuthorId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public bool IsPined { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime DeletionDate { get; set; }
        public List<Attachment> Attachments { get; set; }
    }
}
