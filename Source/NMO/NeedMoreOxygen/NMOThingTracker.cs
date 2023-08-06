﻿using JetBrains.Annotations;
using RimWorld;
using TAE;
using TeleCore;
using TeleCore.Data.Events;
using Verse;

namespace NMO;

public class NMOThingTracker : ThingTrackerComp
{
    public NMOThingTracker([NotNull] ThingTrackerMapInfo parent) : base(parent)
    {
    }

    public override void Notify_ThingRegistered(ThingStateChangedEventArgs args)
    {
        switch (args.Thing)
        {
            case ThingWithComps twc:
            {
                if (OxygenUtility.IsBurner(twc))
                {
                    twc.Map.GetMapInfo<AirMapInfo>().RegisterBurner(twc);
                }
                break;
            }
            case Fire fire:
                fire.Map.GetMapInfo<AirMapInfo>().RegisterFire(fire);
                break;
        }
    }

    public override void Notify_ThingDeregistered(ThingStateChangedEventArgs args)
    {
        switch (args.Thing)
        {
            case ThingWithComps twc:
            {
                if (OxygenUtility.IsBurner(twc))
                {
                    twc.Map.GetMapInfo<AirMapInfo>().Deregister(twc);
                }
                break;
            }
            case Fire fire:
                fire.Map.GetMapInfo<AirMapInfo>().Deregister(fire);
                break;
        }
    }

    public override void Notify_ThingSentSignal(ThingStateChangedEventArgs args)
    {
    }
}