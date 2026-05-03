using UnityEngine;

public class BossTriggerZone : MonoBehaviour
{
    [SerializeField] private BossController _boss;

    void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerLifeController playerLife = collision.gameObject.GetComponent<PlayerLifeController>();

        if (playerLife == null)
            return;

        _boss.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
