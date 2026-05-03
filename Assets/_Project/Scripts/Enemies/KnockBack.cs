using System.Collections;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private EnemiesController _enemies;

    void Awake()
    {
        if (_rb == null) _rb = GetComponent<Rigidbody2D>();
        if (_enemies == null) _enemies = GetComponent<EnemiesController>();
    }

    public void Knockback(Transform player, float knockbackForce, float stunTime)
    {
        _enemies.SetState(EnemiesController.STATE.Knockback);
        StartCoroutine(StunTimer(stunTime));
        Vector2 direction = (transform.position - player.transform.position);
        direction.Normalize();
        _rb.velocity = direction * knockbackForce;
    }

    private IEnumerator StunTimer(float stunTime)
    {
        yield return new WaitForSeconds(stunTime);
        _rb.velocity = Vector2.zero;
        _enemies.SetState(EnemiesController.STATE.Idle);
    }
}
