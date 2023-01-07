namespace Model
{
    public interface ISimulation<T> : IUpdatable
    {
        void Initialize(T tObject);
    }
}