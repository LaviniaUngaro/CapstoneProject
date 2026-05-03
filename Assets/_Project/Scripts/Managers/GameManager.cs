using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int SavedPlayerHP { get; private set; } = -1;

    [SerializeField] private PlayerLifeController _player;
    [SerializeField] private GameObject _gameOverUI;
    [SerializeField] private GameObject _winUI;

    private bool _isGameOver = false;
    private bool _isWin = false;

    [SerializeField] private UnityEvent _onGameOver;
    [SerializeField] private UnityEvent _onWin;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (_player == null) _player = FindAnyObjectByType<PlayerLifeController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Update()
    {
        if (_isGameOver || _gameOverUI == null || _winUI == null)
            return;

        if (_gameOverUI.activeInHierarchy || _winUI.activeInHierarchy)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void GameOver()
    {
        Invoke(nameof(DelayGameOver), 1);
    }

    public void DelayGameOver()
    {
        if (_isGameOver)
            return;

        _isGameOver = true;
        if (PlayerController.Instance != null)
            PlayerController.Instance.SetInputEnabled(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _onGameOver?.Invoke();
        SoundManager.Instance.StopBackgroundMusic();
        SoundManager.Instance.PlayBackgroundMusic("Game Over");
    }

    public void Win()
    {
        Invoke(nameof(DelayWin), 3);
    }

    public void DelayWin()
    {
        if (_isWin)
            return;

        _isWin = true;
        if (PlayerController.Instance != null)
            PlayerController.Instance.SetInputEnabled(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _onWin?.Invoke();
        SoundManager.Instance.PlayBackgroundMusic("Win");
    }

    public void MainMenu()
    {
        _isGameOver = false;
        _isWin = false;
        SavedPlayerHP = -1;
        SoundManager.Instance.StopBackgroundMusic();
        StartCoroutine(LoadScene("MainMenu"));
    }

    public void Retry()
    {
        _isGameOver = false;
        _isWin = false;
        SavedPlayerHP = -1;
        SoundManager.Instance.StopBackgroundMusic();
        StartCoroutine(LoadScene("Level 1"));
    }

    public void NextLevel(string sceneName)
    {
        StartCoroutine(LoadScene(sceneName));
    }

    private IEnumerator LoadScene(string sceneName)
    {
        yield return StartCoroutine(FadeCanvas.Instance.FadeOut());
        _gameOverUI.SetActive(false);
        _winUI.SetActive(false);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        yield return operation;
        yield return null;
        StartCoroutine(FadeCanvas.Instance.FadeIn());
    }

    public void SavePlayerHP(int hp)
    {
        SavedPlayerHP = hp;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Level 1" || scene.name == "MainMenu")
            SoundManager.Instance.PlayBackgroundMusic("Level 1");
    }
}
