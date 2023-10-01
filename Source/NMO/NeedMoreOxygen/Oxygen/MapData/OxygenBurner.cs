﻿using RimWorld;
using TeleCore.Static;
using Verse;

namespace NMO;

public class OxygenBurner : AtmosphereConverter
{
    private CompRefuelable _fuelSource;
    
    public OxygenBurner(ThingWithComps thing) : base(thing)
    {
        _fuelSource = thing.GetComp<CompRefuelable>();
    }

    public override float BurningRate => 10;

    private bool previouslyHadAtmosphere = false;

    public override void Tick()
    {
        if (GenTicks.TicksAbs % 90 != 0) return;
        if (Atmosphere == null)
        {
            Log.Warning($"Tried to tick oxygen burner with thing without a room: {_sourceThing}");
            return;
        }

        if (!_fuelSource.HasFuel) return;

        var result = Atmosphere.Volume.TryRemove(NMODefOf.Atmosphere_Oxygen, BurningRate);
        if (result)
        {
            previouslyHadAtmosphere = true;
            Atmosphere.Volume.TryAdd(NMODefOf.Atmosphere_CarbonMonoxide, 5);
        }
        else if (previouslyHadAtmosphere)
        {
            previouslyHadAtmosphere = false;
            ((ThingWithComps)_sourceThing).BroadcastCompSignal(KnownCompSignals.RanOutOfFuel);
        }
    }
}