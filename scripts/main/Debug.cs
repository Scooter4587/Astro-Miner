using Godot;
using System.Collections.Generic;

public partial class Debug : Node
{
    // MASTER SWITCH
    public static bool Enabled = true;

    // SEKCIE
    public static bool ShipGeneral = true;
    public static bool ShipCollision = false;
    public static bool DrillDetection = true;
    public static bool DrillAction = true;
    public static bool Camera = false;
    public static bool Hud = false;
    public static bool MainFlow = false;

    // Anti-spam pamäť podľa kanálu
    private static readonly Dictionary<string, string> _lastMessageByChannel = new();

    private const string CHANNEL_SHIP_GENERAL = "ship_general";
    private const string CHANNEL_SHIP_COLLISION = "ship_collision";
    private const string CHANNEL_DRILL_DETECTION = "drill_detection";
    private const string CHANNEL_DRILL_ACTION = "drill_action";
    private const string CHANNEL_CAMERA = "camera";
    private const string CHANNEL_HUD = "hud";
    private const string CHANNEL_MAIN_FLOW = "main_flow";

    public static void LogShipGeneral(string message, bool dedupe = false)
    {
        Log(CHANNEL_SHIP_GENERAL, ShipGeneral, message, dedupe);
    }

    public static void LogShipCollision(string message, bool dedupe = false)
    {
        Log(CHANNEL_SHIP_COLLISION, ShipCollision, message, dedupe);
    }

    public static void LogDrillDetection(string message, bool dedupe = true)
    {
        Log(CHANNEL_DRILL_DETECTION, DrillDetection, message, dedupe);
    }

    public static void LogDrillAction(string message, bool dedupe = true)
    {
        Log(CHANNEL_DRILL_ACTION, DrillAction, message, dedupe);
    }

    public static void LogCamera(string message, bool dedupe = false)
    {
        Log(CHANNEL_CAMERA, Camera, message, dedupe);
    }

    public static void LogHud(string message, bool dedupe = false)
    {
        Log(CHANNEL_HUD, Hud, message, dedupe);
    }

    public static void LogMainFlow(string message, bool dedupe = false)
    {
        Log(CHANNEL_MAIN_FLOW, MainFlow, message, dedupe);
    }

    public static void ClearChannel(string channel)
    {
        if (_lastMessageByChannel.ContainsKey(channel))
            _lastMessageByChannel.Remove(channel);
    }

    public static void ClearAll()
    {
        _lastMessageByChannel.Clear();
    }

    private static void Log(string channel, bool sectionEnabled, string message, bool dedupe)
    {
        if (!Enabled || !sectionEnabled)
            return;

        if (dedupe &&
            _lastMessageByChannel.TryGetValue(channel, out string lastMessage) &&
            lastMessage == message)
        {
            return;
        }

        _lastMessageByChannel[channel] = message;
        GD.Print(message);
    }
}