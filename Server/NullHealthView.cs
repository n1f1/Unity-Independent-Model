using Model.Characters.CharacterHealth;

namespace Server
{
    internal class NullHealthView : IHealthView
    {
        public void Display(float normalizedHealth)
        {
        }
    }
}