using Model;
using UnityEngine;
using View;
using Transform = Model.Transform;

public class BulletFactory : IBulletFactory<DefaultBullet>
{
    private readonly IViewFactory<IPositionView> _viewFactory;
    private readonly GameObject _bulletTemplate;
    private readonly BulletsContainer _bulletsContainer;

    public BulletFactory(IViewFactory<IPositionView> viewFactory, GameObject bulletTemplate,
        BulletsContainer bulletsContainer)
    {
        _bulletsContainer = bulletsContainer;
        _bulletTemplate = bulletTemplate;
        _viewFactory = viewFactory;
    }

    public DefaultBullet CreateBullet(ITrajectory trajectory, float speed)
    {
        GameObject bullet = Object.Instantiate(_bulletTemplate);
        IPositionView positionView = _viewFactory.Create(bullet);
        DefaultBullet defaultBullet = new DefaultBullet(new Transform(positionView), trajectory, speed);
        _bulletsContainer.Add(defaultBullet);
            
        return defaultBullet;
    }
}