using System;
using UnityEngine;

namespace LeftRightGame
{
    public class GameTimer : MonoBehaviour
    {
        [SerializeField] float intialTimeLeft;
        public bool isTimerRunning;

        private bool isGameOver;
        public bool IsGameOver => isGameOver;

        private float timeLeft;

        public float TimeLeft => timeLeft;
        public float PercentTimeLeft => timeLeft / intialTimeLeft;

        public Action OnTimerFinished;

        private void Start()
        {
            timeLeft = intialTimeLeft;
            isGameOver = false;
            isTimerRunning = false;
        }

        private void Update()
        {
            if (isGameOver || !isTimerRunning)
                return;

            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0f)
            {
                timeLeft = 0f;
                isGameOver = true;
                OnTimerFinished?.Invoke();
            }
        }
    }
}