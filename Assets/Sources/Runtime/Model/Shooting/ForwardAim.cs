using System.Numerics;
using Model.Shooting.Trajectory;

namespace Model.Shooting
{
    public class ForwardAim : IAim
    {
        private readonly IAimView _aimView;

        private Vector3 _position;
        private Vector3 _aimPosition;
        private float _aimHeight = 1f;
        private float _trajectoryDistance = 50f;

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
            Aiming = true;
        }

        public ITrajectory Trajectory =>
            GetTrajectory();

        public bool Aiming { get; private set; }

        public void Stop()
        {
            _aimView.Stop();
            Aiming = false;
        }

        private ForwardTrajectory GetTrajectory()
        {
            Vector3 direction = Vector3.Normalize(_aimPosition - _position);

            return new ForwardTrajectory(_position, _position + direction * _trajectoryDistance);
        }
    }
}