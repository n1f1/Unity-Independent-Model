using Model.Characters.CharacterHealth;

namespace Tests.Model.Characters.CharacterHealth.Support
{
    internal class NullHealthVew : IHealthView
    {
        public void Display(float normalizedHealth)
        {
            
        }

        public float NormalizedHealth { get; }
    }
}