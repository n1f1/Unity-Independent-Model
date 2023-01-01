using UnityEngine.LowLevel;

namespace Utility.GameLoop
{
    public class GameLoopUtility
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
    }
}