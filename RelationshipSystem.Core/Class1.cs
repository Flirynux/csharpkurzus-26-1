namespace RelationshipSystem.Core;

public enum InteractionType
{
    CombatEncouter,
    Betrayal,
    GiftGiven,
    AssistanceProvided,
    Humiliation
}

public class Interaction<TId>
{
    public InteractionType Type { get; init; }
    public float ImpactValue { get; init; } 
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

public interface IRelationshipActor<TId>
{
    TId Id { get; }
}

public class RelationshipManager<TId> where TId : notnull
{
    private readonly Dictionary<TId, Dictionary<TId, Relationship<TId>>> _registry = new();

    public Relationship<TId> GetRelationship(TId subject, TId target)
    {
        if (!_registry.TryGetValue(subject, out var connections))
        {
            connections = new Dictionary<TId, Relationship<TId>>();
            _registry[subject] = connections;
        }

        if (!connections.TryGetValue(target, out var relationship))
        {
            relationship = new Relationship<TId>(subject, target);
            connections[target] = relationship;
        }

        return relationship;
    }

    public void RecordInteraction(TId actorA, TId actorB, Interaction<TId> interaction)
    {
        GetRelationship(actorA, actorB).AddInteraction(interaction);
    }
}