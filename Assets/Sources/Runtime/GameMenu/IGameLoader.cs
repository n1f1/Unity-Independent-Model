using System.Threading.Tasks;

namespace GameMenu
{
    public interface IGameLoader
    {
        Task Load(IGame game);
    }
}