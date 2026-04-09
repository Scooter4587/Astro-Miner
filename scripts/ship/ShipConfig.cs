using Godot;

[GlobalClass]
public partial class ShipConfig : Resource
{
    [ExportGroup("Movement")]
    [Export] public float MaxSpeedMps { get; set; } = 500.0f;
    [Export] public float ThrustAccelerationMps2 { get; set; } = 250.0f;
    [Export] public float RotationSpeedRad { get; set; } = 2.5f;
    [Export] public float AlignmentToleranceDeg { get; set; } = 6.0f;
    [Export] public float DrillSafeSpeedMps { get; set; } = 50.0f;

    [ExportGroup("Direction Change")]
    [Export] public float DirectionChangeBrakeDecelerationMps2 { get; set; } = 220.0f;
    [Export] public float DirectionChangeBrakeAngleDeg { get; set; } = 100.0f;
    [Export] public float MinSpeedForDirectionBrakeMps { get; set; } = 20.0f;

    [ExportGroup("Thrusters")]
    [Export] public float FullStopDecelerationMps2 { get; set; } = 160.0f;

    [ExportGroup("Flight Modes")]
    [Export] public bool StartInArcadeMode { get; set; } = false;
    [Export] public bool RealisticRequiresAlignment { get; set; } = true;
    [Export] public bool ArcadeContinuousThrust { get; set; } = true;

    [ExportGroup("UI")]
    [Export] public bool ShowSpeedInMps { get; set; } = true;

    [ExportGroup("Collision")]
    [Export] public float ImpactTangentPreserve { get; set; } = 0.98f;
    [Export] public float CrashSpeedThresholdMps { get; set; } = 150.0f;
    [Export] public float CrashExtraDamping { get; set; } = 0.77f;

    [Export] public float FlightHeadOnDotThreshold { get; set; } = 0.65f;
    [Export] public float FlightBounceMultiplier { get; set; } = 0.45f;
    [Export] public float FlightBounceCooldownSec { get; set; } = 0.14f;
    [Export] public float FlightBounceMinImpactSpeedMps { get; set; } = 70.0f;

    [ExportGroup("Drill")]
    [Export] public float DrillDetectDistancePx { get; set; } = 18.0f;
}