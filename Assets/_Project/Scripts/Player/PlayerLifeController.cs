using System.Collections;
using UnityEngine;

public class PlayerLifeController : LifeController
{
    [SerializeField] private AnimationManager _animator;

    void Awake()
    {
        if (_animator == null) _animator = GetComponent<AnimationManager>();
    }

    void Start()
    {
        if (UI_Lifebar.Instance != null)
            _onLifeChanged.AddListener(UI_Lifebar.Instance.UpdateLifeGraphics);

        if (GameManager.Instance.SavedPlayerHP != -1)
            SetHP(GameManager.Instance.SavedPlayerHP);

        StartCoroutine(InitLife());
    }

    void OnEnable()
    {
        OnDamage += HandleDamage;
        OnHeal += HandleHeal;
        OnDeath += HandleDeath;
    }

    void OnDisable()
    {
        OnDamage -= HandleDamage;
        OnHeal -= HandleHeal;
        OnDeath -= HandleDeath;
    }

    void OnDestroy()
    {
        GameManager.Instance.SavePlayerHP(HP);
    }

    public void HandleDamage()
    {
        _animator.Damage();
        SoundManager.Instance.PlaySFXSound("Damage");
    }

    public void HandleHeal()
    {
        SoundManager.Instance.PlaySFXSound("Heal");
    }

    public void HandleDeath()
    {
        _animator.Dead();
        SoundManager.Instance.PlaySFXSound("Death");
        Destroy(gameObject, 0.5f);
        GameManager.Instance.GameOver();
    }

    private IEnumerator InitLife()
    {
        yield return null;
        _onLifeChanged.Invoke(_currentHP, _maxHP);
    }
}
