﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NMO.Oxygen;
using RimWorld;
using TAE;
using TeleCore.Primitive;
using UnityEngine;
using Verse;

namespace NMO;

public enum OxygenCategory : byte
{
    Saturated,
    Low,
    Hypoxia
}

public class Need_Oxygen : Need
{
    private Comp_PawnAtmosphereTracker _atmosTracker;
    private BreathingExtension _cachedProps;

    public BreathingExtension BreathingProps => _cachedProps;

    public float BreathingLevelRequired => BreathingProps?.OxygenLevelPercentageWantBreathe ?? 0.75f;
    public float PercentageThreshUrgentlyOxygenDeprived => BreathingLevelRequired * 0.4f;

    /*
    private float HypoxiaFactorBase
    {
        get
        {
            float baseFactor = 0f;

            if (!pawn.health.capacities.CapableOf(PawnCapacityDefOf.Breathing)) return 0f;

            var breathing = pawn.health.capacities.GetLevel(PawnCapacityDefOf.Breathing);
            var bloodPumping = pawn.health.capacities.GetLevel(PawnCapacityDefOf.BloodPumping);
            var consciousness = pawn.health.capacities.GetLevel(PawnCapacityDefOf.Consciousness);

            baseFactor += 1 - (breathing * bloodPumping * consciousness);

            // Incorporate health conditions
            if (pawn.health.hediffSet.HasHediff(HediffDefOf.Hypothermia))
            {
                baseFactor *= 1.25f;
            }

            if (pawn.health.hediffSet.HasHediff(HediffDefOf.Asthma))
            {
                baseFactor *= 2;
            }

            return 1 + baseFactor;
        }
    }

    private float HypoxiaSeverityPerInterval
    {
        get
        {
            var hypoxiaFactor = HypoxiaFactorBase;
            if (hypoxiaFactor <= 0) return 0;

            var levelFactor = 1f - CurLevel;
            return hypoxiaFactor * levelFactor * (HypoxiaSeverityFactor * Mathf.Lerp(0.8f, 1.2f, Rand.ValueSeeded(pawn.thingIDNumber ^ 2551674)));
        }
    }
    */

    //States
    public bool Suffocating => CurCategory == OxygenCategory.Hypoxia;
    public override bool IsFrozen => base.IsFrozen || pawn.Deathresting;

    //TODO: Add a way to track whether room is getting filled or not
    public override int GUIChangeArrow { get; }

    public Need_Oxygen(Pawn pawn) : base(pawn)
    {
        _atmosTracker = Comp_PawnAtmosphereTracker.CompFor(pawn);
        _cachedProps = pawn.kindDef.GetModExtension<BreathingExtension>();
    }

    public OxygenCategory CurCategory
    {
        get
        {
            if (CurLevelPercentage <= 0f)
            {
                return OxygenCategory.Hypoxia;
            }
            if (CurLevelPercentage < PercentageThreshUrgentlyOxygenDeprived)
            {
                return OxygenCategory.Low;
            }
            return OxygenCategory.Saturated;
        }
    }

    public override void NeedInterval()
    {
        if (!pawn.Spawned)
        {
            //Not Spawned...
            return;
        }

        if (_atmosTracker != null)
        {
            CurLevel = _atmosTracker.RoomComp.Volume.StoredPercentOf(NMODefOf.Atmosphere_Oxygen);

            if (!_atmosTracker.IsOutside)
                _atmosTracker.RoomComp.Volume.TryRemove(NMODefOf.Atmosphere_Oxygen, 2);
        }

        if (!IsFrozen || pawn.Deathresting)
        {
            var hasHypoxia = pawn.health.hediffSet.GetFirstHediff<Hediff_Hypoxia>();
            if (Suffocating)
            {
                var hypoxia = hasHypoxia ?? (Hediff_Hypoxia) pawn.health.AddHediff(NMODefOf.Hypoxia);
                hypoxia.Severity += 0.1f; 
            }
            else if (hasHypoxia != null)
            {
                pawn.health.RemoveHediff(hasHypoxia);
                if(!pawn.health.hediffSet.HasHediff(NMODefOf.HypoxiaSickness))
                    pawn.health.AddHediff(NMODefOf.HypoxiaSickness);
            }
        }
    }
}