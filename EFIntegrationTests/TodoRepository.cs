namespace EFIntegrationTests
{
    public class TodoRepository : Repository<TodoContext, Todo>
    {
        public TodoRepository(TodoContext ctx) : base(ctx)
        {
        }
    }
}
