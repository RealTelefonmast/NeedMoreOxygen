using TAE;
using TeleCore.FlowCore;
using Verse;

namespace NMO.Oxygen;

public class Comp_OxygenProvider : ThingComp, IContainerImplementer<AtmosphericDef, IContainerHolderThing<AtmosphericDef>,ValueContainerThing<AtmosphericDef, IContainerHolderThing<AtmosphericDef>>>
{
    public ValueContainerThing<AtmosphericDef, IContainerHolderThing<AtmosphericDef>> Container { get; }

    public CompEquippable Equippable { get; private set; }
    
    public override void PostSpawnSetup(bool respawningAfterLoad)
    {
        base.PostSpawnSetup(respawningAfterLoad);
        Equippable = parent.TryGetComp<CompEquippable>();
    }
}