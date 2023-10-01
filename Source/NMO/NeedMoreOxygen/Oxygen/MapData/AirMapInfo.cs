using System;
using System.Collections.Generic;
using System.Xml;
using JetBrains.Annotations;
using RimWorld;
using TAE;
using TAE.Atmosphere.Rooms;
using TeleCore;
using TeleCore.Primitive;
using TeleCore.Static;
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

public class AirMapInfo : MapInformation
{
    private readonly Dictionary<Thing, AtmosphereConverter> _converters;

    public IReadOnlyCollection<AtmosphereConverter> Converters => _converters.Values;
    
    public AirMapInfo([NotNull] Map map) : base(map)
    {
        _converters = new Dictionary<Thing, AtmosphereConverter>();
    }

    public AtmosphereConverter ConverterFor(Thing thing)
    {
        if (_converters.TryGetValue(thing, out var conv))
            return conv;
        return null;
    }
    
    public void RegisterBurner(ThingWithComps burner)
    {
        var converter = new OxygenBurner(burner); 
        _converters.Add(burner, converter);
    }
    
    public void RegisterFire(Fire fire)
    {
        var converter = new OxygenBurnerFire(fire); 
        _converters.Add(fire,converter);
    }
    
    public void Deregister(Thing thing)
    {
        if (_converters.TryGetValue(thing, out var converter))
        {
            _converters.Remove(thing);
        }
    }

    public override void Tick()
    {
        foreach (var converter in Converters)
        {
            converter.Tick();
        }
    }
}