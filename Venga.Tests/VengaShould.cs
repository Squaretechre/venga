using System.Collections.Generic;
using System.Linq;
using Venga.Tests.Commands;
using Venga.Tests.Handlers;
using Xunit;

namespace Venga.Tests
{
    public class VengaShould
    {
        [Fact]
        public void forward_command_to_handler()
        {
            var command = new FooCommand();

            var venga = new Venga();

            venga.RegisterHandler(new FooHandler());
            venga.Handle(command);
            Assert.True(command.WasHandled);
        }

        [Fact]
        public void forward_command_to_composite_handler()
        {
            var barCommand = new BarCommand();

            var venga = new Venga();

            var handlers = new List<HandleCommand<BarCommand>>
            {
                new BarHandler(),
                new FakeLoggingBarHandler()
            };

            var compositeHandler = new CompositeHandler(handlers);
            venga.RegisterHandler(compositeHandler);
            venga.Handle(barCommand);

            Assert.True(barCommand.WasHandled);
            Assert.True(barCommand.WasLogged);
        }

        [Fact]
        public void forward_same_command_to_multiple_different_handlers()
        {
            var venga = new Venga();

            var command = new FooCommand();

            var aFooHandler = new FooHandler();
            var anotherFooHandler = new OtherFooHandler();

            venga.RegisterHandler(aFooHandler);
            venga.RegisterHandler(anotherFooHandler);

            venga.Handle(command);

            Assert.True(command.WasHandledBy.Any(handler => handler is FooHandler));
            Assert.True(command.WasHandledBy.Any(handler => handler is OtherFooHandler));
        }

        [Fact]
        public void throw_if_no_handler_registered_for_command()
        {
            var venga = new Venga();

            var command = new FooCommand();

            var exception = Record.Exception(() => venga.Handle(command));

            Assert.NotNull(exception);
            Assert.IsType<UnhandledCommandException>(exception);
            Assert.Equal("No handler registered for FooCommand", exception.Message);
        }
    }
}