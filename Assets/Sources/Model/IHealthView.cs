namespace Model
{
    public interface IHealthView : IView
    {
        void Display(float normalizedHealth);
    }
}