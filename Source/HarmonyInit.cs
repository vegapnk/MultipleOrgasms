using System;
using Verse;
using HarmonyLib;

namespace Multipleorgasm
{
    [StaticConstructorOnStartup]
    internal static class HarmonyInit
    {
        static HarmonyInit()
        {
            Harmony harmony = new Harmony("multiple_orgasms");
            harmony.PatchAll();
        }
    }
}
