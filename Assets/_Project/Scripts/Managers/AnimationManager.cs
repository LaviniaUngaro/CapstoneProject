using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator _animator;

    [Header("Animation Settings")]
    [SerializeField] private string _direction = "direction";
    [SerializeField] private string _isAttacking = "isAttacking";
    [SerializeField] private string _takeDamage = "takeDamage";
    [SerializeField] private string _isDead = "isDead";

    void Awake()
    {
        if (_animator == null) _animator = GetComponentInChildren<Animator>();
    }

    public void SetDirection(float direction)
    {
        _animator.SetFloat(_direction, direction);
    }

    public void Attack()
    {
        _animator.SetTrigger(_isAttacking);
    }

    public void Dead()
    {
        _animator.SetBool(_isDead, true);
    }

    public void Damage()
    {
        _animator.SetTrigger(_takeDamage);
    }
}
