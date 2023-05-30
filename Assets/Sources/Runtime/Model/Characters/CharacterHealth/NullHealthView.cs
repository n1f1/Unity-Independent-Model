using System;

namespace Model.Characters.CharacterHealth
{
    public class NullHealthView : IHealthView
    {
        public void Display(float normalizedHealth)
        {
            Console.WriteLine("hit " + normalizedHealth);
        }

        public float NormalizedHealth => 1;
    }
}