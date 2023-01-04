namespace Model
{
    public interface IViewSimulation
    {
        T AddView<T>() where T : IView;
    }
}