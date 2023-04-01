namespace ObjectComposition
{
    public interface IObjectSender
    {
        void Send<TType>(TType command);
    }
}