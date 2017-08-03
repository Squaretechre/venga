using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

    public class UnhandledCommandException : Exception
    {
        public UnhandledCommandException(string message) : base(message)
        {
        }
    }

    public class OtherFooHandler : HandleCommand<FooCommand>
    {
        public void Handle(FooCommand command)
        {
            command.WasHandledBy.Add(this);
        }
    }

    public interface HandleCommand<in T>
    {
        void Handle(T command);
    }

    public class Venga : HandleCommand<object>
    {
        private readonly List<object> _handlers = new List<object>();

        public void Handle(object command)
        {
            var commandType = command.GetType();
            var targetHandlers = _handlers.Where(handler => HandlerThatCanHandleCommandType(handler, commandType));

            var handlers = targetHandlers as object[] ?? targetHandlers.ToArray();

            if (!handlers.Any())
                throw new UnhandledCommandException($"No handler registered for {commandType.Name}");

            foreach (var targetHandler in handlers)
            {
                var handlerType = targetHandler.GetType();
                var handleMethodInfo = handlerType.GetMethod("Handle");
                handleMethodInfo.Invoke(targetHandler, new[] {command});
            }
        }

        private static bool HandlerThatCanHandleCommandType(object handler, Type commandType)
        {
            return handler.GetType().GetInterfaces()[0].GenericTypeArguments[0] == commandType;
        }

        public void RegisterHandler<T>(HandleCommand<T> handler)
        {
            _handlers.Add(handler);
        }
    }
}