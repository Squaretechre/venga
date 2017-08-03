using Venga.Tests.Commands;

namespace Venga.Tests.Handlers
{
    public class FooHandler : HandleCommand<FooCommand>
    {
        public void Handle(FooCommand command)
        {
            command.WasHandled = true;
            command.WasHandledBy.Add(this);
        }
    }
}