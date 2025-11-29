using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LeftRightGame
{
    public class GameUI : MonoBehaviour
    {
        private GameManager gameManager;
        private GameInput gameInput;
        private GameTimer gameTimer;

        [Header("UI")]
        [SerializeField] private GameObject character;
        [SerializeField] private DanceCommand characterOriginFaceSide;
        [SerializeField] private TMP_Text commandText;
        [SerializeField] private TMP_Text feedbackText;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private Slider timerBar;
        [SerializeField] private Toggle touchToggle;

        [Header("Settings")]
        [SerializeField] private Color neutralColor;
        [SerializeField] private Color matchColor;
        [SerializeField] private Color unmatchColor;
        [SerializeField] private string[] matchFeedbacks;
        [SerializeField] private string[] unmatchFeedbacks;

        [Header("UI/ResultPanel")]
        [SerializeField] private GameObject resultCanvas;
        [SerializeField] private TMP_Text resultTitleText;
        [SerializeField] private TMP_Text resultScoreText;
        [SerializeField] private TMP_Text highScoreText;
        [SerializeField] private Button buttonPlayAgain;
        [SerializeField] private Color titleDefaultColor;
        [SerializeField] private Color titleNewBestColor;

        [Header("Decor")]
        [SerializeField] private Image decorArrow;
        [SerializeField] private DanceCommand decorArrowFaceSide;
        [SerializeField] private int decorArrowMultiplier;

        private static bool isEnablingTouch;

        private void Awake()
        {
            gameManager = FindAnyObjectByType<GameManager>();
            gameManager.OnDanceCommandUpdated += HandleDanceCommandUpdated;
            gameManager.OnDanceCommandFeedback += HandleDanceCommandFeedback;
            gameManager.OnScoreUpdated += HandleScoreUpdated;

            gameInput = FindAnyObjectByType<GameInput>();
            gameInput.OnDanceCommandInvoked += HandleDanceCommandStarted;

            gameTimer = FindAnyObjectByType<GameTimer>();
            gameTimer.OnTimerFinished += ShowResultPanel;

            buttonPlayAgain.onClick.AddListener(() =>
            {
                gameManager.TryRestartGame();
            });

            touchToggle.onValueChanged.AddListener((isOn) => isEnablingTouch = isOn);

            ResetUI();
        }

        private void Update()
        {
            if (gameTimer.isTimerRunning)
            {
                timerBar.value = gameTimer.PercentTimeLeft;
            }
            else
            {
                timerBar.value = 1f;
            }
        }

        private void ResetUI()
        {
            feedbackText.SetText("");
            commandText.SetText("");
            resultCanvas.SetActive(false);
            touchToggle.isOn = isEnablingTouch;
        }

        private void HandleDanceCommandStarted(DanceCommand command)
        {
            Vector3 scale = Vector3.one;
            scale.x = command == characterOriginFaceSide
                    ? Mathf.Abs(scale.x)
                    : -Mathf.Abs(scale.x);
            character.transform.DOKill();
            character.transform.localScale = scale;
            // character.transform.DOShakeScale(0.1f, 0.5f);
            character.transform.rotation = Quaternion.identity;
            character.transform.DOShakeRotation(0.1f, 50f);

            UpdateDecorArrow(command);
        }

        private void HandleScoreUpdated(int obj)
        {
            scoreText.SetText($"Score: {obj}");
        }

        private void HandleDanceCommandFeedback(bool match)
        {
            if (match)
            {
                int index = UnityEngine.Random.Range(0, matchFeedbacks.Length);
                feedbackText.SetText(matchFeedbacks[index]);
                commandText.color = matchColor;
            }
            else
            {
                int index = UnityEngine.Random.Range(0, unmatchFeedbacks.Length);
                feedbackText.SetText(unmatchFeedbacks[index]);
                commandText.color = unmatchColor;
            }
        }

        private void HandleDanceCommandUpdated(DanceCommand command, bool isFirstMove = false)
        {
            string stringCommand = command switch
            {
                DanceCommand.Left => "<<< Left",
                DanceCommand.Right => "Right >>>",
                _ => "",
            };

            if (isFirstMove)
            {
                stringCommand += " To Start";
            }

            commandText.SetText(stringCommand);
            commandText.transform.localScale = Vector3.one;
            commandText.transform.DOKill();
            commandText.transform.DOShakeScale(0.1f, 0.5f);
            commandText.color = neutralColor;
        }

        private void ShowResultPanel()
        {
            int score = gameManager.Score;
            int lastScore = PlayerPrefs.GetInt("LeftRightGame/HighScore", 0);
            if (score > lastScore)
            {
                PlayerPrefs.SetInt("LeftRightGame/HighScore", score);
            }

            resultCanvas.SetActive(true);

            var panel = resultCanvas.transform.Find("Result Panel");
            panel.DOKill();
            panel.rotation = Quaternion.identity;
            panel.DOShakeRotation(0.5f, 10f);

            resultScoreText.SetText($"Score: {score}");
            highScoreText.SetText($"Best Score: {Mathf.Max(score, lastScore)}");
            if (lastScore > 0 && score > lastScore)
            {
                resultTitleText.SetText("New best!");
                resultTitleText.color = titleNewBestColor;
            }
            else
            {
                resultTitleText.SetText("Time's up!");
                resultTitleText.color = titleDefaultColor;
            }
        }

        private void UpdateDecorArrow(DanceCommand command)
        {
            Vector3 scale = decorArrow.transform.localScale;
            scale.x = command == decorArrowFaceSide
                    ? Mathf.Abs(scale.x)
                    : -Mathf.Abs(scale.x);

            decorArrow.pixelsPerUnitMultiplier = (gameManager.Score / decorArrowMultiplier) + 1;
            decorArrow.transform.localScale = scale;
        }
    }
}