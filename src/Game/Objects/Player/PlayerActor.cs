using System.Numerics;
using Engine.Input;
using Engine.Objects;
using Engine.Objects.ParticleSystem;
using Raylib_cs;

public class PlayerActor : Actor
{

    protected const int PlayerSpriteIndex = 2;
    protected const int PlayerWithoutDashSpriteIndex = PlayerSpriteIndex + 1;
    protected const float Acceleration = MoveSpeed * 10f;
    protected const float AirDrag = 25f * 10f;
    protected const float MoveSpeed = 50f;
    protected const float MinJumpHeight = 0.2f * GameStaticData.TileSize;
    protected const float MaxJumpHeight = 2f * GameStaticData.TileSize;
    protected const int MaxAirJumps = 0;

    protected const float DashDistance = 5f * GameStaticData.TileSize;
    protected const float DashAcceleration = 700f;

    protected InputSystem _inputSystem;
    protected SheetRenderer _sheetRenderer;

    protected Vector2 _lookDirection;

    protected Vector2 _moveVelocity;
    protected float _targetMoveVelocity;
    protected float _minJumpForce;
    protected float _jumpForce;

    protected Vector2 _dashVelocity;
    protected bool _hasDash;
    protected bool _isDashing;

    protected int _airJumps = 0;
    protected bool _isAirborneBecauseOfJump;

    protected float _dashForce;
    protected float _dashTime;

    protected List<FruitTrigger> _fruits = new List<FruitTrigger>();

    public PlayerActor(Vector2 position) : base(position, new BoxCollider(6, 6))
    {
        _inputSystem = DI.Get<InputSystem>();
        _sheetRenderer = new SheetRenderer("player", PlayerSpriteIndex);
        _sheetRenderer.DrawOrder = DrawableLayers.EntityLayer;
        SetRenderer(_sheetRenderer);
        
        _minJumpForce = (float)Math.Sqrt(MinJumpHeight * 2f * GameStaticData.Gravity);
        _jumpForce = (float)Math.Sqrt(MaxJumpHeight * 2f * GameStaticData.Gravity);
        _dashForce = (float)Math.Sqrt(DashDistance * 2f * DashAcceleration);

        _coreEngine.OnFrame += Frame;
        _lookDirection = Vector2.UnitX;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        _coreEngine.OnFrame -= Frame;
    }

    private void Frame(float deltatime)
    {
        Vector2 motion = Vector2.Zero;
        if (_inputSystem.IsKeyDown(_inputSystem.Right)) motion.X = 1f;
        else if (_inputSystem.IsKeyDown(_inputSystem.Left)) motion.X = -1f;
        if (_inputSystem.IsKeyDown(_inputSystem.Up)) motion.Y = 1f;
        else if (_inputSystem.IsKeyDown(_inputSystem.Down)) motion.Y = -1f;

        if (motion.LengthSquared() > 0)
        {
            _lookDirection = motion;
        }

        _targetMoveVelocity = motion.X * MoveSpeed;
        var currentAcceleration = _isGrounded ? Acceleration : AirDrag;
        _moveVelocity.X = MathHelper.MoveTowards(_moveVelocity.X, _targetMoveVelocity, currentAcceleration * deltatime);

        if (!_isDashing)
        {
            // if (_moveVelocity.Y > 0) _moveVelocity.Y -= GameStaticData.Gravity * deltatime;
            // else _moveVelocity.Y -= 2f * GameStaticData.Gravity * deltatime;
            _moveVelocity.Y -= GameStaticData.Gravity * deltatime;
        }

        if (_inputSystem.IsKeyPressed(_inputSystem.Jump))
        {
            if (_isGrounded)
            {
                Jump();
            }
            else if (_airJumps < MaxAirJumps)
            {
                _airJumps += 1;
                Jump();
            }
        }
        else if (_inputSystem.IsKeyReleased(_inputSystem.Jump) && _isAirborneBecauseOfJump && _moveVelocity.Y > _minJumpForce)
        {
            _moveVelocity.Y = _minJumpForce;
        }

        if (CanDash() && _inputSystem.IsKeyPressed(_inputSystem.Dash))
        {
            StartDash();
        }

        if (_isDashing)
        {
            _dashTime += deltatime;
            _dashVelocity = MathHelper.MoveTowards(_dashVelocity, Vector2.Zero, DashAcceleration * deltatime);
            if (_dashVelocity.LengthSquared() <= 0f) _isDashing = false;
        }

        var finalMotion = (_dashVelocity + _moveVelocity) * deltatime;
        MoveX(finalMotion.X, (solid) =>
        {
            _moveVelocity.X = 0f;
            _dashVelocity.X = 0f;
        });

        var lastIsGrounded = _isGrounded;
        MoveY(finalMotion.Y, (solid) =>
        {
            _moveVelocity.Y = 0f;
            _dashVelocity.Y = 0f;
        });

        if (finalMotion.X > 0) _renderer.FlipX = false;
        else if (finalMotion.X < 0) _renderer.FlipX = true;

        if (_isGrounded)
        {
            AllowDash();
            _isAirborneBecauseOfJump = false;

            if (!lastIsGrounded)
            {
                for (int i = 0; i < _fruits.Count; i++)
                {
                    var fruit = _fruits[i];
                    fruit.Collect(i);
                }
                _fruits.Clear();

                _objectSystem.Create(new ParticleEmitter(_position - new Vector2(0f, 4f), ParticleEmitterSettings.LandEffect()));
            }
        }
    }

    public void AllowDash()
    {
        _hasDash = true;
        _sheetRenderer.SetIndex(PlayerSpriteIndex);
        _airJumps = 0;
    }

    private bool CanDash()
    {
        return _lookDirection.LengthSquared() > 0 && _hasDash;
    }

    private void StartDash()
    {
        _isDashing = true;
        var dashDirection = Vector2.Normalize(_lookDirection);
        _dashVelocity = dashDirection * _dashForce;
        _dashTime = 1f;
        _moveVelocity = Vector2.Zero;
        _hasDash = false;
        _sheetRenderer.SetIndex(PlayerWithoutDashSpriteIndex);
        _objectSystem.Create(new ParticleEmitter(_position, ParticleEmitterSettings.DashEffect(_dashVelocity, dashDirection * DashAcceleration)));
    }

    public void Death()
    {
        _dashVelocity = Vector2.Zero;
        _moveVelocity = Vector2.Zero;
        _isDashing = false;
        _isAirborneBecauseOfJump = false;
        _hasDash = true;
        _fruits.Clear();
    }

    public void JumpPad()
    {
        var padForce = (float)Math.Sqrt(4f * GameStaticData.TileSize * 2f * GameStaticData.Gravity);
        _moveVelocity.Y = padForce;
        AllowDash();
    }

    private void Jump()
    {
        _moveVelocity.Y = _jumpForce;
        _isAirborneBecauseOfJump = true;
    }

    public override void Draw()
    {
        _renderer.Draw(_position);

        if (_isDashing && _dashTime > 0.06f)
        {
            _dashTime = 0f;
            var decal = new PlayerGhostDecal(_position, 0.5f);
            decal.Renderer.FlipX = _renderer.FlipX;
            _objectSystem.Create(decal);
        }
    }

    public IGameObject GetFruit(FruitTrigger fruit)
    {
        _fruits.Add(fruit);
        if (_fruits.Count == 1)
        {
            return this;
        }
        else
        {
            return _fruits[_fruits.Count - 2];
        }
    }
}