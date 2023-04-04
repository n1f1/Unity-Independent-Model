using Model;

namespace GameMenu
{
    public interface IGame : IUpdatable
    {
        void Load();
    }
}