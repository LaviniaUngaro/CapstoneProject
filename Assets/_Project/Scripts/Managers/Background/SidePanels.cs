using UnityEngine;

public class SidePanels : MonoBehaviour
{
    [SerializeField] private float _speedPanel;
    [SerializeField] private Renderer _panelsRenderer;

    void Awake()
    {
        if (_panelsRenderer == null) _panelsRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        _panelsRenderer.material.mainTextureOffset += new Vector2(0, _speedPanel * Time.deltaTime);
    }

    public void ShowPanel()
    {
        gameObject.SetActive(true);
    }
}
