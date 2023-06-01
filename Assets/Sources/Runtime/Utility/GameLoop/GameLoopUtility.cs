using UnityEngine.LowLevel;

namespace Utility.GameLoop
{
    public static class GameLoopUtility
    {
        public static bool ReplaceSystem<T>(ref PlayerLoopSystem system, PlayerLoopSystem replacement)
        {
            if (system.type == typeof(T))
            {
                system = replacement;
                return true;
            }

            if (system.subSystemList == null)
                return false;

            for (var i = 0; i < system.subSystemList.Length; i++)
            {
                if (ReplaceSystem<T>(ref system.subSystemList[i], replacement))
                    return true;
            }

            return false;
        }

        public static PlayerLoopSystem FindSubSystem<T>(PlayerLoopSystem def)
        {
            if (def.type == typeof(T))
                return def;

            if (def.subSystemList == null)
                return default;

            foreach (PlayerLoopSystem s in def.subSystemList)
            {
                PlayerLoopSystem system = FindSubSystem<T>(s);

                if (system.type == typeof(T))
                    return system;
            }

            return default;
        }
    }
}