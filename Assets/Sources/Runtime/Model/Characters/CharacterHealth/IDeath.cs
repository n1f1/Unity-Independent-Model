namespace Model.Characters.CharacterHealth
{
    public interface IDeath
    {
        void Die();
        bool Dead { get; }
    }
}