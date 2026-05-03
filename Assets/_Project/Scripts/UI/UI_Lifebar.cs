using UnityEngine;
using UnityEngine.UI;

public class UI_Lifebar : MonoBehaviour
{
    public static UI_Lifebar Instance { get; private set; }

    [SerializeField] private Image _lifebar;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void UpdateLifeGraphics(int currentHP, int maxHP)
    {
        _lifebar.fillAmount = (float)currentHP / maxHP;
    }
}
