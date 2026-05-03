using System;
using UnityEngine;
using UnityEngine.Events;

public class LifeController : MonoBehaviour
{
    [SerializeField] protected int _currentHP = 100;
    [SerializeField] protected int _maxHP = 100;

    protected bool _isDead;

    protected event Action OnHeal;
    protected event Action OnDamage;
    protected event Action OnDeath;

    [SerializeField] protected UnityEvent<int, int> _onLifeChanged;

    public int HP => _currentHP;
    public int MaxHP => _maxHP;
    public bool IsDead => _isDead;

    public void SetHP(int hp)
    {
        hp = Mathf.Clamp(hp, 0, _maxHP);

        if (hp != _currentHP)
        {
            _currentHP = hp;
            _onLifeChanged.Invoke(_currentHP, _maxHP);
        }
    }

    public void AddHp(int amount)
    {
        SetHP(HP + amount);
        OnHeal?.Invoke();
    }

    public void TakeDamage(int amount)
    {
        if (_isDead)
            return;

        SetHP(_currentHP - amount);
        OnDamage?.Invoke();
        Debug.Log($"{gameObject.name} ha subito {amount} di danno. Vita rimanente {HP}");

        if (HP <= 0)
        {
            Die();
            return;
        }
    }

    public void Die()
    {
        if (_isDead)
            return;

        _isDead = true;
        OnDeath?.Invoke();
    }
}
