using System;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

namespace Utility.GameLoop
{
    public class UnityUpdateReplace : IDisposable
    {
        private readonly PlayerLoopSystem _defaultSystems;

        public UnityUpdateReplace()
        {
            _defaultSystems = PlayerLoop.GetDefaultPlayerLoop();
        }

        public void Replace(PlayerLoopSystem.UpdateFunction update)
        {
            PlayerLoopSystem customUpdate = new PlayerLoopSystem()
            {
                type = typeof(ReplaceUpdate),
                updateDelegate = update
            };

            PlayerLoopSystem replace = PlayerLoop.GetDefaultPlayerLoop();
            GameLoopUtility.ReplaceSystem<Update.ScriptRunBehaviourUpdate>(ref replace, customUpdate);
            PlayerLoop.SetPlayerLoop(replace);
        }

        public void Dispose()
        {
            PlayerLoop.SetPlayerLoop(_defaultSystems);
        }
    }
}