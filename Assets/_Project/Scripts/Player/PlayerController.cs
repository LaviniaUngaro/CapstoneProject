using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    [Header("References")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private AnimationManager _animator;

    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _lastDirection = 1f;
    private float _moveInput;
    private bool _isInputEnambled;

    [Header("Jumping")]
    [SerializeField] private float _jumpHeight = 8f;
    [SerializeField] private float _fallMultiplier = 2.5f;
    [SerializeField] private CollisionChecker _groundChecker;
    private bool _isGrounded;

    [Header("Wall Sliding")]
    [SerializeField] private float _wallSlideSpeed = 3f;
    [SerializeField] private CollisionChecker _wallCheckerRight;
    [SerializeField] private CollisionChecker _wallCheckerLeft;
    private bool _isTouchingWall;

    [Header("Wall Jumping")]
    [SerializeField] private float _wallJumpHeightX = 5f;
    [SerializeField] private float _wallJumpHeightY = 8f;
    [SerializeField] private float _wallJumpDuration = 0.2f;
    private bool _isWallJumping;
    private float _wallJumpTimer;

    public float LastDirection => _lastDirection;
    public bool IsGrounded => _isGrounded;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (_rb == null) _rb = GetComponent<Rigidbody2D>();
        if (_groundChecker == null) _groundChecker = GetComponentInChildren<CollisionChecker>();
        if (_animator == null) _animator = GetComponent<AnimationManager>();
    }

    private void Update()
    {
        _moveInput = Input.GetAxisRaw("Horizontal");
        if (_moveInput != 0)
            _lastDirection = Mathf.Sign(_moveInput);

        _animator.SetDirection(_lastDirection);

        // controllo se il player sta collidendo con ground o walls
        _isGrounded = _groundChecker.IsColliding;
        _isTouchingWall = _wallCheckerRight.IsColliding || _wallCheckerLeft.IsColliding;

        if (Input.GetButtonDown("Jump") && _isGrounded)
            Jump();

        if (Input.GetButtonDown("Jump") && CanWallSlide())
            WallJump();

        if (_isWallJumping)
        {
            _wallJumpTimer -= Time.deltaTime;
            if (_wallJumpTimer <= 0)
                _isWallJumping = false;
        }
    }

    private void FixedUpdate()
    {
        if (!_isWallJumping)
        {
            _rb.velocity = new Vector2(_moveInput * _moveSpeed, _rb.velocity.y);

            if (_moveInput != 0 && _isGrounded)
                SoundManager.Instance.PlaySFXLoopSound("Walk");
            else
                SoundManager.Instance.StopSFXLoopSound();
        }

        if (CanWallSlide())
            WallSliding();

        if (_rb.velocity.y < 0)
            _rb.velocity += Vector2.up * Physics2D.gravity.y * (_fallMultiplier - 1) * Time.fixedDeltaTime;
    }

    private void Jump()
    {
        _rb.velocity = new Vector2(_rb.velocity.x, _jumpHeight);
    }

    // se sono in aria, sto toccando un muro e mi muovo, posso scivolare
    private bool CanWallSlide()
    {
        return !_isGrounded && _isTouchingWall && _moveInput != 0;
    }

    // scivolo più lentamente
    private void WallSliding()
    {
        _rb.velocity = new Vector2(_rb.velocity.x, Mathf.Max(_rb.velocity.y, -_wallSlideSpeed));
    }

    private void WallJump()
    {
        _isWallJumping = true;
        _wallJumpTimer = _wallJumpDuration;

        // restituisce la direzione in cui il player si sta muovendo
        int wallDirection = (int)Mathf.Sign(_moveInput);

        _rb.velocity = new Vector2(_wallJumpHeightX * -wallDirection, _wallJumpHeightY);
    }

    public void SetKinematic(bool isKinematic)
    {
        _rb.isKinematic = isKinematic;
        if (isKinematic)
        {
            _rb.velocity = Vector2.zero;
            _rb.angularVelocity = 0;
        }
    }

    public void SetInputEnabled(bool enabled)
    {
        _isInputEnambled = enabled;
        if (!enabled)
        {
            _moveInput = 0;
            _rb.velocity = new Vector2(0, _rb.velocity.y);
        }
    }
}
