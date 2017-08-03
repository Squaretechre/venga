using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Venga.Tests
{
    public class VengaShould
    {
        [Fact]
        public void forward_baz_command_to_baz_handler()
        {
            var command = new BazCommand();

            var venga = new Venga();

            var bazHandler = new BazHandler();
            venga.RegisterHandler(bazHandler);
            venga.Handle(command);

            Assert.True(command.WasHandled);
        }

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
        public void forward_correct_command_to_handler()
        {
            var command = new BarCommand();

            var venga = new Venga();

            venga.RegisterHandler(new BarHandler());
            venga.Handle(command);

            Assert.True(command.WasHandled);
        }
    }

    public class BazHandler : IHandleCommand<BazCommand>
    {
        public void Handle(BazCommand command)
        {
            command.WasHandled = true;
        }
    }

    public class BazCommand
    {
        public bool WasHandled { get; set; }
    }

    public class BarHandler : IHandleCommand<BarCommand>
    {
        public void Handle(BarCommand command)
        {
            command.WasHandled = true;
        }
    }

    public interface IHandleCommand<T>
    {
        void Handle(T command);
    }

    public class BarCommand
    {
        public bool WasHandled { get; set; }
    }

    public class Venga
    {
        private readonly List<object> _handlers = new List<object>();

        public void Handle(object command)
        {
            var commandType = command.GetType();
            var handler = _handlers.First(h => h.GetType().GetInterfaces()[0].GenericTypeArguments[0] == commandType);

            var handlerType = handler.GetType();
            var handleMethodInfo = handlerType.GetMethod("Handle");
            handleMethodInfo.Invoke(handler, new[] {command});
        }

        public void RegisterHandler<T>(IHandleCommand<T> fooHandler)
        {
            _handlers.Add(fooHandler);
        }
    }

    public class FooCommand
    {
        public bool WasHandled { get; set; }
    }

    public class FooHandler : IHandleCommand<FooCommand>
    {
        public void Handle(FooCommand command)
        {
            command.WasHandled = true;
        }
    }
}