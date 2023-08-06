using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using TAE;
using Verse;

namespace NMO;

[DefOf]
public static class NMODefOf
{
    public static AtmosphericValueDef Atmosphere_Oxygen;
    public static AtmosphericValueDef Atmosphere_CarbonMonoxide;
    public static TAERulesetDef OxygenRuleset;
    public static HediffDef Hypoxia;
    public static HediffDef CerebralHypoxia;
    public static HediffDef HypoxiaSickness;
}