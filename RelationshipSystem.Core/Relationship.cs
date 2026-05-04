namespace RelationshipSystem.Core;

public interface IRelationshipActor<TId>
{
    TId Id { get; }
}

public class Relationship<TId>
{
    public TId Source { get; }
    public TId Target { get; }
    public float Value { get; private set; } // -1.0 (hate) : 1.0 (love)

    public List<Interaction<TId>> History { get; } = new();

    public Relationship(TId source, TId target)
    {
        Source = source;
        Target = target;
        Value = 0.0f; 
    }

    public void AddInteraction(Interaction<TId> interaction)
    {
        History.Add(interaction);
        Value = Math.Clamp(Value + interaction.ImpactValue, -1.0f, 1.0f);
    }
}
