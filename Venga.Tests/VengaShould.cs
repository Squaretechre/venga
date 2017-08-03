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
            var targetHandler = _handlers.First(handler => HandlerThatCanHandleCommandType(handler, commandType));

            var handlerType = targetHandler.GetType();
            var handleMethodInfo = handlerType.GetMethod("Handle");
            handleMethodInfo.Invoke(targetHandler, new[] {command});
        }

        private static bool HandlerThatCanHandleCommandType(object handler, Type commandType) 
            => handler.GetType().GetInterfaces()[0].GenericTypeArguments[0] == commandType;

        public void RegisterHandler<T>(HandleCommand<T> handler) 
            => _handlers.Add(handler);
    }
}