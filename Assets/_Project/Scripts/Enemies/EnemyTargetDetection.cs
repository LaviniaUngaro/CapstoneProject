using UnityEngine;

public class EnemyTargetDetection : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _rayOrigin;
    [SerializeField] private LayerMask _obstacleLayer;
    [SerializeField] private Vector2 _targetOffset = new Vector2(0f, 0.8f);
    [SerializeField] private EnemiesController _enemy;

    [Header("Vision Cone Settings")]
    [SerializeField] private float _viewAngle = 90f;
    [SerializeField] private float _sightDistance = 8f;
    // [SerializeField] private int _segments = 12;
    // [SerializeField] Color _normalColor = Color.blue;
    // [SerializeField] Color _alertColor = Color.red;

    void Awake()
    {
        if (_enemy == null) _enemy = GetComponentInParent<EnemiesController>();
    }

    void Update()
    {
        if (_target == null)
            return;
    }

    public bool CanSeeTarget()
    {
        if (_target == null)
            return false;

        Vector2 origin = _rayOrigin.position;
        Vector2 targetPosition = (Vector2)_target.position + _targetOffset;
        Vector2 toTarget = targetPosition - origin;
        float distanceToTarget = toTarget.sqrMagnitude;

        // se il player è a una distanza maggiore della capacità visiva del nemico -> non lo vede
        if (distanceToTarget > _sightDistance * _sightDistance)
            return false;

        // altrimenti
        toTarget.Normalize();

        // verifica matematica per determinare se un oggetto si trova fuori dal campo visivo di un altro oggetto
        // più l'angolo diminuisce, più il coseno aumenta
        // se il coseno è inferiore vuol dire che l'angolo è più ampio rispetto a quello limite, quindi fuori -> non lo vede
        if (Vector3.Angle(transform.right * _enemy.FacingDirection, toTarget) > _viewAngle * 0.5f)
            return false;

        // se ci sono ostacoli -> non lo vede
        RaycastHit2D hit = Physics2D.Raycast(_rayOrigin.position, toTarget, _sightDistance, _obstacleLayer);
        if (hit.collider != null)
            return false;

        return true;
    }

    // private void OnDrawGizmos()
    // {
    //     if (_rayOrigin == null)
    //         return;

    //     float startAngle = -_viewAngle / 2;

    //     Vector2 origin = _rayOrigin.position;
    //     Vector2 forward = transform.right * _enemy.FacingDirection;

    //     Gizmos.color = CanSeeTarget() ? _alertColor : _normalColor;

    //     for (int i = 0; i <= _segments; i++)
    //     {
    //         float currentAngle = startAngle + (_viewAngle / _segments) * i;
    //         Vector2 direction = Quaternion.Euler(0, 0, currentAngle) * forward;
    //         Vector2 point = origin + direction * _sightDistance;

    //         RaycastHit2D hit = Physics2D.Raycast(origin, direction, _sightDistance, _obstacleLayer);
    //         if (hit.collider != null)
    //             point = hit.point;

    //         if (i > 0)
    //             Gizmos.DrawLine(origin, point);
    //     }
    // }
}