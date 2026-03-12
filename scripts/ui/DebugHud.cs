using Godot;

public partial class DebugHud : CanvasLayer
{
    [Export] public NodePath ShipPath { get; set; }

    private Ship _ship;
    private Camera2D _camera;

    private Label _speedLabel;
    private Label _modeLabel;
    private Label _velocityLabel;
    private Label _zoomLabel;

    public override void _Ready()
    {
        _ship = GetNodeOrNull<Ship>(ShipPath);

        _speedLabel = GetNodeOrNull<Label>("MarginContainer/VBoxContainer/SpeedLabel");
        _modeLabel = GetNodeOrNull<Label>("MarginContainer/VBoxContainer/ModeLabel");
        _velocityLabel = GetNodeOrNull<Label>("MarginContainer/VBoxContainer/VelocityLabel");
        _zoomLabel = GetNodeOrNull<Label>("MarginContainer/VBoxContainer/ZoomLabel");

        if (_ship != null)
        {
            _camera = _ship.GetNodeOrNull<Camera2D>("Camera2D");
        }

        if (_speedLabel == null || _modeLabel == null || _velocityLabel == null || _zoomLabel == null)
        {
            GD.PushWarning("DebugHud: some label nodes are missing. Check node names in DebugHud.tscn.");
        }
    }

    public override void _Process(double delta)
    {
        if (_ship == null || _speedLabel == null || _modeLabel == null || _velocityLabel == null || _zoomLabel == null)
            return;

        Vector2 velocity = _ship.Velocity;

        _speedLabel.Text = $"Speed: {_ship.CurrentSpeedMps:F1} m/s";
        _modeLabel.Text = $"Mode: {(_ship.IsArcadeMode ? "Arcade" : "Realistic")}";
        _velocityLabel.Text = $"Velocity: ({velocity.X:F1}, {velocity.Y:F1})";

        if (_camera != null)
        {
            _zoomLabel.Text = $"Zoom: {_camera.Zoom.X:F2}";
        }
        else
        {
            _zoomLabel.Text = "Zoom: n/a";
        }
    }
}