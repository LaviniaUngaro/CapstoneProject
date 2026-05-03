using UnityEngine;

public class SlashPoolSystem : PoolSystem<Slash>
{
    protected override Slash CreateObject()
    {
        Slash slash = Instantiate(_prefab, transform);
        slash.SetPool(_pool);
        return slash;
    }

    public void SpawnSlash(Vector3 position, Vector2 direction, int damage, Transform parent = null)
    {
        Slash slash = _pool.Get();
        slash.gameObject.SetActive(false);

        slash.transform.position = position;
        slash.transform.rotation = Quaternion.identity;

        slash.transform.SetParent(parent);
        slash.SetPoolParent(this);

        slash.gameObject.SetActive(true);
        slash.Setup(direction, damage);

    }
}
