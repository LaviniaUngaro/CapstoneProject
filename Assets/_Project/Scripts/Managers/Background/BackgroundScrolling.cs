using UnityEngine;

public class BackgroundScrolling : MonoBehaviour
{
    [SerializeField] private BackgroundElement[] _backgroundElements;
    private float _scrollMultiplier = 0.01f;

    void Start()
    {
        foreach (BackgroundElement element in _backgroundElements)
        {
            element._spriteMaterial = element._backgroundSprite.material;
        }
    }

    void Update()
    {
        foreach (BackgroundElement element in _backgroundElements)
        {
            element._spriteMaterial.mainTextureOffset = new Vector2 (transform.position.x * element._scrollSpeed * _scrollMultiplier, 0);
        }
    }
}
