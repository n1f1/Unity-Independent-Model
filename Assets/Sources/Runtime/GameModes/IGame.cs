using Model;

namespace GameModes
{
    public interface IGame : IUpdatable
    {
        void Load();
    }
}