using UnityEngine;

public class BatEnemyController : EnemiesController
{
    [Header("Bat Settings")]
    [SerializeField] private float _flySpeed = 3;
    [SerializeField] private int _damage = 5;
    [SerializeField] private string _hSpeedParam = "hSpeed";

    [Header("Patrol Settings")]
    [SerializeField] private Transform _pointA;
    [SerializeField] private Transform _pointB;
    [SerializeField] private float _patrolSpeed = 1f;
    [SerializeField] private float _pointReachDistance = 0.1f;
    [SerializeField] private Transform _playerPos;

    private Transform _currentPoint;


    void Start()
    {
        _currentPoint = _pointB;
    }

    protected override void IdleUpdate()
    {
        base.IdleUpdate();

        if (_currentState != STATE.Idle)
            return;

        Patrol();
        UpdateFacingAndAnimation(_rb.velocity.x);
    }

    private void Patrol()
    {
        float distance = Mathf.Abs(_currentPoint.position.x - transform.position.x);

        if (distance <= _pointReachDistance)
        {
            if (_currentPoint == _pointB)
                _currentPoint = _pointA;
            else
                _currentPoint = _pointB;

            _rb.velocity = Vector2.zero;
            return;
        }

        float direction = Mathf.Sign(_currentPoint.position.x - transform.position.x);
        _rb.velocity = new Vector2(direction * _patrolSpeed, 0);
    }

    protected override void ChaseUpdate()
    {
        base.ChaseUpdate();

        if (_currentState != STATE.Chase)
            return;

        Vector2 direction = _playerPos.position - transform.position;
        direction.Normalize();
        _rb.velocity = direction * _flySpeed;
        UpdateFacingAndAnimation(_rb.velocity.x);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerLifeController playerLife = collision.gameObject.GetComponent<PlayerLifeController>();

        if (playerLife == null)
            return;

        SoundManager.Instance.PlaySFXSound("Bat");
        playerLife.TakeDamage(_damage);
        SetDeath();
    }

    private void UpdateFacingAndAnimation(float hSpeed)
    {
        // setta la direzione del cono di visione
        if (_rb.velocity.x >= 0)
            _facingDirection = 1;
        else
            _facingDirection = -1;

        _animator.SetFloat(_hSpeedParam, hSpeed);
    }
}