using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LeftRightGame
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private List<DanceCommand> fixedDance = new List<DanceCommand>();
        [SerializeField] private List<float> fixedDanceDelay = new List<float>();

        private GameInput gameInput;
        private GameTimer gameTimer;
        private GameSound gameSound;

        private DanceCommand? currentDanceCmd;
        private int score;
        private bool danceMatched;
        private bool madeFirstMove;

        public Action<DanceCommand, bool> OnDanceCommandUpdated;
        public Action<int> OnScoreUpdated;
        public Action<bool> OnDanceCommandFeedback;
        public int Score => score;

        private void Awake()
        {
            gameInput = FindAnyObjectByType<GameInput>();
            gameInput.OnDanceCommandInvoked += HandleDanceCommandStarted;
            gameInput.RestartRequested += TryRestartGame;

            gameTimer = FindAnyObjectByType<GameTimer>();

            gameSound = FindAnyObjectByType<GameSound>();
            gameSound.OnFixedPopDance += ForcePopDanceCommand;
        }

        private void Start()
        {
            RestartGame();
        }

        private void FixedUpdate()
        {
            if (madeFirstMove && fixedDanceDelay.Count > 0 && fixedDanceDelay[0] > 0)
            {
                fixedDanceDelay[0] -= Time.fixedDeltaTime;
            }

            if (CanLoadNextDance())
            {
                danceMatched = false;
                if (fixedDance.Count > 0)
                {
                    LoadNextDanceFromFixedList();
                }
                else
                {
                    currentDanceCmd = GetRandomDanceCmd();
                    // gameSound.PlayCommand(currentDanceCmd.Value);
                    gameTimer.isTimerRunning = true;
                }
                OnDanceCommandUpdated?.Invoke(currentDanceCmd.Value, false);
            }
        }
        private void RestartGame()
        {
            score = 0;
            madeFirstMove = false;
            gameSound.StopBgm();

            LoadNextDanceFromFixedList();
            OnScoreUpdated?.Invoke(score);
            OnDanceCommandUpdated?.Invoke(currentDanceCmd.Value, true);
        }

        private void LoadNextDanceFromFixedList()
        {
            currentDanceCmd = fixedDance[0];
            fixedDance.RemoveAt(0);
            fixedDanceDelay.RemoveAt(0);
        }

        private DanceCommand GetRandomDanceCmd()
        {
            return UnityEngine.Random.value < 0.5f
                 ? DanceCommand.Left
                 : DanceCommand.Right;
        }

        private void HandleDanceCommandStarted(DanceCommand command)
        {
            if (gameTimer.IsGameOver || !currentDanceCmd.HasValue || danceMatched)
            {
                return;
            }

            danceMatched = command == currentDanceCmd.Value;
            OnDanceCommandFeedback?.Invoke(danceMatched);

            if (danceMatched)
            {
                if (!madeFirstMove)
                {
                    madeFirstMove = true;
                    gameSound.StartBgm();
                }

                currentDanceCmd = null;
                score++;

                gameSound.PlayWhistle();
                OnScoreUpdated?.Invoke(score);
            }
        }

        private bool CanLoadNextDance()
        {
            if (fixedDance.Count <= 0)
            {
                return !currentDanceCmd.HasValue;
            }
            else
            {
                return madeFirstMove && fixedDanceDelay[0] <= 0;
            }
        }

        public void TryRestartGame()
        {
            if (gameTimer.IsGameOver)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        private void ForcePopDanceCommand()
        {
            danceMatched = false;
            LoadNextDanceFromFixedList();
            OnDanceCommandUpdated?.Invoke(currentDanceCmd.Value, false);
        }
    }
}