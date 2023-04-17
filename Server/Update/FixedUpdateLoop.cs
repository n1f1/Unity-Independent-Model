using System;

namespace Server.Update
{
    internal class FixedUpdateLoop
    {
        private readonly long _fixedTimeInTicks;
        private readonly ITimeUpdate _update;
        private DateTime _lastUpdate;

        public FixedUpdateLoop(int fixedTimeInMilliseconds, ITimeUpdate update)
        {
            if (fixedTimeInMilliseconds < 0)
                throw new ArgumentNullException(nameof(fixedTimeInMilliseconds));

            _fixedTimeInTicks = fixedTimeInMilliseconds * TimeSpan.TicksPerMillisecond;
            _update = update ?? throw new ArgumentNullException(nameof(update));
            _lastUpdate = DateTime.Now - TimeSpan.FromMilliseconds(fixedTimeInMilliseconds);
        }

        public void Update()
        {
            long timeSinceLastUpdate = (DateTime.Now - _lastUpdate).Ticks;

            if (timeSinceLastUpdate < _fixedTimeInTicks)
                return;

            _lastUpdate = DateTime.Now - TimeSpan.FromTicks(timeSinceLastUpdate - _fixedTimeInTicks);
            _update.AddPassedTime((float) _fixedTimeInTicks / TimeSpan.TicksPerSecond);
        }
    }
}