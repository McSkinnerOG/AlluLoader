using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlluLoader.API
{
    public static class ApiInitializer
    {
        private static int _initialized; 
        public static void Initialize()
        {
            if (Interlocked.Exchange(ref _initialized, 1) != 0) return; 
            var harmony = new Harmony("alluloader.api");
            harmony.PatchAll(typeof(ApiInitializer).Assembly); 
            Logging.Log.Write("AlluLoader API patches applied.");
        }
    }
}