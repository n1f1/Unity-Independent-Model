using System;
using Model.Characters.CharacterHealth;

namespace Server.Characters.ClientPlayer
{
    internal class NullHealthView : IHealthView
    {
        public void Display(float normalizedHealth)
        {
            Console.WriteLine("hit " + normalizedHealth);
        }
    }
}