namespace Venga.Tests
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