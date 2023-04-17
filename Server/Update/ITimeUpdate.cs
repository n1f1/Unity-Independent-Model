namespace Server.Update
{
    internal interface ITimeUpdate
    {
        void AddPassedTime(float fixedTimeInMilliseconds);
    }
}