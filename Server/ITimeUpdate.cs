namespace Server
{
    internal interface ITimeUpdate
    {
        void AddPassedTime(float fixedTimeInMilliseconds);
    }
}