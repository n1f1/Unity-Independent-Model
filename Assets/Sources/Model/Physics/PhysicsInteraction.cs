public interface PhysicsInteraction<in TType>
{
    void Invoke(TType collision);
}