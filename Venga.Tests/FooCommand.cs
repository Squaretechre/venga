using System.Collections.Generic;

namespace Venga.Tests
{
    public class FooCommand
    {
        public FooCommand()
        {
            WasHandledBy = new List<object>();
        }

        public bool WasHandled { get; set; }
        public List<object> WasHandledBy { get; set; }
    }
}