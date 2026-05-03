using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private void Start()
    {
        if (PlayerController.Instance == null)
            return;

        PlayerController.Instance.SetKinematic(true);
        PlayerController.Instance.transform.position = transform.position;
        PlayerController.Instance.SetKinematic(false);
    }
}
