namespace ObjectComposition
{
    public class NullObjectSender : IObjectSender
    {
        public void Send<TType>(TType command)
        {
        
        }
    }
}