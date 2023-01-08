namespace Model.Physics
{
    public interface PhysicsInteraction<in TInteractionType>
    {
        void Invoke(TInteractionType collision);
    }
}