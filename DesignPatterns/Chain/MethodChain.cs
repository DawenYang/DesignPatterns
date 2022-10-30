using System.Xml;

namespace DesignPatterns.Chain;

public class Creature
{
    public string Name;
    public int Attack, Defense;

    public Creature(string name, int attack, int defense)
    {
        Name = name ?? throw new ArgumentNullException(paramName: nameof(name));
        Attack = attack;
        Defense = defense;
    }

    public override string ToString()
    {
        return $"{nameof(Name)}: {Name}, {nameof(Attack)} : {Attack}, {nameof(Defense)}: {Defense}";
    }
}

public class CreatureModifier
{
    protected Creature _creature;
    protected CreatureModifier next;
    
    public CreatureModifier(Creature creature)
    {
        this._creature = creature ?? throw new ArgumentNullException(paramName: nameof(creature));
    }

    public void Add(CreatureModifier cm)
    {
        if (next != null) next.Add(cm);
        else next = cm;
    }

    public virtual void Handle() => next?.Handle();
}

public class NoBanusesModifier : CreatureModifier
{
    public NoBanusesModifier(Creature creature) : base(creature)
    {
    }

    public override void Handle()
    {
        Console.WriteLine("No bonuses for you!");
    }
}

public class DoubelAttackModifier : CreatureModifier
{
    public DoubelAttackModifier(Creature creature) : base(creature)
    {
    }

    public override void Handle()
    {
        Console.WriteLine($"Doubling {_creature.Name}'s attack");
        _creature.Attack *= 2;
        base.Handle();
    }
}

public class IncreaseDefenseModifier : CreatureModifier
{
    public IncreaseDefenseModifier(Creature creature) : base(creature)
    {
    }

    public override void Handle()
    {
        Console.WriteLine("Increasing goblin's defense");
        _creature.Defense += 3;
        base.Handle();
    }
}

public class MethodChain
{
    static void Demo()
    {
        var goblin = new Creature("Goblin", 2, 2);
        Console.WriteLine(goblin);

        var root = new CreatureModifier(goblin);
        
        // this guy does not invoke base.handle()
        // root.Add(new NoBanusesModifier(goblin));
        
        // root next is null, next: DoubleAttack
        root.Add(new DoubelAttackModifier(goblin));
        
        // root next is DoubleAttack, then invoke DoubleAttack.Add(), then DoubleAttack.next is IncreaseDefense
        root.Add(new IncreaseDefenseModifier(goblin));
        
        // root.Handle() -> DoubleAttack.Handle() -> base.Handle() which is next.Handle() -> IncreaseDefense.Handle() -> null
        root.Handle();
    }
}