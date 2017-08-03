using Venga.Tests.Commands;

namespace Venga.Tests.Handlers
{
    public class FakeLoggingBarHandler : HandleCommand<BarCommand>
    {
        public void Handle(BarCommand command) 
            => command.WasLogged = true;
    }
}