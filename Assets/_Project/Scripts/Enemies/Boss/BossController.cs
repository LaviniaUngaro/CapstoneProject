using System;
using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public enum BossState { Entering, Idle, Shooting, Dead }
    public enum BossPhase { Phase1, Phase2 }

    [Header("References")]
    [SerializeField] private Transform _player;
    [SerializeField] private BulletPoolSystem _bulletPoolSystem;
    [SerializeField] private Transform[] _firePoints;
    [SerializeField] private Animator _animator;
    [SerializeField] private EnemyLifeController _lifeController;
    [SerializeField] private UI_BossLifebarAnimation _lifebarAnimation;
    [SerializeField] private SidePanels _sidePanelLeft;
    [SerializeField] private SidePanels _sidePanelRight;
    [SerializeField] private string _isEnteredParam = "isEntered";
    [SerializeField] private string _die = "isDead";

    [Header("Boss Settings")]
    [SerializeField] private int _contactDamage = 5;

    [Header("Phase 1")]
    [SerializeField] private float _p1FireRate = 0.7f;
    [SerializeField] private float _p1BulletSpeed = 6f;
    [SerializeField] private int _p1BulletDamage = 5;
    [SerializeField] private int _p1SpreadCount = 1;
    [SerializeField] private float _p1SpreadAngle = 0f;

    [Header("Phase Treshold")]
    [SerializeField] private float _phase2Treshold = 50;

    [Header("Phase 2")]
    [SerializeField] private float _p2FireRate = 0.4f;
    [SerializeField] private float _p2BulletSpeed = 9f;
    [SerializeField] private int _p2BulletDamage = 10;
    [SerializeField] private int _p2SpreadCount = 5;
    [SerializeField] private float _p2SpreadAngle = 60f;

    [Header("Burst")]
    [SerializeField] private int _burstCount = 3;
    [SerializeField] private float _burstInterval = 0.15f;


    private float _fireTimer;
    private bool _isDead;
    private bool _eneteringSoundPlayed = false;

    private BossState _state;
    private BossPhase _phase;

    void Awake()
    {
        if (_animator == null) _animator = GetComponent<Animator>();
        if (_lifeController == null) _lifeController = GetComponent<EnemyLifeController>();

        _state = BossState.Entering;
        _phase = BossPhase.Phase1;
    }

    void Update()
    {
        if (_isDead)
            return;

        CheckPhase();

        switch (_state)
        {
            case BossState.Entering:
                OnEntering();
                break;
            case BossState.Idle:
                break;
            case BossState.Shooting:
                HandleCombat();
                break;
            case BossState.Dead:
                HandleDeath();
                break;
        }
    }

    private void OnEntering()
    {
        if (_eneteringSoundPlayed)
            return;

        _eneteringSoundPlayed = true;
        SoundManager.Instance.PlaySFXSound("Boss Entering");
    }

    private void HandleDeath()
    {
        if (_isDead)
            return;

        _isDead = true;
        _animator.SetTrigger(_die);
        SoundManager.Instance.PlaySFXSound("Boss Death");
        SoundManager.Instance.FadeOutBackgroundMusic(3f);
        GameManager.Instance.Win();
    }

    #region Phase
    private void CheckPhase()
    {
        if (_phase == BossPhase.Phase1 && _lifeController.HP <= _phase2Treshold)
        {
            _phase = BossPhase.Phase2;
            OnPhase2Enter();
        }
    }

    private void OnPhase2Enter()
    {
        SoundManager.Instance.PlayBackgroundMusic("Boss Phase 2");
    }
    #endregion

    #region Combat
    private void HandleCombat()
    {
        _fireTimer -= Time.deltaTime;

        float fireRate = 0;
        if (_phase == BossPhase.Phase1)
            fireRate = _p1FireRate;
        else if (_phase == BossPhase.Phase2)
            fireRate = _p2FireRate;

        if (_fireTimer <= 0)
        {
            _fireTimer = fireRate;
            StartAttack();
        }
    }

    private void StartAttack()
    {
        if (_phase == BossPhase.Phase1)
            StartCoroutine(ShootSpread(_p1SpreadCount, _p1SpreadAngle, _p1BulletSpeed, _p1BulletDamage));
        else
            StartCoroutine(ShootBurst());
    }

    private IEnumerator ShootSpread(int count, float totalAngle, float speed, int damage)
    {
        if (_player == null)
            yield break;

        SoundManager.Instance.PlaySFXSound("Fireball");

        Vector2 baseDirection = _player.position - _firePoints[0].position;
        baseDirection.Normalize();

        float step;
        if (count > 1)
            step = totalAngle / (count - 1);
        else
            step = 0;

        float startAngle = -totalAngle / 2f;

        foreach (Transform firepoint in _firePoints)
        {
            for (int i = 0; i < count; i++)
            {
                float angle = startAngle + step * i;
                Vector2 direction = Quaternion.Euler(0, 0, angle) * baseDirection;
                _bulletPoolSystem.SpawnBullet(firepoint.position, direction, speed, damage);
            }
        }
        yield return null;
    }

    private IEnumerator ShootBurst()
    {
        for (int i = 0; i < _burstCount; i++)
        {
            yield return StartCoroutine(ShootSpread(_p2SpreadCount, _p2SpreadAngle, _p2BulletSpeed, _p2BulletDamage));

            if (i < _burstCount - 1)
            {
                SoundManager.Instance.PlaySFXSound("Fireball");
                yield return new WaitForSeconds(_burstInterval);
            }
        }
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerLifeController playerLife = collision.gameObject.GetComponent<PlayerLifeController>();

        if (playerLife == null)
            return;

        playerLife.TakeDamage(_contactDamage);
    }

    // x evento
    private void OnEntryAnimationFinished()
    {
        _state = BossState.Shooting;
        _animator.SetBool(_isEnteredParam, true);
        SoundManager.Instance.PlayBackgroundMusic("Boss Phase 1");
        _lifebarAnimation.ShowHealthBar();
        _sidePanelLeft.ShowPanel();
        _sidePanelRight.ShowPanel();
    }

    public void Die()
    {
        _state = BossState.Dead;
        StopAllCoroutines();
    }
}