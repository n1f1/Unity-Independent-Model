using Model.Characters.CharacterHealth;

namespace Tests.Model.Characters.CharacterHealth.Support
{
    internal class TestDeathStatus : IDeath
    {
        public bool Dead { get; private set; }

        public void Die()
        {
            Dead = true;
        }
    }
}