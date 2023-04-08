namespace GameModes.MultiPlayer
{
    public static class NetworkConstants
    {
        public const int BaseLatency = 300;
        public const int JitterDelta = 50;
        public static float RTT => 0.3f;
        public static float ServerFixedDeltaTime => 0.1f;
    }
}