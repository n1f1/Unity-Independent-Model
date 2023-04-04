namespace ObjectComposition
{
    public interface INetworkObjectSender
    {
        void Send<TType>(TType sent);
    }
}