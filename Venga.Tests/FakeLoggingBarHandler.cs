namespace Venga.Tests
{
    public class FakeLoggingBarHandler : HandleCommand<BarCommand>
    {
        public void Handle(BarCommand command) 
            => command.WasLogged = true;
    }
}