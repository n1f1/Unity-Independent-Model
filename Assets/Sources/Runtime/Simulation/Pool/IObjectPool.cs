namespace Simulation.Pool
{
    public interface IObjectPool<TPoolable> where TPoolable : IPoolable
    {
        int Capacity { get; }
        bool CanGet();
        void Return(TPoolable poolable);
        TPoolable GetFree();
        void AddNew(TPoolable poolable);
    }
}