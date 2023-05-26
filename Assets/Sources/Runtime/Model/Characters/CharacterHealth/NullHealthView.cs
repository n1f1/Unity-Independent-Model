using System;
using Model.Characters.CharacterHealth;

namespace Server.Characters.ClientPlayer
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