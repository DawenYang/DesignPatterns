namespace DesignPatterns.Decorator;

public abstract class Shape
{
    public virtual string AsString() => string.Empty;
}

public class Circle : Shape
{
    private float radius;

    public Circle() : this(0)
    {
        
    }

    public Circle(float radius)
    {
        this.radius = radius;
    }

    public void Resize(float factor)
    {
        radius *= factor;
    }

    public override string AsString() => $"A circle of radius {radius}";
}

public class Square : Shape
{
    private float side;

    public Square(float side)
    {
        this.side = side;
    }

    public override string AsString() => $"A square with side {side}";
}

// dynamic
public class ColoredShape : Shape
{
    private Shape _shape;
    private string _color;

    public ColoredShape(Shape shape, string color)
    {
        this._shape = shape ?? throw new ArgumentNullException(paramName: nameof(shape));
        this._color = color ?? throw new ArgumentNullException(paramName: nameof(color));
    }

    public override string AsString() => $"{_shape.AsString()} has the color {_color}";
}

public class TransparentShape : Shape
{
    private Shape _shape;
    private float _transparency;

    public TransparentShape(Shape shape, float transparency)
    {
        _shape = shape;
        _transparency = transparency;
    }
    
    public override string AsString() => $"{_shape.AsString()} has {_transparency * 100.0f} transparency";
}

// CRTP cannot be done
//public class ColoredShape2<T> : T where T : Shape { }
public class ColoredShape<T> : Shape where T : Shape, new()
{
    private string color;
    private T shape = new T();

    public ColoredShape() : this("black")
    {
      
    }

    public ColoredShape(string color) // no constructor forwarding
    {
        this.color = color ?? throw new ArgumentNullException(paramName: nameof(color));
    }

    public override string AsString()
    {
        return $"{shape.AsString()} has the color {color}";
    }
}

public class DynamicDecorator
{
    
}