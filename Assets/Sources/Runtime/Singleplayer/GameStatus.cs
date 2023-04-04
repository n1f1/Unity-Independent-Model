namespace GameMenu
{
    public class GameStatus
    {
        public void Loose()
        {
            Lost = true;
            Finished = true;
        }

        public bool Lost { get; private set; }
        public bool Finished { get; private set; }
    }
}