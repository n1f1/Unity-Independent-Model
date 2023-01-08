namespace Model
{
    public interface ISimulation<in TSimulated> : IUpdatable
    {
        void Initialize(TSimulated tObject);
    }
}