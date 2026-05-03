using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyFireball : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Animator _animator;
    [SerializeField] private Collider2D _collider;

    [Header("Fireball Settings")]
    [SerializeField] private int _damage;
    [SerializeField] private float _lifeTime = 3f;
    [SerializeField] private float _fireballBornTime;
    private Vector2 _direction;

    [Header("Animation References")]
    [SerializeField] private string _hit = "hit";
    [SerializeField] private string _isLeft = "isLeft";

    private IObjectPool<EnemyFireball> _fireballPool;
    private bool _isActive = false;


    void Awake()
    {
        if (_rb == null) _rb = GetComponent<Rigidbody2D>();
        if (_animator == null) _animator = GetComponent<Animator>();
        if (_collider == null) _collider = GetComponent<Collider2D>();
    }

    void OnEnable()
    {
        _isActive = true;
        _fireballBornTime = Time.time;
        _collider.enabled = true;
    }

    void Update()
    {
        if (Time.time > _fireballBornTime + _lifeTime)
            ReturnToPool();
    }

    public void Setup(Vector2 direction, float speed, int damage)
    {
        _damage = damage;
        _direction = direction;
        _rb.velocity = direction * speed;
    }

    public void SetPool(IObjectPool<EnemyFireball> pool)
    {
        _fireballPool = pool;
    }

    public void ReturnToPool()
    {
        if (!_isActive)
            return;

        _isActive = false;
        _rb.velocity = Vector2.zero;
        _fireballPool?.Release(this);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _rb.velocity = Vector2.zero;
        _collider.enabled = false;

        _animator.SetBool(_isLeft, _direction.x < 0);
        _animator.SetTrigger(_hit);

        PlayerLifeController _playerLife = collision.gameObject.GetComponent<PlayerLifeController>();
        if (_playerLife != null)
            _playerLife.TakeDamage(_damage);

        StartCoroutine(ReturnToPoolDelayed(0.3f));
    }

    private IEnumerator ReturnToPoolDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);
        ReturnToPool();
    }
}
