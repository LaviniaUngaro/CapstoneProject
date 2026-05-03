using UnityEngine;
using UnityEngine.UI;

public class UI_BossLifebar : MonoBehaviour
{
    [SerializeField] private Image _lifebar;

    public void UpdateLifeGraphics(int currentHP, int maxHP)
    {
        _lifebar.fillAmount = (float)currentHP / maxHP;
    }
}
