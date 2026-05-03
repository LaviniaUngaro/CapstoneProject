using UnityEngine;

public class FireballPoolSystem : PoolSystem<EnemyFireball>
{
    protected override EnemyFireball CreateObject()
    {
        EnemyFireball fireball = Instantiate(_prefab, transform);
        fireball.SetPool(_pool);
        return fireball;
    }

    public void SpawnFireball(Vector3 position, Vector2 direction, float speed, int damage)
    {
        EnemyFireball fireball = _pool.Get();
        fireball.transform.position = position;
        fireball.transform.rotation = Quaternion.identity;
        fireball.Setup(direction, speed, damage);
    } 
}
