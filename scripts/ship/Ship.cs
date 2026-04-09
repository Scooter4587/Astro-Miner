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

    private const string ACTION_MOVE_LEFT = "move_left";
    private const string ACTION_MOVE_RIGHT = "move_right";
    private const string ACTION_MOVE_UP = "move_up";
    private const string ACTION_MOVE_DOWN = "move_down";
    private const string ACTION_FULL_STOP = "full_stop";
    private const string ACTION_TOGGLE_FLIGHT_MODE = "toggle_flight_mode";
    private const string ACTION_DRILL_HOLD = "drill_hold";

    private Sprite2D _sprite;
    private Marker2D _drillPoint;
    private CollisionShape2D _collisionShape;

    private bool _isArcadeMode;
    private float _flightBounceCooldownTimer = 0.0f;

    public float CurrentSpeedMps => Velocity.Length();
    public bool IsArcadeMode => _isArcadeMode;

    private struct DrillTargetInfo
    {
        public TileMapLayer Layer;
        public Vector2 WorldHitPosition;
        public Vector2 LocalHitPosition;
        public Vector2I Cell;
        public bool HasTile;
    }

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

        Debug.LogShipGeneral("Ship movement ready.");
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Config == null)
            return;

        float dt = (float)delta;
        _flightBounceCooldownTimer = Mathf.Max(0.0f, _flightBounceCooldownTimer - dt);

        if (Input.IsActionJustPressed(ACTION_TOGGLE_FLIGHT_MODE))
        {
            _isArcadeMode = !_isArcadeMode;
            Debug.LogShipGeneral(_isArcadeMode ? "Flight mode: ARCADE" : "Flight mode: REALISTIC");
        }

        Vector2 desiredInput = GetDesiredInputVector();
        bool hasDesiredDirection = desiredInput != Vector2.Zero;
        bool fullStopPressed = Input.IsActionPressed(ACTION_FULL_STOP);

        bool thrustActive = false;

        if (fullStopPressed)
        {
            Velocity = Velocity.MoveToward(Vector2.Zero, Config.FullStopDecelerationMps2 * dt);
            SetThrustVisual(false);
        }
        else
        {
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
        }

        Vector2 velocityBeforeMove = Velocity;
        MoveAndSlide();
        ApplyCollisionDamping(velocityBeforeMove);

        HandleDrillDetectionDebug();
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

    private void ApplyCollisionDamping(Vector2 velocityBeforeMove)
    {
        int collisionCount = GetSlideCollisionCount();
        if (collisionCount == 0)
            return;

        float strongestImpactSpeed = 0.0f;
        Vector2 strongestNormal = Vector2.Zero;

        for (int i = 0; i < collisionCount; i++)
        {
            KinematicCollision2D collision = GetSlideCollision(i);
            Vector2 normal = collision.GetNormal().Normalized();

            // koľko rýchlosti išlo proti stene
            float impactSpeed = Mathf.Max(0.0f, -velocityBeforeMove.Dot(normal));

            if (impactSpeed > strongestImpactSpeed)
            {
                strongestImpactSpeed = impactSpeed;
                strongestNormal = normal;
            }
        }

        if (strongestImpactSpeed <= 0.0f)
            return;

        // Rozklad rýchlosti na časť do normály a časť po povrchu.
        float normalDot = velocityBeforeMove.Dot(strongestNormal);
        Vector2 normalComponent = strongestNormal * normalDot;
        Vector2 tangentComponent = velocityBeforeMove - normalComponent;

        // Základ pre side/scrape feel.
        Vector2 resultVelocity = tangentComponent * Config.ImpactTangentPreserve;

        if (strongestImpactSpeed >= Config.CrashSpeedThresholdMps)
        {
            resultVelocity *= (1.0f - Config.CrashExtraDamping);
        }

        // Ako veľmi bol náraz "čelný".
        float impactAlignment = 0.0f;
        if (velocityBeforeMove.LengthSquared() > 0.0001f)
        {
            impactAlignment = Mathf.Max(
                0.0f,
                -velocityBeforeMove.Normalized().Dot(strongestNormal)
            );
        }

        bool isHeadOn =
            impactAlignment >= Config.FlightHeadOnDotThreshold &&
            strongestImpactSpeed >= Config.FlightBounceMinImpactSpeedMps;

        if (isHeadOn && _flightBounceCooldownTimer <= 0.0f)
        {
            // Malý kontrolovaný rebound iba pre flight baseline.
            Vector2 bounceVelocity = velocityBeforeMove.Bounce(strongestNormal) * Config.FlightBounceMultiplier;

            // Zober silnejšiu z možností, aby head-on nepôsobil ako úplné zapichnutie.
            if (bounceVelocity.Length() > resultVelocity.Length())
            {
                resultVelocity = bounceVelocity;
            }

            _flightBounceCooldownTimer = Config.FlightBounceCooldownSec;
        }

        Velocity = resultVelocity;
    }

    private void HandleDrillDetectionDebug()
    {
        if (!Input.IsActionPressed(ACTION_DRILL_HOLD))
        {
            Debug.ClearChannel("drill_detection");
            return;
        }

        if (CurrentSpeedMps > Config.DrillSafeSpeedMps)
        {
            PrintDrillDebugMessage("DRILL | blocked: speed too high");
            return;
        }

        if (TryGetDrillTarget(out DrillTargetInfo target))
        {
            PrintDrillDebugMessage(
                $"DRILL | layer={target.Layer.Name} | world={target.WorldHitPosition} | local={target.LocalHitPosition} | cell={target.Cell} | has_tile={target.HasTile}"
            );
        }
        else
        {
            PrintDrillDebugMessage("DRILL | no valid target");
        }
    }

    private void PrintDrillDebugMessage(string message)
    {
        Debug.LogDrillDetection(message, dedupe: true);
    }

    private bool TryGetDrillTarget(out DrillTargetInfo target)
    {
        target = default;

        if (_drillPoint == null || Config == null)
            return false;

        if (CurrentSpeedMps > Config.DrillSafeSpeedMps)
            return false;

        Vector2 from = _drillPoint.GlobalPosition;
        Vector2 to = from + Vector2.Right.Rotated(Rotation) * Config.DrillDetectDistancePx;

        var exclude = new Godot.Collections.Array<Rid> { GetRid() };
        var query = PhysicsRayQueryParameters2D.Create(from, to, CollisionMask, exclude);
        query.CollideWithBodies = true;
        query.CollideWithAreas = false;
        query.HitFromInside = false;

        var result = GetWorld2D().DirectSpaceState.IntersectRay(query);
        if (result.Count == 0)
            return false;

        Rid hitRid = (Rid)result["rid"];
        TileMapLayer layer = FindTileMapLayerByBodyRid(GetTree().CurrentScene, hitRid);

        if (layer == null)
            return false;

        Vector2 worldHit = (Vector2)result["position"];
        Vector2 localHit = layer.ToLocal(worldHit);
        Vector2I cell = layer.LocalToMap(localHit);
        bool hasTile = layer.GetCellSourceId(cell) != -1;

        if (!hasTile)
            return false;

        target = new DrillTargetInfo
        {
            Layer = layer,
            WorldHitPosition = worldHit,
            LocalHitPosition = localHit,
            Cell = cell,
            HasTile = true
        };

        return true;
    }

    private TileMapLayer FindTileMapLayerByBodyRid(Node root, Rid bodyRid)
    {
        if (root == null)
            return null;

        if (root is TileMapLayer layer && layer.HasBodyRid(bodyRid))
            return layer;

        foreach (Node child in root.GetChildren())
        {
            TileMapLayer found = FindTileMapLayerByBodyRid(child, bodyRid);
            if (found != null)
                return found;
        }

        return null;
    }
}