using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LeftRightGame
{
    public class GameInput : MonoBehaviour
    {
        [SerializeField] private InputActionReference[] inputActions;
        [SerializeField] private DanceCommand[] danceCommands;
        [SerializeField] private InputActionReference restartAction;

        public Action<DanceCommand> OnDanceCommandInvoked;
        public Action RestartRequested;

        private void Update()
        {
            for (int i = 0; i < inputActions.Length; i++)
            {
                if (inputActions[i].action.WasPressedThisDynamicUpdate())
                {
                    OnDanceCommandInvoked?.Invoke(danceCommands[i]);
                    break;
                }
            }

            if (restartAction.action.WasPressedThisDynamicUpdate())
            {
                RestartRequested?.Invoke();
            }
        }
    }
}