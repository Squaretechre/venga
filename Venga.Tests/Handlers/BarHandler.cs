using Venga.Tests.Commands;

namespace Venga.Tests.Handlers
{
    public class BarHandler : HandleCommand<BarCommand>
    {
        public void Handle(BarCommand command)
        {
            command.WasHandled = true;
        }
    }
}