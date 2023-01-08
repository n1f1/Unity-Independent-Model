namespace Model.Characters.CharacterHealth
{
    public class Death : IDeath
    {
        public bool Dead { get; private set; }

        public void Die()
        {
            Dead = true;
        }
    }
}