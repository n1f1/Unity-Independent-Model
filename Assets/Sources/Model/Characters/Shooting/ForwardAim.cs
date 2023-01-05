using System.Numerics;

namespace Model.Characters.Shooting
{
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
            Aiming = true;
        }

        public ITrajectory Trajectory =>
            new ForwardTrajectory(_position, _position + Vector3.Normalize(_aimPosition - _position) * 50f);

        public bool Aiming { get; private set; }

        public void Stop()
        {
            _aimView.Stop();
            Aiming = false;
        }
    }
}