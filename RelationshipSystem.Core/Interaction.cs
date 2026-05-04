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
