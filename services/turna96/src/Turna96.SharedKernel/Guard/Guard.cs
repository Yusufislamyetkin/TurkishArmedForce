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

    public static void AgainstNullOrWhiteSpace(string? input, string name)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            throw new ArgumentException($"{name} cannot be null or whitespace.", name);
        }
    }

    public static void AgainstOutOfRange(int value, int min, int max, string name)
    {
        if (value < min || value > max)
        {
            throw new ArgumentOutOfRangeException(name, value, $"Expected between {min} and {max}.");
        }
    }

    public static void AgainstByteLength(string input, int max, string name)
    {
        if (System.Text.Encoding.UTF8.GetByteCount(input) > max)
        {
            throw new ArgumentException($"{name} exceeds byte length {max}.", name);
        }
    }
}
