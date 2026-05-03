using UnityEngine;
using UnityEngine.Pool;

public abstract class PoolSystem<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] protected T _prefab;
    [SerializeField] protected int _poolSize = 10;
    [SerializeField] protected int _maxPoolSize = 20;
    [SerializeField] protected bool _collectionCheck = true;

    protected IObjectPool<T> _pool;

    protected void Awake()
    {
        _pool = new ObjectPool<T>(CreateObject, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject, _collectionCheck, _poolSize, _maxPoolSize);
    }

    // per creare un item quando la pool è vuota
    protected abstract T CreateObject();

    protected void OnGetFromPool(T obj)
    {
        obj.gameObject.SetActive(true);
    }

    protected void OnReleaseToPool(T obj)
    {
        obj.gameObject.SetActive(false);
    }

    protected void OnDestroyPooledObject(T obj)
    {
        Destroy(obj.gameObject);
    }
}
