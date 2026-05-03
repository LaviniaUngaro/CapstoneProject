using UnityEngine;
using UnityEngine.UI;

public class UI_PersistentCanvas : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverCanvas;
    [SerializeField] private GameObject _winCanvas;

    [SerializeField] private Button _retryButton;
    [SerializeField] private Button _gameOverMainMenuButton;
    [SerializeField] private Button _winMainMenuButton;

    void Awake()
    {
        var all = FindObjectsByType<UI_PersistentCanvas>(FindObjectsSortMode.None);
        if (all.Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(_gameOverCanvas);
        DontDestroyOnLoad(_winCanvas);
    }

    void Start()
    {
        _retryButton.onClick.AddListener(() => GameManager.Instance.Retry());
        _gameOverMainMenuButton.onClick.AddListener(() => GameManager.Instance.MainMenu());
        _winMainMenuButton.onClick.AddListener(() => GameManager.Instance.MainMenu());
    }
}
