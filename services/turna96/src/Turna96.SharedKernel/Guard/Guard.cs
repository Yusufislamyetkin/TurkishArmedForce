namespace Turna96.SharedKernel.Guard;

public static class Guard
{
    public static void AgainstNull(object? input, string name)
    {
        if (input is null)
        {
            throw new ArgumentNullException(name);
        }
    }
}
