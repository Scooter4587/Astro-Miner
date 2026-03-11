using Godot;

public partial class Ship : CharacterBody2D
{
    [ExportGroup("Config")]
    [Export] public ShipConfig Config { get; set; }

    [ExportGroup("Visual")]
    [Export] public Texture2D IdleTexture { get; set; }
    [Export] public Texture2D ThrustTexture { get; set; }
    [Export] public float VisualScale { get; set; } = 0.20f;
    [Export] public bool FlipSpriteHorizontally { get; set; } = true;

    [ExportGroup("Debug")]
    [Export] public bool DebugEnabled { get; set; } = true;

    private const string ACTION_MOVE_LEFT = "move_left";
    private const string ACTION_MOVE_RIGHT = "move_right";
    private const string ACTION_MOVE_UP = "move_up";
    private const string ACTION_MOVE_DOWN = "move_down";
    private const string ACTION_FULL_STOP = "full_stop";
    private const string ACTION_TOGGLE_FLIGHT_MODE = "toggle_flight_mode";

    private Sprite2D _sprite;
    private Marker2D _drillPoint;
    private CollisionShape2D _collisionShape;

    private bool _isArcadeMode;

    public float CurrentSpeedMps => Velocity.Length();

    public override void _Ready()
    {
        _sprite = GetNode<Sprite2D>("Sprite2D");
        _drillPoint = GetNode<Marker2D>("DrillPoint");
        _collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");

        MotionMode = MotionModeEnum.Floating;

        ApplyVisualSetup();
        SetThrustVisual(false);

        if (Config == null)
        {
            GD.PushWarning("ShipConfig is missing on Ship.");
            return;
        }

        _isArcadeMode = Config.StartInArcadeMode;

        if (DebugEnabled)
        {
            GD.Print("Ship movement ready.");
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Config == null)
            return;

        float dt = (float)delta;

        if (Input.IsActionJustPressed(ACTION_TOGGLE_FLIGHT_MODE))
        {
            _isArcadeMode = !_isArcadeMode;

            if (DebugEnabled)
            {
                GD.Print(_isArcadeMode ? "Flight mode: ARCADE" : "Flight mode: REALISTIC");
            }
        }

        Vector2 desiredInput = GetDesiredInputVector();
        bool hasDesiredDirection = desiredInput != Vector2.Zero;
        bool fullStopPressed = Input.IsActionPressed(ACTION_FULL_STOP);

        bool thrustActive = false;

        if (fullStopPressed)
        {
            Velocity = Velocity.MoveToward(Vector2.Zero, Config.FullStopDecelerationMps2 * dt);
            SetThrustVisual(false);
            MoveAndSlide();
            return;
        }

        if (hasDesiredDirection)
        {
            float targetRotation = desiredInput.Angle();
            RotateTowards(targetRotation, dt);

            if (_isArcadeMode)
            {
                thrustActive = Config.ArcadeContinuousThrust;
            }
            else
            {
                // Realistic mode:
                // počas otáčania nespomaľujeme umelo, loď len driftuje zotrvačnosťou
                // a main engine sa zapne až po dorovnaní na želaný smer
                thrustActive = !Config.RealisticRequiresAlignment || IsAlignedTo(targetRotation);
            }
        }

        if (thrustActive)
        {
            Vector2 forward = Vector2.Right.Rotated(Rotation);
            Velocity += forward * Config.ThrustAccelerationMps2 * dt;
        }

        Velocity = Velocity.LimitLength(Config.MaxSpeedMps);

        SetThrustVisual(thrustActive);
        MoveAndSlide();
    }

    private Vector2 GetDesiredInputVector()
    {
        Vector2 input = Vector2.Zero;

        if (Input.IsActionPressed(ACTION_MOVE_LEFT))
            input.X -= 1.0f;

        if (Input.IsActionPressed(ACTION_MOVE_RIGHT))
            input.X += 1.0f;

        if (Input.IsActionPressed(ACTION_MOVE_UP))
            input.Y -= 1.0f;

        if (Input.IsActionPressed(ACTION_MOVE_DOWN))
            input.Y += 1.0f;

        return input.Normalized();
    }

    private bool ShouldBrakeForDirectionChange(float targetRotation)
    {
        if (CurrentSpeedMps < Config.MinSpeedForDirectionBrakeMps)
            return false;

        float velocityAngleDiff = Mathf.Abs(ShortestAngleDifference(Velocity.Angle(), targetRotation));
        float brakeAngleRad = Mathf.DegToRad(Config.DirectionChangeBrakeAngleDeg);

        return velocityAngleDiff >= brakeAngleRad;
    }

    private void RotateTowards(float targetRotation, float delta)
    {
        float angleDiff = ShortestAngleDifference(Rotation, targetRotation);
        float maxStep = Config.RotationSpeedRad * delta;
        Rotation += Mathf.Clamp(angleDiff, -maxStep, maxStep);
    }

    private bool IsAlignedTo(float targetRotation)
    {
        float angleDiff = Mathf.Abs(ShortestAngleDifference(Rotation, targetRotation));
        float toleranceRad = Mathf.DegToRad(Config.AlignmentToleranceDeg);
        return angleDiff <= toleranceRad;
    }

    private float ShortestAngleDifference(float from, float to)
    {
        return Mathf.Atan2(Mathf.Sin(to - from), Mathf.Cos(to - from));
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