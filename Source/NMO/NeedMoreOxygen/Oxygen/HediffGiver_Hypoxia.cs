using UnityEngine;
using Verse;

namespace NMO.Oxygen;

public class HediffGiver_Hypoxia : HediffGiver
{
    public override void OnIntervalPassed(Pawn pawn, Hediff cause)
    {
        if (!pawn.Spawned) return;
        
        var need = pawn.needs.TryGetNeed<Need_Oxygen>();
        HediffSet hediffSet = pawn.health.hediffSet;
        Hediff firstHediffOfDef = hediffSet.GetFirstHediffOfDef(hediff, false);
        if (need.CurCategory == OxygenCategory.Low)
        {
            var num = Mathf.Abs(need.PercentageThreshUrgentlyOxygenDeprived - need.CurLevel);
            float num2 = Mathf.InverseLerp(0, need.PercentageThreshUrgentlyOxygenDeprived, num);
            num2 = Mathf.Lerp(num2, 0.000375F, 0.01f);
            HealthUtility.AdjustSeverity(pawn, NMODefOf.Hypoxia, num2);
        }
        else if (firstHediffOfDef != null)
        {
            float num2 = firstHediffOfDef.Severity * 0.027f;
            num2 = Mathf.Clamp(num2, 0.0015f, 0.015f);
            firstHediffOfDef.Severity -= num2;
        }
    }
}