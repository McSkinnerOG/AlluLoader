using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlluLoader
{
    public static class ApiInitializer
    {
        private static int _initialized; 
        public static void Initialize() 
        {
            if (Interlocked.Exchange(ref _initialized, 1) != 0) return; 
            var harmony = new Harmony("AlluLoader");
            harmony.PatchAll(typeof(ApiInitializer).Assembly); 
            Logging.Log.Write("AlluLoader API patches applied.");
        }
    }
}