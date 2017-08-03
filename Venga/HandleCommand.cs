namespace Venga
{
    public interface HandleCommand<in T>
    {
        void Handle(T command);
    }
}