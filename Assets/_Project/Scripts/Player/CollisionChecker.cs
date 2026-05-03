using UnityEngine;

public class CollisionChecker : MonoBehaviour
{
    [SerializeField] private Vector2 _checkSize;
    [SerializeField] private LayerMask _layer;

    private bool _isColliding = true;

    public bool IsColliding => _isColliding;

    void FixedUpdate()
    {
        _isColliding = Physics2D.OverlapBox(transform.position, _checkSize, 0, _layer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, _checkSize);
    }
}