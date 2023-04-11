namespace Model.Physics
{
    public interface IPhysicsInteraction<in TInteractionType>
    {
        void Invoke(TInteractionType collision);
    }
}