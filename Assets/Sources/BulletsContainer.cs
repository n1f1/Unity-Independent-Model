using System.Collections.Generic;
using Model;

public class BulletsContainer
{
    private readonly List<IBullet> _bullets = new();

    public void Add(IBullet bullet) => 
        _bullets.Add(bullet);

    public void UpdateBullets(float deltaTime)
    {
        foreach (IBullet bullet in _bullets)
            bullet.AddPassedTime(deltaTime);
    }
}