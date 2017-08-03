using System.Collections.Generic;
using System.Linq;
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
        public void forward_correct_command_to_handler()
        {
            var command = new BarCommand();

            var venga = new Venga();

            venga.RegisterHandler(new BarHandler());
            venga.Handle(command);

            Assert.True(command.WasHandled);
        }
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
        private readonly List<FooHandler> _fooHandlers = new List<FooHandler>();
        private readonly List<object> _handlers = new List<object>(); 

        public void Handle(object command)
        {
            if (command is FooCommand)
            {
                var fooHandler = (FooHandler)_fooHandlers.First();
                fooHandler.Handle((FooCommand) command);
            }
            else
            {
                var barHandler = (BarHandler)_handlers.First();
                barHandler.Handle((BarCommand) command);
            }
        }

        public void RegisterHandler<T>(IHandleCommand<T> fooHandler) 
        {
            if (fooHandler is FooHandler)
            {
                _fooHandlers.Add((FooHandler) fooHandler);
            }
            else
            {
                _handlers.Add((object)fooHandler);
            }
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
