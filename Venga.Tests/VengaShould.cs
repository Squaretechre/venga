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
            var fakeHandler = new FakeHandler();

            venga.RegisterHandler(fakeHandler);
            venga.Handle(command);
            Assert.True(fakeHandler.WasCalled);
        }
    }

    public class Venga
    {
        private readonly List<FakeHandler> _handlers = new List<FakeHandler>();

        public void Handle(FooCommand command)
        {
            _handlers.First().WasCalled = true; 
        }

        public void RegisterHandler(FakeHandler fakeHandler)
        {
            _handlers.Add(fakeHandler);
        }
    }

    public class FooCommand
    {
    }

    public class FakeHandler
    {
        public bool WasCalled { get; set; }
    }
}
