using System.Collections.Generic;
using Model.Characters.Shooting.Bullets;

public class BulletsContainer
{
    private readonly LinkedList<IBullet> _bullets = new();
    private readonly IBulletDestroyer _bulletDestroyer;

    public BulletsContainer(IBulletDestroyer bulletDestroyer)
    {
        _bulletDestroyer = bulletDestroyer;
    }

    public void Add(IBullet bullet)
    {
        _bullets.AddLast(bullet);
    }

    public void Update(float deltaTime)
    {
        Stack<IBullet> collidedBullets = new Stack<IBullet>(16);
        
        for (LinkedListNode<IBullet> node = _bullets.First; node != null; node = node.Next)
        {
            IBullet bullet = node.Value;
                
            bullet.UpdatePassedTime(deltaTime);

            if (bullet.Collided)
            {
                collidedBullets.Push(bullet);
                _bullets.Remove(node);
            }
        }

        while (collidedBullets.Count > 0) 
            _bulletDestroyer.Destroy(collidedBullets.Pop());
    }
}