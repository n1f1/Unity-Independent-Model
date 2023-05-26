namespace Model.Characters.CharacterHealth
{
    public interface IHealthView
    {
        void Display(float normalizedHealth);
        float NormalizedHealth { get; }
    }
}