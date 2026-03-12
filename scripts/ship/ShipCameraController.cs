using Godot;

public partial class ShipCameraController : Camera2D
{
    [ExportGroup("Smoothing")]
    [Export] public bool UsePositionSmoothing { get; set; } = true;
    [Export] public float PositionSmoothingSpeedValue { get; set; } = 8.0f;

    [ExportGroup("Zoom")]
    [Export] public float DefaultZoom { get; set; } = 1.00f;
    [Export] public float MinZoom { get; set; } = 0.70f;
    [Export] public float MaxZoom { get; set; } = 1.80f;
    [Export] public float ZoomStep { get; set; } = 0.10f;

    public override void _Ready()
    {
        Enabled = true;
        IgnoreRotation = true;

        PositionSmoothingEnabled = UsePositionSmoothing;
        PositionSmoothingSpeed = PositionSmoothingSpeedValue;

        SetZoomLevel(DefaultZoom);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is not InputEventMouseButton mouseButton || !mouseButton.Pressed)
            return;

        if (mouseButton.ButtonIndex == MouseButton.WheelUp)
        {
            SetZoomLevel(Zoom.X + ZoomStep);
            GetViewport().SetInputAsHandled();
        }
        else if (mouseButton.ButtonIndex == MouseButton.WheelDown)
        {
            SetZoomLevel(Zoom.X - ZoomStep);
            GetViewport().SetInputAsHandled();
        }
    }

    private void SetZoomLevel(float value)
    {
        float clamped = Mathf.Clamp(value, MinZoom, MaxZoom);
        Zoom = new Vector2(clamped, clamped);
    }
}