using UnityEngine;

public class FallingManager : MonoBehaviour
{
    [SerializeField] private PlayerLifeController _playerLife;

    void Awake()
    {
        if (_playerLife == null) _playerLife = FindObjectOfType<PlayerLifeController>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        _playerLife.TakeDamage(1000);
    }
}
