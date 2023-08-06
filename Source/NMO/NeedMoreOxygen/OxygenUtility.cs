using RimWorld;
using Verse;

namespace NMO;

public static class OxygenUtility
{
    private static bool BurnsMaterial(ThingFilter filter)
    {
        foreach (var def in filter.thingDefs)
        {
            if (def == ThingDefOf.WoodLog || def == ThingDefOf.Chemfuel)
                return true;
        }

        return false;
    }

    public static bool IsBurner(ThingWithComps thing)
    {
        var invalid = true;
        if (thing.comps.NullOrEmpty()) return false;
        foreach (var comp in thing.comps)
        {
            switch (comp)
            {
                case CompRefuelable refuel:
                    invalid &= !BurnsMaterial(refuel.Props.fuelFilter);
                    break;
                case CompHeatPusher heat:
                    invalid &= !(heat.Props.heatPerSecond > 0);
                    break;
                
            }
        }
        return !invalid;
    }
}