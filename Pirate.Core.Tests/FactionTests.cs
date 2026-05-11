using Pirate.Core;
using Pirate.Core.entities;
namespace Pirate.Core.Tests;

public class Tests
{

    [Test]
    public void FactionWarIsMutualTest()
    {
        Random rnd = new Random(42);
        Faction faction1 = new Faction(FactionType.ENGLISH, "fac1", rnd);
        Faction faction2 = new Faction(FactionType.SPANISH, "fac2", rnd);

        faction1.War(faction2);

        Assert.Multiple(() =>
        {
            Assert.That(faction1.Enemies.Contains(faction2), Is.True);
            Assert.That(faction2.Enemies.Contains(faction1), Is.True);

        });
    }

    [Test]
    public void FactionAllianceIsMutualTest()
    {
        Random rnd = new Random(42);
        Faction faction1 = new Faction(FactionType.ENGLISH, "fac1", rnd);
        Faction faction2 = new Faction(FactionType.SPANISH, "fac2", rnd);

        faction1.Alliance(faction2);

        Assert.Multiple(() =>
        {
            Assert.That(faction1.Allies.Contains(faction2), Is.True);
            Assert.That(faction2.Allies.Contains(faction1), Is.True);

        });
    }

    [Test]
    public void FactionPeaceIsMutualTest()
    {
        Random rnd = new Random(42);
        Faction faction1 = new Faction(FactionType.ENGLISH, "fac1", rnd);
        Faction faction2 = new Faction(FactionType.SPANISH, "fac2", rnd);

        faction1.War(faction2);
        faction2.SetNeutralRelation(faction1);

        faction2.Alliance(faction1);
        faction1.SetNeutralRelation(faction2);


        Assert.Multiple(() =>
        {
            Assert.That(faction1.Enemies.Contains(faction2), Is.False);
            Assert.That(faction2.Enemies.Contains(faction1), Is.False);

            Assert.That(faction1.Allies.Contains(faction2), Is.False);
            Assert.That(faction2.Allies.Contains(faction1), Is.False);

        });
    }
}
