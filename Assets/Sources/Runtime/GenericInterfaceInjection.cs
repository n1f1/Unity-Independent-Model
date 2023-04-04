using UnityEngine;

public static class InspectorInterfaceInjection
{
    public static void TrySetObject<T>(ref MonoBehaviour monoBehaviour)
    {
        if (monoBehaviour == null)
            return;

        if (monoBehaviour is T)
            return;

        Debug.LogWarning(monoBehaviour.name + " needs to implement " + typeof(T));
        monoBehaviour = null;
    }
}