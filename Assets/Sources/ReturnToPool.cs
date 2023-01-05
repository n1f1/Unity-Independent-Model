using System;
using Model;

public class ReturnToPool<TBullet> : IBulletDestroyer where TBullet : IBullet
{
    private readonly SimulatedObjectPool<TBullet> _simulatedObjectPool;

    public ReturnToPool(SimulatedObjectPool<TBullet> simulatedObjectPool)
    {
        _simulatedObjectPool = simulatedObjectPool;
    }

    public void Destroy(IBullet bullet)
    {
        if (bullet is TBullet defaultBullet)
            _simulatedObjectPool.Return(defaultBullet);
        else
            throw new ArgumentException();
    }
}