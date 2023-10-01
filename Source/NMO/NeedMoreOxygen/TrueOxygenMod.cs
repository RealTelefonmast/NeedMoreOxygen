using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Verse;
using RimWorld;

namespace NeedMoreOxygen
{
    public class TrueOxygenMod : Mod
    {
        private static Harmony _oxygen;
        
        public TrueOxygenMod(ModContentPack content) : base(content)
        {
            _oxygen = new Harmony("telefonmast.trueoxygen");
            _oxygen.PatchAll();
        }
    }
}
