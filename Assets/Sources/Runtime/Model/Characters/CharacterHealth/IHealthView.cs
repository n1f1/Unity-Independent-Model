namespace Model.Characters.CharacterHealth
{
    public interface IHealthView : IView
    {
        void Display(float normalizedHealth);
    }
}