using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarmfulBushes : MonoBehaviour
{
    [SerializeField] private int _damage = 5;
    [SerializeField] private float _damageCooldown = 2f;

    private float _lastDamage = -Mathf.Infinity;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (Time.time > _lastDamage + _damageCooldown)
                ApplyDamage(other);
        }
    }

    private void ApplyDamage(Collider2D other)
    {
        PlayerLifeController playerLife = other.GetComponent<PlayerLifeController>();
        if (playerLife != null)
        {
            playerLife.TakeDamage(_damage);
            _lastDamage = Time.time;
        }
    }
}
