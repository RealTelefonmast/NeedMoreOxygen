﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace NMO.Oxygen
{
    public class BreathingExtension : DefModExtension
    {
        public List<BreathingRequirements> requiredAtmospheres;
        
        public void PostLoad()
        {
            
        }
	}

    public class BreathingRequirements
    {
        public AtmosphericValueDef atmosphere;
        public float baseLevel = 0.21f;
        public List<BreathingStage> stages;
    }

    public class BreathingStage
    {
        public string label;
        public float level;
        public List<HediffGiver> hediffGivers;
    }
}
