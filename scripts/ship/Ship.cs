using Godot;

public partial class Ship : CharacterBody2D
{
    [ExportCategory("Visual")]
    [Export] public Texture2D IdleTexture { get; set; }
    [Export] public Texture2D ThrustTexture { get; set; }
    [Export] public float VisualScale { get; set; } = 0.20f;
    [Export] public bool FlipSpriteHorizontally { get; set; } = true;

    [ExportCategory("Movement")]
    [Export] public float ThrustAcceleration { get; set; } = 600.0f;
    [Export] public float BrakeAcceleration { get; set; } = 400.0f;
    [Export] public float RotationSpeed { get; set; } = 2.5f;
    [Export] public float MaxSpeed { get; set; } = 500.0f;
    [Export] public float DrillSafeSpeed { get; set; } = 50.0f;

    [ExportCategory("Debug")]
    [Export] public bool DebugEnabled { get; set; } = true;

    private Sprite2D _sprite;
    private Marker2D _drillPoint;
    private CollisionShape2D _collisionShape;

    public override void _Ready()
    {
        _sprite = GetNode<Sprite2D>("Sprite2D");
        _drillPoint = GetNode<Marker2D>("DrillPoint");
        _collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");

        ApplyVisualSetup();
        SetThrustVisual(false);

        if (DebugEnabled)
        {
            GD.Print("Ship placeholder ready.");
        }
    }

    private void ApplyVisualSetup()
    {
        if (_sprite == null)
            return;

        _sprite.FlipH = FlipSpriteHorizontally;
        _sprite.Scale = new Vector2(VisualScale, VisualScale);

        if (IdleTexture != null)
        {
            _sprite.Texture = IdleTexture;
        }
    }

    public void SetThrustVisual(bool enabled)
    {
        if (_sprite == null)
            return;

        if (enabled && ThrustTexture != null)
        {
            _sprite.Texture = ThrustTexture;
            return;
        }

        if (IdleTexture != null)
        {
            _sprite.Texture = IdleTexture;
        }
    }
}