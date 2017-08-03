namespace Venga.Tests
{
    public class BazHandler : HandleCommand<BazCommand>
    {
        public void Handle(BazCommand command)
        {
            command.WasHandled = true;
        }
    }
}