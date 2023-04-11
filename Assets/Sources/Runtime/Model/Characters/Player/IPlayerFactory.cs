using System.Numerics;

namespace Model.Characters.Player
{
    public interface IPlayerFactory
    {
        Player CreatePlayer(Vector3 position);
    }
}