namespace Model.Characters.CharacterHealth
{
    public interface IDamageable
    {
        void TakeDamage(float damage);
        bool CanTakeDamage();
    }
}