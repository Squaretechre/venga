using Venga.Tests.Commands;

namespace Venga.Tests.Handlers
{
    public class OtherFooHandler : HandleCommand<FooCommand>
    {
        public void Handle(FooCommand command)
        {
            command.WasHandledBy.Add(this);
        }
    }
}