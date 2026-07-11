using BaseLib.Config;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Modding;

namespace BetterPlayerCursors.BetterPlayerCursorsCode;

[ModInitializer(nameof(Initialize))]
public partial class MainFile : Node
{
    public const string ModId = "BetterPlayerCursors";

    public static MegaCrit.Sts2.Core.Logging.Logger Logger { get; } = new(ModId, MegaCrit.Sts2.Core.Logging.LogType.Generic);

    public static CursorConfig Config { get; private set; } = null!;

    public static void Initialize()
    {
        Harmony harmony = new(ModId);
        harmony.PatchAll();

        Config = new CursorConfig(); // loads saved values (or writes defaults) on construction
        Config.ConfigChanged += (_, _) => CursorTinter.Apply();
        Config.OnConfigReloaded += CursorTinter.Apply;
        ModConfigRegistry.Register(ModId, Config);

        // cursors spawn in the lobby before slot indices exist; recolor them once the run starts
        MegaCrit.Sts2.Core.Runs.RunManager.Instance.RunStarted += _ => RemoteCursorColorPatches.ReapplyAll();
    }
}