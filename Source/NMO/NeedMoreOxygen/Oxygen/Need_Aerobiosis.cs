﻿using RimWorld;
using Verse;

namespace NMO.Oxygen;

public class Need_Aerobiosis : Need
{
    private Comp_PawnAtmosphereTracker _atmosTracker;
    private BreathingExtension _cachedProps;

    
    public Need_Aerobiosis(Pawn pawn) : base(pawn)
    {
        _atmosTracker = Comp_PawnAtmosphereTracker.CompFor(pawn);
        _cachedProps = pawn.kindDef.GetModExtension<BreathingExtension>();
    }

    public override void NeedInterval()
    {
        
    }
}