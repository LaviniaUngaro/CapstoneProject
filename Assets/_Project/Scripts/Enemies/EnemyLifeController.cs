using UnityEngine;
using UnityEngine.Events;

public class EnemyLifeController : LifeController
{
    [SerializeField] private Animator _animator;
    [SerializeField] private string _isHitted = "isHitted";

    void Awake()
    {
        if (_animator == null) _animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        OnDamage += HandleDamage;
        OnDeath += HandleDeath;
    }

    void OnDisable()
    {
        OnDamage -= HandleDamage;
        OnDeath -= HandleDeath;
    }

    public void HandleDamage()
    {
        _animator.SetTrigger(_isHitted);
        SoundManager.Instance.PlaySFXSound("Enemies Damage");
    }

    public void HandleDeath()
    {
        EnemiesController enemy = GetComponent<EnemiesController>();
        if (enemy != null)
        {
            enemy.SetDeath();
            SoundManager.Instance.PlaySFXSound("Enemies Death");
        }

        BossController boss = GetComponent<BossController>();
        if (boss != null)
        {
            boss.Die();
            SoundManager.Instance.PlaySFXSound("Boss Death");
        }
    }
}
