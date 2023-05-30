using System.Numerics;

namespace Model.Characters.Player
{
    public readonly struct PlayerData
    {
        public PlayerData(Vector3 position, float health)
        {
            Health = health;
            Position = position;
        }

        public Vector3 Position { get; }
        public float Health { get; }
    }
}