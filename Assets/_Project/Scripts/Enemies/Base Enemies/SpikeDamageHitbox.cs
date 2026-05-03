using UnityEngine;

public class SpikeDamageHitbox : MonoBehaviour
{
    [SerializeField] private int _damage = 15;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerLifeController playerLife = collision.gameObject.GetComponent<PlayerLifeController>();
        if (playerLife == null)
            return;

        playerLife.TakeDamage(_damage);
    }
}