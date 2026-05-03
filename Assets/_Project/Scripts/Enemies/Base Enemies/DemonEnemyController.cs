using System.Collections;
using UnityEngine;

public class DemonEnemyController : EnemiesController
{
    [Header("Fireball References")]
    [SerializeField] private Transform _firePoint;
    [SerializeField] private FireballPoolSystem _fireballPoolSystem;

    [Header("Fireball Settings")]
    [SerializeField] private float _fireballCooldown = 2f;
    [SerializeField] private float _fireballSpeed = 3f;
    [SerializeField] private int _fireballDamage = 10;

    [Header("Animation References")]
    [SerializeField] private string _directionParam = "direction";
    
    private float _lastShootTime;
    private bool _isAttacking = false;
    
    protected override void Awake()
    {
        base.Awake();
        if (_fireballPoolSystem == null) _fireballPoolSystem = FindObjectOfType<FireballPoolSystem>();
    }

    protected override void ChaseUpdate()
    {
        base.ChaseUpdate();

        if (_currentState != STATE.Chase)
            return;

        SetFacingToPlayer();
        UpdateAnimation();
    }

    protected override void AttackUpdate()
    {
        if (!_isAttacking)
            base.AttackUpdate();

        if (_currentState != STATE.Attack)
            return;

        SetFacingToPlayer();
        UpdateAnimation();

        if (!_isAttacking && Time.time > _lastShootTime + _fireballCooldown)
        {
            _lastShootTime = Time.time;
            StartCoroutine(AttackRoutine());
        }
    }

    protected override void OnExitAttack()
    {
        StopAllCoroutines();
        _isAttacking = false;
    }

    private IEnumerator AttackRoutine()
    {
        _isAttacking = true;
        _animator.SetTrigger("attack");
        yield return new WaitForSeconds(0.35f);

        Shoot();
        SoundManager.Instance.PlaySFXSound("Fireball");
        yield return new WaitForSeconds(1f);

        _isAttacking = false;
    }

    private void Shoot()
    {
        Vector3 spawnPosition = _firePoint.position;
        spawnPosition.x = transform.position.x + (Mathf.Abs(_firePoint.localPosition.x) * _facingDirection);
        
        Vector2 direction = PlayerController.Instance.transform.position - _firePoint.transform.position;
        direction.Normalize();

        _fireballPoolSystem.SpawnFireball(spawnPosition, direction, _fireballSpeed, _fireballDamage);
    }

    private void UpdateAnimation()
    {
        if (PlayerController.Instance == null)
            return;

        _animator.SetFloat(_directionParam, _facingDirection);
    }

    private void SetFacingToPlayer()
    {
        if (PlayerController.Instance == null)
            return;

        float direction = PlayerController.Instance.transform.position.x - transform.position.x;
        if (direction >= 0)
            _facingDirection = 1;
        else
            _facingDirection = -1;
    }
}