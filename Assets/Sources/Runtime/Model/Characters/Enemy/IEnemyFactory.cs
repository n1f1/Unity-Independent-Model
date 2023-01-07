using System.Numerics;

namespace Model.Characters.Enemy
{
    public interface IEnemyFactory
    {
        Enemy Create(Vector3 position);
        void Destroy(Enemy enemy);
    }
}