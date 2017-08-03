using System.Collections.Generic;
using Venga.Tests.Commands;

namespace Venga.Tests.Handlers
{
    public class CompositeHandler : HandleCommand<BarCommand>
    {
        private readonly List<HandleCommand<BarCommand>> _handlers;

        public CompositeHandler(List<HandleCommand<BarCommand>> handlers)
        {
            _handlers = handlers;
        }

        public void Handle(BarCommand command) 
            => _handlers.ForEach(h => h.Handle(command));
    }
}