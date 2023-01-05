using UnityEngine;

internal class Collidable<T> : MonoBehaviour
{
    public T CollidableObject { get; private set; }

    public void Initialize(T tObject)
    {
        CollidableObject = tObject;
    }
}