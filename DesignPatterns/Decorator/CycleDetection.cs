using System.Text;

namespace DesignPatterns.Decorator;

public abstract class ShapeDecoratorCyclePolicy
{
    public abstract bool TypeAdditionAllowed(Type type, IList<Type> allTypes);
    public abstract bool ApplicationAllowed(Type type, IList<Type> allTypes);
}

public class ThrowOnCyclePolicy : ShapeDecoratorCyclePolicy
{
    private bool handler(Type type, IList<Type> allTypes)
    {
        if (allTypes.Contains(type))
            throw new InvalidOperationException($"Cycle detected! Type is aleady a {type.FullName}");

        return true;
    }
    
    public override bool TypeAdditionAllowed(Type type, IList<Type> allTypes)
    {
        return handler(type, allTypes);
    }

    public override bool ApplicationAllowed(Type type, IList<Type> allTypes)
    {
        return handler(type, allTypes);
    }
}

public class AbsorbCyclePolicy : ShapeDecoratorCyclePolicy
{
    public override bool TypeAdditionAllowed(Type type, IList<Type> allTypes)
    {
        return true;
    }

    public override bool ApplicationAllowed(Type type, IList<Type> allTypes)
    {
        return !allTypes.Contains(type);
    }
}

public class CyclesAllowedPolicy : ShapeDecoratorCyclePolicy
{
    public override bool TypeAdditionAllowed(Type type, IList<Type> allTypes)
    {
        return true;
    }

    public override bool ApplicationAllowed(Type type, IList<Type> allTypes)
    {
        return true;
    }
}

public abstract class ShapeDecorator : Shape
{
    protected internal readonly List<Type> _types = new();
    protected internal Shape _shape;

    public ShapeDecorator(Shape shape)
    {
        this._shape = shape;
        if (shape is ShapeDecorator sd)
            _types.AddRange(sd._types);
    }
}

public abstract class ShapeDecorator<TSelf, TCyclePolicy> : ShapeDecorator
    where TCyclePolicy : ShapeDecoratorCyclePolicy, new()
{
    protected readonly TCyclePolicy _policy = new();

    public ShapeDecorator(Shape shape) : base(shape)
    {
        if (_policy.TypeAdditionAllowed(typeof(TSelf), _types))
            _types.Add(typeof(TSelf));
    }
}

public class ShapeDecoratorWithPolicy<T> : ShapeDecorator<T, ThrowOnCyclePolicy>
{
    public ShapeDecoratorWithPolicy(Shape shape) : base(shape)
    {
    }
}

public class ColoredShapeT : ShapeDecorator<ColoredShapeT, AbsorbCyclePolicy>
{
    private readonly string color;

    public ColoredShapeT(Shape shape, string color) : base(shape)
    {
        this.color = color;
    }

    public override string AsString()
    {
        var sb = new StringBuilder($"{_shape.AsString()}");

        if (_policy.ApplicationAllowed(_types[0], _types.Skip(1).ToList()))
            sb.Append($"has the color {color}");

        return sb.ToString();
    }
}

public class CycleDetection
{
    static void Demo()
    {
        var circle = new Circle(2);
        var colored1 = new ColoredShapeT(circle, "red");
        var colored2 = new ColoredShapeT(colored1, "blue");
    }
}