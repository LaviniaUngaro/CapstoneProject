using UnityEngine;

[System.Serializable]
public class BackgroundElement
{
    [SerializeField] public SpriteRenderer _backgroundSprite;
    [Range(0,1)] public float _scrollSpeed;
    public Material _spriteMaterial;
}
