using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Animator _animator;

    [Header("Fireball Settings")]
    [SerializeField] private int _damage;
    [SerializeField] private float _lifeTime = 3f;
    [SerializeField] private float _bulletBornTime;
    private Vector2 _direction;

    [Header("Animation References")]
    [SerializeField] private string _hit = "hit";
    [SerializeField] private string _isLeft = "isLeft";

    private IObjectPool<Bullet> _bulletPool;
    private bool _isActive = false;

    void Awake()
    {
        if (_rb == null) _rb = GetComponent<Rigidbody2D>();
        if (_animator == null) _animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        _isActive = true;
        _bulletBornTime = Time.time;
    }

    void Update()
    {
        if (Time.time > _bulletBornTime + _lifeTime)
            ReturnToPool();
    }

    public void Setup(Vector2 direction, float speed, int damage)
    {
        _damage = damage;
        _direction = direction;
        _rb.velocity = Vector2.zero;
        _rb.velocity = direction * speed;
    }

    public void SetPool(IObjectPool<Bullet> pool)
    {
        _bulletPool = pool;
    }

    public void ReturnToPool()
    {
        if (!_isActive)
            return;

        _isActive = false;
        _rb.velocity = Vector2.zero;
        _bulletPool?.Release(this);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _rb.velocity = Vector2.zero;

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
