using System.Numerics;

namespace Model.Characters
{
    public interface IPlayerFactory
    {
        Player CreatePlayer(Vector3 position);
    }
}