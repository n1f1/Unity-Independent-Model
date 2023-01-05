namespace Model.Physics
{
    public interface PhysicsInteraction<in TType>
    {
        void Invoke(TType collision);
    }
}