using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;
using UnityEngine;

namespace FallBalance;

// Here are some basic resources on code style and naming conventions to help
// you in your first CSharp plugin!
// https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions
// https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/identifier-names
// https://learn.microsoft.com/en-us/dotnet/standard/design-guidelines/names-of-namespaces

// This BepInAutoPlugin attribute comes from the Hamunii.BepInEx.AutoPlugin
// NuGet package, and it will generate the BepInPlugin attribute for you!
// For more info, see https://github.com/Hamunii/BepInEx.AutoPlugin
[BepInAutoPlugin]
public partial class Plugin : BaseUnityPlugin
{
    internal static ManualLogSource Log { get; private set; } = null!;

    private void Awake()
    {
        // BepInEx gives us a logger which we can use to log information.
        // See https://lethal.wiki/dev/fundamentals/logging
        Log = Logger;

        // BepInEx also gives us a config file for easy configuration.
        // See https://lethal.wiki/dev/intermediate/custom-configs

        // We can apply our hooks here.
        // See https://lethal.wiki/dev/fundamentals/patching-code

        Harmony val = new Harmony("com.github.cjmanca.FallBalance");
        val.PatchAll(Assembly.GetExecutingAssembly());

        // Log our awake here so we can see it in LogOutput.log file
        Log.LogInfo($"Plugin {Name} is loaded!");
    }



    [HarmonyPatch(typeof(CharacterMovement), "FallTime")]
    public class Patch_FallBalance
    {
        [HarmonyPrefix]
        public static float FallTime(CharacterMovement __instance)
        {
            float num = Mathf.Min(__instance.character.data.sinceJump, __instance.character.data.sinceGrounded);
            num -= 0.5f;
            if (__instance.character.data.lastBouncedTime + 2.25f > Time.time)
            {
                num -= __instance.character.data.lastBouncedTime + 2.5f - Time.time;
            }
            return num;
        }
    }
}
