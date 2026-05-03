using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    [SerializeField] private int _heal;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            ApplyEffect(player.gameObject);
            Destroy(gameObject);
        }
    }

    private void ApplyEffect(GameObject player)
    {
        LifeController _playerLife = player.GetComponent<LifeController>();
        _playerLife.AddHp(_heal);
    }
}
