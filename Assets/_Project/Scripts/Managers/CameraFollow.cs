using Cinemachine;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;

    void Awake()
    {
        if (_virtualCamera == null) _virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    void Start()
    {
        _virtualCamera.Follow = PlayerController.Instance.transform;
    }
}
