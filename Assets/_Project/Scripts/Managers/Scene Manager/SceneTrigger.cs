using UnityEngine;

public class SceneTrigger : MonoBehaviour
{
    [SerializeField] private string _targetScene;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            GameManager.Instance.NextLevel(_targetScene);
    }
}
