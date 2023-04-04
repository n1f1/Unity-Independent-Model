using System;
using System.Collections.Generic;
using System.Linq;
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

        public void AddAfterUpdate(PlayerLoopSystem.UpdateFunction update)
        {
            PlayerLoopSystem playerLoop = PlayerLoop.GetCurrentPlayerLoop();

            PlayerLoopSystem loop = new PlayerLoopSystem
            {
                type = typeof(ReplaceUpdate),
                updateDelegate = update
            };

            int index = Array.FindIndex(playerLoop.subSystemList, system => system.type == typeof(Update));
            PlayerLoopSystem updateLoop = playerLoop.subSystemList[index];
            List<PlayerLoopSystem> tempList = updateLoop.subSystemList.ToList();
            tempList.Add(loop);
            updateLoop.subSystemList = tempList.ToArray();
            playerLoop.subSystemList[index] = updateLoop;

            PlayerLoop.SetPlayerLoop(playerLoop);
        }

        public void Dispose()
        {
            PlayerLoop.SetPlayerLoop(_defaultSystems);
        }
    }
}