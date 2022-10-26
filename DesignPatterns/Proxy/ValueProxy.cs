namespace DesignPatterns.Proxy;

public struct Percentage
{
    private readonly float value;

    internal Percentage(float value)
    {
        this.value = value;
    }

    public static float operator *(float f, Percentage p)
    {
        return f * p.value;
    }

    public static Percentage operator +(Percentage a, Percentage b)
    {
        return new Percentage(a.value + b.value);
    }

    public static implicit operator Percentage(int value)
    {
        return value.Percent();
    }

    public bool Equals(Percentage other)
    {
        return value.Equals(other.value);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        return obj is Percentage other && Equals(other);
    }

    public override int GetHashCode()
    {
        return value.GetHashCode();
    }

    public override string ToString()
    {
        return $"{value * 100}%";
    }
}

public static class PercentageExtensions
{
    public static Percentage Percent(this int value) => new Percentage(value / 100.0f);

    public static Percentage Percent(this float value) => new Percentage(value / 100.0f);
}

public class ValueProxy
{
    static void Demo()
    {
        Console.WriteLine(10f * 5.Percent());
        Console.WriteLine(2.Percent() + 3.Percent());
    }
}