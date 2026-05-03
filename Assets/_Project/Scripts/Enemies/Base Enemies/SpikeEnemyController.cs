using System.Collections;
using UnityEngine;

public class SpikeEnemyController : EnemiesController
{
    [Header("Spikes References")]
    [SerializeField] private Collider2D[] _spikesColliders;

    [Header("Spikes Settings")]
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _warningTime = 0.5f;
    [SerializeField] private float _activeTime = 1f;
    [SerializeField] private float _attackCooldown = 4f;

    [Header("Animation References")]
    [SerializeField] private string _directionParam = "direction";
    [SerializeField] private string _isAttackingParam = "isAttacking";

    private bool _isOnAttack;
    private float _cooldownTimer;

    protected override void Awake()
    {
        base.Awake();
        SetSpikesActive(false);
    }

    protected override void ChaseUpdate()
    {
        base.ChaseUpdate();

        if (_currentState != STATE.Chase)
            return;

        MoveToPlayer();
    }

    protected override void OnEnterAttack()
    {
        UpdateFacing();
        StartCoroutine(AttackRoutine());
    }

    protected override void OnExitAttack()
    {
        StopAllCoroutines();
        _isOnAttack = false;
        SetSpikesActive(false);
    }

    protected override void AttackUpdate()
    {
        if (!_isOnAttack)
            base.AttackUpdate();
    }

    private IEnumerator AttackRoutine()
    {
        _isOnAttack = true;
        _animator.SetFloat(_directionParam, _facingDirection);
        _animator.SetTrigger(_isAttackingParam);

        yield return new WaitForSeconds(_warningTime + _activeTime);

        _isOnAttack = false;
        SetState(STATE.Cooldown);
    }

    protected override void OnEnterCooldown()
    {
        _cooldownTimer = _attackCooldown;
    }

    protected override void CooldownUpdate()
    {
        _cooldownTimer -= Time.fixedDeltaTime;
        MoveToPlayer();

        if (_cooldownTimer <= 0)
        {
            if (PlayerInAggroDistance())
                SetState(STATE.Chase);
            else
                SetState(STATE.Idle);
        }
    }

    private void SetSpikesActive(bool active)
    {
        foreach (Collider2D collider in _spikesColliders)
        {
            if (collider != null)
                collider.enabled = active;
        }
    }

    private void UpdateFacing()
    {
        if (PlayerController.Instance == null)
            return;

        if (PlayerController.Instance.transform.position.x >= transform.position.x)
            _facingDirection = 1;
        else
            _facingDirection = -1;

        _animator.SetFloat(_directionParam, _facingDirection);
    }

    protected override bool CanAttackPlayer()
    {
        return DistanceToPlayer() <= _attackDistance * _attackDistance;
    }

    private void MoveToPlayer()
    {
        if (PlayerController.Instance == null)
            return;
            
        Vector2 direction = PlayerController.Instance.transform.position - transform.position;
        direction.Normalize();
        _rb.velocity = direction * _moveSpeed;
        UpdateFacing();
    }

    private void SpikesOut()
    {
        SetSpikesActive(true);
        SoundManager.Instance.PlaySFXSound("Spikes");
    }

    private void SpikesIn()
    {
        SetSpikesActive(false);
    }
}