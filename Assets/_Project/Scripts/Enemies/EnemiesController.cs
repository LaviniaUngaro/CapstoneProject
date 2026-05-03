using UnityEngine;

public class EnemiesController : MonoBehaviour
{
    public enum STATE { Idle, Chase, Attack, Cooldown, Death, Knockback }

    [Header("States Attributes")]
    [SerializeField] protected STATE _currentState;

    [Header("References")]
    [SerializeField] protected Rigidbody2D _rb;
    [SerializeField] protected EnemyTargetDetection _targetDetection;
    [SerializeField] protected Animator _animator;

    [Header("Enemies Settings")]
    [SerializeField] protected float _aggroDistance = 5f;
    [SerializeField] protected float _loseSightDelay = 2f;
    [SerializeField] protected float _attackDistance = 1.5f;

    protected float _loseSightTimer;
    protected int _facingDirection = 1;

    public int FacingDirection => _facingDirection;

    protected virtual void Awake()
    {
        if (_rb == null) _rb = GetComponent<Rigidbody2D>();
        if (_targetDetection == null) _targetDetection = GetComponent<EnemyTargetDetection>();
        if (_animator == null) _animator = GetComponent<Animator>();

        SetState(STATE.Idle);
    }
    
    protected virtual void FixedUpdate()
    {
        if (_currentState != STATE.Knockback)
        {
            if (_targetDetection.CanSeeTarget())
                _loseSightTimer = _loseSightDelay;
            else
                _loseSightTimer = Mathf.Max(0, _loseSightTimer - Time.fixedDeltaTime);

            StateUpdate();
        }
    }

    #region StateUpdate
    protected virtual void StateUpdate()
    {
        switch (_currentState)
        {
            case STATE.Idle:
                IdleUpdate();
                break;
            case STATE.Chase:
                ChaseUpdate();
                break;
            case STATE.Attack:
                AttackUpdate();
                break;
            case STATE.Cooldown:
                CooldownUpdate();
                break;
            case STATE.Death:
                DeathUpdate();
                break;
        }
    }

    protected virtual void IdleUpdate()
    {
        if (PlayerInAggroDistance())
        {
            SetState(STATE.Chase);
            return;
        }
    }

    protected virtual void ChaseUpdate()
    {
        if (CanAttackPlayer())
        {
            SetState(STATE.Attack);
            return;
        }

        if (!PlayerInAggroDistance())
        {
            SetState(STATE.Idle);
            return;
        }
    }

    protected virtual void AttackUpdate()
    {
        if (!CanAttackPlayer())
            SetState(STATE.Chase);
    }

    protected virtual void CooldownUpdate()
    {
        if (_loseSightTimer <= 0)
            SetState(STATE.Idle);
    }

    protected virtual void DeathUpdate() { }

    #endregion

    #region SetState, OnExitState, OnEnterState
    public void SetState(STATE state)
    {
        if (_currentState == state)
            return;

        ExitState();
        _currentState = state;
        EnterState();
    }

    protected virtual void ExitState()
    {
        switch (_currentState)
        {
            case STATE.Idle:
                OnExitIdle();
                break;
            case STATE.Chase:
                OnExitChase();
                break;
            case STATE.Attack:
                OnExitAttack();
                break;
            case STATE.Cooldown:
                OnExitCooldown();
                break;
            case STATE.Death:
                OnExitDeath();
                break;
        }
    }

    protected virtual void OnExitIdle() { }

    protected virtual void OnExitChase() { }

    protected virtual void OnExitAttack() { }

    protected virtual void OnExitCooldown() { }

    protected virtual void OnExitDeath() { }


    protected virtual void EnterState()
    {
        switch (_currentState)
        {
            case STATE.Idle:
                OnEnterIdle();
                break;
            case STATE.Chase:
                OnEnterChase();
                break;
            case STATE.Attack:
                OnEnterAttack();
                break;
            case STATE.Cooldown:
                OnEnterCooldown();
                break;
            case STATE.Death:
                OnEnterDeath();
                break;
        }
    }

    protected virtual void OnEnterIdle()
    {
        StopMovement();
    }

    protected virtual void OnEnterChase() { }

    protected virtual void OnEnterAttack()
    {
        StopMovement();
    }

    protected virtual void OnEnterCooldown()
    {
        StopMovement();
    }

    protected virtual void OnEnterDeath()
    {
        StopMovement();

        if (_rb != null)
            _rb.simulated = false;

        _animator.SetBool("isDead", true);
        Destroy(gameObject, 1f);
    }

    #endregion

    protected float DistanceToPlayer()
    {
        if (PlayerController.Instance == null)
            return Mathf.Infinity;

        return (PlayerController.Instance.transform.position - transform.position).sqrMagnitude;
    }

    protected virtual bool PlayerInAggroDistance()
    {
        if (PlayerController.Instance == null)
            return false;

        return DistanceToPlayer() <= _aggroDistance * _aggroDistance;
    }

    protected virtual bool CanAttackPlayer()
    {
        return DistanceToPlayer() <= _attackDistance * _attackDistance && _loseSightTimer > 0;
    }

    protected virtual void StopMovement()
    {
        if (_rb == null)
            return;

        _rb.velocity = new Vector2(0, _rb.velocity.y);
    }

    public void SetDeath()
    {
        SetState(STATE.Death);
    }
}
