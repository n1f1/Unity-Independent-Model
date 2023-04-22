namespace Server.Simulation.CommandSend
{
    internal interface ICommandSender<in TCommand>
    {
        void Send(TCommand command);
    }
}