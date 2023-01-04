using System;
using System.Numerics;

namespace Model
{
    public class Player
    {
        private readonly CharacterMovement _characterMovement;
        private readonly Transform _transform;
        private readonly IDamageable _health;
        private readonly CharacterShooter _shooter;

        public Player(IPositionView positionView, IHealthView healthView, ForwardAim forwardAim,
            IBulletFactory<DefaultBullet> bulletFactory)
        {
            _health = new Health(healthView);
            _transform = new Transform(positionView);
            _shooter = new CharacterShooter(new DefaultGun(forwardAim, bulletFactory), _transform);
            _characterMovement = new CharacterMovement(_transform, 5f);
        }

        public CharacterMovement CharacterMovement => _characterMovement;
        public Transform Transform => _transform;
        public IDamageable Health => _health;
        public CharacterShooter CharacterShooter => _shooter;
    }

    public interface IAimView : IView
    {
        void Aim(Vector3 position, Vector3 aimPosition);
    }

    public class DefaultBullet : IBullet
    {
        private readonly Transform _transform;
        private readonly ITrajectory _trajectory;
        private readonly float _speed;
        private readonly float _distance;

        private float _passedTime;

        public DefaultBullet(Transform transform, ITrajectory trajectory, float speed)
        {
            _speed = speed;
            _trajectory = trajectory;
            _transform = transform;
            _distance = trajectory.Distance;
        }

        public void AddPassedTime(float deltaTime)
        {
            _passedTime += deltaTime;
            _transform.SetPosition(_trajectory.Evaluate(Math.Clamp(_passedTime * _speed / _distance, 0, 1)));
        }
    }

    public interface IBullet
    {
        void AddPassedTime(float deltaTime);
    }

    public class ForwardTrajectory : ITrajectory
    {
        private Vector3 _start;
        private Vector3 _finish;

        public ForwardTrajectory(Vector3 start, Vector3 finish)
        {
            _start = start;
            _finish = finish;
        }

        public float Distance => Vector3.Distance(_start, _finish);

        public Vector3 Evaluate(float ratio)
        {
            return Vector3.Lerp(_start, _finish, ratio);
        }
    }

    public interface ITrajectory
    {
        Vector3 Evaluate(float ratio);
        float Distance { get; }
    }

    public class ForwardAim : IAim
    {
        private readonly IAimView _aimView;

        private Vector3 _position;
        private Vector3 _aimPosition;
        private float _aimHeight = 1f;

        public ForwardAim(IAimView aimView)
        {
            _aimView = aimView;
        }

        public void Aim(Vector3 position, Vector3 aimPosition)
        {
            aimPosition.Y = _aimHeight;
            position.Y = _aimHeight;
            _aimPosition = aimPosition;
            _position = position;
            _aimView.Aim(_position, _aimPosition);
        }

        public ITrajectory Trajectory =>
            new ForwardTrajectory(_position, _position + Vector3.Normalize(_aimPosition - _position) * 50f);
    }

    public class DefaultGun : IWeapon
    {
        private IBulletFactory<IBullet> _bulletFactory;

        public DefaultGun(IAim aim, IBulletFactory<IBullet> bulletFactory)
        {
            _bulletFactory = bulletFactory;
            Aim = aim;
        }

        public IAim Aim { get; }

        public void Shoot()
        {
            IBullet bullet = _bulletFactory.CreateBullet(Aim.Trajectory, 25f);
        }
    }

    public interface IBulletFactory<out TBullet> where TBullet : IBullet
    {
        TBullet CreateBullet(ITrajectory trajectory, float speed);
    }

    public class CharacterShooter
    {
        private readonly IWeapon _weapon;
        private readonly Transform _character;

        public CharacterShooter(IWeapon weapon, Transform character)
        {
            _weapon = weapon;
            _character = character;
        }

        public void Aim(Vector3 aimPosition)
        {
            _weapon.Aim.Aim(_character.Position, aimPosition);
        }

        public void Shoot()
        {
            _weapon.Shoot();
        }
    }

    public interface IWeapon
    {
        public IAim Aim { get; }
        public void Shoot();
    }

    public interface IAim
    {
        void Aim(Vector3 aimPosition, Vector3 from);
        ITrajectory Trajectory { get; }
    }
}