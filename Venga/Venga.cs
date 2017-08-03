using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Venga
{
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
            => handler.GetType().GetInterfaces()[0].GenericTypeArguments[0] == commandType;

        public void RegisterHandler<T>(HandleCommand<T> handler) 
            => _handlers.Add(handler);
    }
}