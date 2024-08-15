namespace PGHub.Application.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string name, int key) : base($"{name} ({key}) is not found")
        {
            
        }
    }
}
