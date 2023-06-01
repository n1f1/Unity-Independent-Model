namespace Model.Characters.CharacterHealth
{
    public class NullHealthView : IHealthView
    {
        public void Display(float normalizedHealth)
        {
        }

        public float NormalizedHealth => 1;
    }
}