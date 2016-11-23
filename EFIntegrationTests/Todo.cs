namespace EFIntegrationTests
{
    public class Todo
    {
        public int Id { get; set; }
        public long DateCreated { get; set; }
        public long? DateCompleted { get; set; }
        public string Text { get; set; }
        public bool Done { get; set; }
    }
}
