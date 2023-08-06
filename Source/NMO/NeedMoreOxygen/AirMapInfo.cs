using System.Collections.Generic;
using System.Xml;
using JetBrains.Annotations;
using RimWorld;
using TAE;
using TAE.Atmosphere.Rooms;
using TeleCore;
using TeleCore.Primitive;
using UnityEngine;
using Verse;

namespace NMO;

public class AirSourceProperties
{
    public List<DefValue<AtmosphericValueDef, double>> atmospheres;
    public int interval;
}

public class AtmosConversionRule
{
    public AtmosConversionRule from;
    public AtmosConversionRule to;
    
    public void LoadDataFromXmlCustom(XmlNode root)
    {
        
    }
}

public class AirSource
{    
}


public abstract class AtmosphereConverter
{
    protected RoomComponent_Atmosphere Atmosphere;

    public AtmosphereConverter(Thing thing)
    {
        Atmosphere = thing.GetRoom().GetRoomComp<RoomComponent_Atmosphere>();
    }
    
    public abstract void Tick();
}

public class OxygenBurner : AtmosphereConverter
{
    private ThingWithComps _sourceThing;
    
    public OxygenBurner(ThingWithComps thing) : base(thing)
    {
        _sourceThing = thing;
    }
    
    public override void Tick()
    {
        if (GenTicks.TicksAbs % 180 != 0) return;
        var result = Atmosphere.Volume.TryRemove(NMODefOf.Atmosphere_Oxygen, 10d);
        if (result)
        {
            Atmosphere.Volume.TryAdd(NMODefOf.Atmosphere_CarbonMonoxide, 5);
        }
        else
        {
            if (_sourceThing.TryGetComp<CompBreakdownable>(out var comp))
            {
                comp.DoBreakdown();        
            }
        }
    }
}

public class OxygenBurnerFire : AtmosphereConverter
{
    private Fire _sourceFire;
    
    public OxygenBurnerFire(Fire fire) : base(fire)
    {
        _sourceFire = fire;
    }
    
    public override void Tick()
    {
        if (GenTicks.TicksAbs % 2 != 0) return;
        var saturation = Atmosphere.Volume.StoredPercentOf(NMODefOf.Atmosphere_Oxygen);
        var result = Atmosphere.Volume.TryRemove(NMODefOf.Atmosphere_Oxygen, 1d);
        if(result.Actual[NMODefOf.Atmosphere_Oxygen] > 0)
        {
            Atmosphere.Volume.TryAdd(NMODefOf.Atmosphere_CarbonMonoxide, 0.25f);
        }

        _sourceFire.fireSize = Mathf.Clamp(_sourceFire.fireSize, 0, Fire.MaxFireSize * saturation);
        // else
        // {
        //     _sourceFire.TakeDamage(new DamageInfo(DamageDefOf.Extinguish, 10f));
        // }
    }
}

public class AirMapInfo : MapInformation
{
    private List<AtmosphereConverter> _convs;
    private readonly Dictionary<Thing, AtmosphereConverter> _converters;
    
    public AirMapInfo([NotNull] Map map) : base(map)
    {
        _convs = new List<AtmosphereConverter>();
        _converters = new Dictionary<Thing, AtmosphereConverter>();
    }
    
    public void RegisterBurner(ThingWithComps burner)
    {
        var converter = new OxygenBurner(burner); 
        _convs.Add(converter);
        _converters.Add(burner, converter);
    }
    
    public void RegisterFire(Fire fire)
    {
        var converter = new OxygenBurnerFire(fire); 
        _convs.Add(converter);
        _converters.Add(fire,converter);
    }
    
    public void Deregister(Thing thing)
    {
        if (_converters.TryGetValue(thing, out var converter))
        {
            _convs.Remove(converter);
            _converters.Remove(thing);
        }
    }

    public override void Tick()
    {
        for (var i = _convs.Count - 1; i >= 0; i--)
        {
            var converter = _convs[i];
            converter.Tick();
        }
    }
}