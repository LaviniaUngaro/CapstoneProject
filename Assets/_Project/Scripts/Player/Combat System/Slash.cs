using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Slash : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator _slashAnimator;
    [SerializeField] private SpriteRenderer _slashSpriteRenderer;
    [SerializeField] private string _directionParam = "direction";
    [SerializeField] private SlashPoolSystem _poolParent;

    [Header("Attack Settings")]
    [SerializeField] private int _damage = 5;
    [SerializeField] private float _knockbackForce = 20f;
    [SerializeField] private float _stunTime = 1f;
    [SerializeField] private float _lifeTime = 0.6f;
    [SerializeField] private float _slashBornTime;

    private IObjectPool<Slash> _slashPool;
    private bool _isActive = false;

    // property
    public int Damage => _damage;

    void Awake()
    {
        if (_slashAnimator == null) _slashAnimator = GetComponent<Animator>();
        if (_poolParent == null) _poolParent = FindObjectOfType<SlashPoolSystem>();
    }

    void OnEnable()
    {
        _isActive = true;
        _slashBornTime = Time.time;
    }

    void Update()
    {
        if (Time.time > _slashBornTime + _lifeTime)
            ReturnToPool();
    }

    public void Setup(Vector2 direction, int damage)
    {
        _damage = damage;

        bool isUpAttack = direction.y > 0;
        _slashAnimator.SetBool("isUpAttack", isUpAttack);

        float dir = Mathf.Sign(direction.x);
        _slashAnimator.SetFloat(_directionParam, dir);
        _slashSpriteRenderer.flipX = dir < 0;
    }

    public void SetPool(IObjectPool<Slash> pool)
    {
        _slashPool = pool;
    }

    public void ReturnToPool()
    {
        if (!_isActive)
            return;

        _isActive = false;
        transform.SetParent(_poolParent.transform);
        _slashPool?.Release(this);
    }

    public void SetPoolParent(SlashPoolSystem poolParent)
    {
        _poolParent = poolParent;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        EnemyLifeController enemy = other.GetComponent<EnemyLifeController>();
        if (enemy == null)
            return;

        enemy.TakeDamage(_damage);

        if (other.TryGetComponent<KnockBack>(out var enemyKnockback))
            enemyKnockback.Knockback(transform, _knockbackForce, _stunTime);

        ReturnToPool();
    }
}
