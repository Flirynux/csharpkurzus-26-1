namespace RelationshipSystem.Core;

public class RelationshipManager<TId> where TId : notnull
{
    private readonly Dictionary<TId, Dictionary<TId, Relationship<TId>>> _registry = new();

    public Relationship<TId> GetRelationship(TId subject, TId target)
    {
        if (!_registry.TryGetValue(subject, out var connections))
        {
            connections = [];
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