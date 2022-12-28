using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NMO.Oxygen;
using RimWorld;
using TAE;
using UnityEngine;
using Verse;

namespace NMO
{
    public enum OxygenCategory : byte
    {
        Saturated,
        Low,
        Hypoxia
    }

    public class Need_Oxygen : Need
    {
        private Comp_PawnAtmosphereTracker atmosTracker;

        public BreathingExtension BreathingProps => pawn.kindDef.GetModExtension<BreathingExtension>();

        public float BreathingLevelRequired => BreathingProps?.OxygenLevelPercentageWantBreathe ?? 1;

        public bool Asphyxiating => this.CurCategory == OxygenCategory.Hypoxia;

        public float PercentageThreshUrgentlyOxygenDeprived => BreathingLevelRequired * 0.4f;

        public Need_Oxygen(Pawn pawn) : base(pawn)
        {
            atmosTracker = Comp_PawnAtmosphereTracker.CompFor(pawn);
        }

        public OxygenCategory CurCategory
        {
            get
            {
                if (base.CurLevelPercentage <= 0f)
                {
                    return OxygenCategory.Hypoxia;
                }
                if (base.CurLevelPercentage < this.PercentageThreshUrgentlyOxygenDeprived)
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

            if (atmosTracker != null)
            {
                CurLevel = atmosTracker.Container.StoredPercentOf(NMODefOf.Atmosphere_Oxygen);

                if (!atmosTracker.IsOutside)
                    atmosTracker.RoomComp.TryRemoveValue(NMODefOf.Atmosphere_Oxygen, 1, out _);
            }

            if (CurLevel <= 0.75f)
            {
                HealthUtility.AdjustSeverity(this.pawn, NMODefOf.Hypoxia, 1 - CurLevel);
            }

            /*
            if (CurCategory == OxygenCategory.Low)
            {
                HealthUtility.AdjustSeverity(this.pawn, NMODefOf.Hypoxia, 0.25f);
            }

            if (Asphyxiating)
            {
                HealthUtility.AdjustSeverity(this.pawn, NMODefOf.Hypoxia, 0.85f);
            }
            */
        }
    }
}
