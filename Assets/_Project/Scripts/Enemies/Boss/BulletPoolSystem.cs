using UnityEngine;

public class BulletPoolSystem : PoolSystem<Bullet>
{
    protected override Bullet CreateObject()
    {
        Bullet bullet = Instantiate(_prefab, transform);
        bullet.SetPool(_pool);
        return bullet;
    }

    public void SpawnBullet(Vector3 position, Vector2 direction, float speed, int damage)
    {
        Bullet bullet = _pool.Get();
        bullet.transform.position = position;
        bullet.transform.rotation = Quaternion.identity;
        
        bullet.Setup(direction, speed, damage);
    } 
}
