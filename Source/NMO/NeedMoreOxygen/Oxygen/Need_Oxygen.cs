using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NMO.Oxygen;
using RimWorld;
using TAE;
using TeleCore;
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
        public BreathingExtension BreathingProps => pawn.kindDef.GetModExtension<BreathingExtension>();

        public float BreathingLevelRequired => BreathingProps?.OxygenLevelPercentageWantBreathe ?? 1;

        public bool Asphyxiating => this.CurCategory == OxygenCategory.Hypoxia;

        public float PercentageThreshUrgentlyOxygenDeprived => BreathingLevelRequired * 0.4f;

        public Need_Oxygen(Pawn pawn) : base(pawn)
        {
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
            var room = pawn.GetRoom();
            if (room == null)
            {
                Log.Warning("Room null...");
                return;
            }

            var isOutdoors = room.UsesOutdoorTemperature;
            AtmosphericContainer container;
            if (isOutdoors)
            {
                container = room.Map.GetMapInfo<AtmosphericMapInfo>().MapContainer;
            }
            else
            {
                container = room.GetRoomComp<RoomComponent_Atmospheric>().RoomContainer;
            }

            CurLevel = Mathf.Lerp(0, 1, container.StoredPercentOf(NMODefOf.Oxygen) / 0.85f);
            if (!isOutdoors)
            {
                container.TryRemoveValue(NMODefOf.Oxygen, 10, out _);
            }

            if (CurCategory == OxygenCategory.Low)
            {
                HealthUtility.AdjustSeverity(this.pawn, NMODefOf.Hypoxia, 0.25f);
            }

            if (Asphyxiating)
            {
                HealthUtility.AdjustSeverity(this.pawn, NMODefOf.Hypoxia, 0.85f);
            }
        }
    }
}
