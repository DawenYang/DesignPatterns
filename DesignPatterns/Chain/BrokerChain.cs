namespace DesignPatterns.Chain;

public class Query
{
    public string CreateName;
    
    public enum Argument
    {
        Attack, Defense
    }

    public Argument WhatToQuery;

    public int Value;

    public Query(string createName, Argument whatToQuery, int value)
    {
        CreateName = createName;
        WhatToQuery = whatToQuery;
        Value = value;
    }
}

public class Game // mediator pattern
{
    public event EventHandler<Query> Queries; // effectively a chain

    public void PerformQuery(object sender, Query q)
    {
        Queries?.Invoke(sender, q);
    }
}

public class CreaturePlus
{
    private Game game;
    public string name;
    private int attack, defense;

    public CreaturePlus(Game game, string name, int attack, int defense)
    {
        this.game = game;
        this.name = name;
        this.attack = attack;
        this.defense = defense;
    }

    public int Attack
    {
        get
        {
            var q = new Query(name, Query.Argument.Attack, attack);
            game.PerformQuery(this, q);
            return q.Value;
        }
    }

    public int Defense
    {
        get
        {
            var q = new Query(name, Query.Argument.Defense, defense);
            game.PerformQuery(this, q);
            return q.Value;
        }
    }
    
    public override string ToString() // no game
    {
        return $"{nameof(name)}: {name}, {nameof(attack)}: {Attack}, {nameof(defense)}: {Defense}";
        //                                                 ^^^^^^^^ using a property    ^^^^^^^^^
    }
}

public abstract class CreatureModifierPlus : IDisposable
{
    protected Game game;
    protected CreaturePlus creture;

    protected CreatureModifierPlus(Game game, CreaturePlus creature)
    {
        this.game = game;
        this.creture = creature;
        game.Queries += Handle;
    }

    protected abstract void Handle(object sender, Query q);

    public void Dispose()
    {
        game.Queries -= Handle;
    }
}

public class DoubleAttackModifierPlus : CreatureModifierPlus
{
    public DoubleAttackModifierPlus(Game game, CreaturePlus creature) : base(game, creature)
    {
    }

    protected override void Handle(object sender, Query q)
    {
        if (q.CreateName == creture.name && q.WhatToQuery == Query.Argument.Attack)
        {
            q.Value *= 2;
        }
    }
}

public class IncreaseDefenseModifierPlus : CreatureModifierPlus
{
    public IncreaseDefenseModifierPlus(Game game, CreaturePlus creature) : base(game, creature)
    {
    }

    protected override void Handle(object sender, Query q)
    {
        if (q.CreateName == creture.name && q.WhatToQuery == Query.Argument.Defense)
        {
            q.Value += 2;
        }
    }
}

public class BrokerChain
{
    static void Demo()
    {
        var game = new Game();
        var goblin = new CreaturePlus(game, "Strong Goblin", 3, 3);

        using (new DoubleAttackModifierPlus(game, goblin))
        {
            using (new IncreaseDefenseModifierPlus(game, goblin))
            {
                Console.WriteLine(goblin);
            }
        }
    }
}