namespace Venga.Tests
{
    public class BarHandler : HandleCommand<BarCommand>
    {
        public void Handle(BarCommand command)
        {
            command.WasHandled = true;
        }
    }
}