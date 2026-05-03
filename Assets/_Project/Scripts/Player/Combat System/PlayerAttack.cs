using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera _cam;
    [SerializeField] private AnimationManager _animator;
    [SerializeField] private Slash _slashPrefab;
    [SerializeField] private SlashPoolSystem _slashPoolSystem;

    [Header("Attack Settings")]
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private Transform _attackPointUp;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private int _damage = 5;
    [SerializeField] private float _attackRate = 0.3f;
    private float _lastAttackTime;

    void Awake()
    {
        if (_cam == null) _cam = Camera.main;
        if (_animator == null) _animator = GetComponent<AnimationManager>();
        if (_slashPoolSystem == null) _slashPoolSystem = FindObjectOfType<SlashPoolSystem>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time > _lastAttackTime + _attackRate)
        {
            _lastAttackTime = Time.time;
            Attack();
        }

        if (Input.GetKeyDown(KeyCode.W) && PlayerController.Instance.IsGrounded && Time.time > _lastAttackTime + _attackRate)
        {
            _lastAttackTime = Time.time;
            AttackUp();
        }
    }

    private void Attack()
    {
        _animator.Attack();
        SoundManager.Instance.PlaySFXSound("Attack");
        Slash();
    }

    private void AttackUp()
    {
        _animator.Attack();
        SoundManager.Instance.PlaySFXSound("Attack");
        _slashPoolSystem.SpawnSlash(_attackPointUp.position, Vector2.up, _damage, transform);
    }

    private void Slash()
    {
        float dirX = PlayerController.Instance.LastDirection;
        bool isGrounded = PlayerController.Instance.IsGrounded;

        if (!isGrounded)
        {
            Vector2 direction;
            Vector3 spawnPosition;

            if (Input.GetKey(KeyCode.W))
            {
                direction = Vector2.up;
                spawnPosition = _attackPointUp.position;
            }
            else
            {
                direction = new Vector2(dirX, 0);
                spawnPosition = _attackPoint.position;
                spawnPosition.x = transform.position.x + (Mathf.Abs(_attackPoint.localPosition.x) * dirX);
            }

            Transform parent;
            if (direction == Vector2.up)
                parent = transform;
            else
                parent = null;

            _slashPoolSystem.SpawnSlash(spawnPosition, direction, _damage, parent);
        }
        else
        {
            Vector2 direction = new Vector2(dirX, 0);
            Vector3 spawnPosition = _attackPoint.position;
            spawnPosition.x = transform.position.x + (Mathf.Abs(_attackPoint.localPosition.x) * dirX);
            _slashPoolSystem.SpawnSlash(spawnPosition, direction, _damage);
        }
    }
}
