namespace DesignPatterns.Decorator;

public class Bird
{
    public void Fly()
    {
        
    }
}

public class Lizard
{
    public void Crawl()
    {
        
    }
}

public class Dragon
{
    private Bird _bird;
    private Lizard _lizard;

    public Dragon(Bird bird, Lizard lizard)
    {
        _bird = bird ?? throw new ArgumentNullException(paramName: nameof(bird));
        _lizard = lizard ?? throw new ArgumentNullException(paramName: nameof(lizard));
    }

    public void Crawl()
    {
        _lizard.Crawl();
    }

    public void Fly()
    {
        _bird.Fly();
    }
}

public interface ICreature
{
    int Age { get; set; }
}

public interface IBird : ICreature
{
    void Fly()
    {
        if (Age >= 10)
        {
            Console.WriteLine("I am flying");
        }
    }
}

public interface ILizard : ICreature
{
    void Crawl()
    {
        if (Age < 10)
        {
            Console.WriteLine("I am crawling");
        }
    }
}

public class Organism {}

public class DragonD : ICreature
{
    public int Age { get; set; }
}

public class MultipleInheritance
{
    
}