using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public sealed class GameLoopManager : MonoBehaviour
{
    [Header("Target Objects")]
    [SerializeField] GameObject targetObjectSlot;
    [SerializeField] LevelInfo[] levels;

    [Header("Managed Objects")]
    [SerializeField] RotationPlane rotationPlane;
    [SerializeField] LevelValidator levelValidator;
    [SerializeField] MeshRenderer levelGoalRenderer;

    [Header("Timer")]
    [SerializeField] Text timerText;
    [SerializeField] private Color defaultTimerColor;
    [SerializeField] private Color hurryTimerColor;

    [SerializeField] private int gameLoopDurationInSeconds = 20;
    private int secondsToFinishTheGame = 20;
    public bool IsGameActive { get { return secondsToFinishTheGame > 0;  } }

    private static GameLoopManager instance = null;
    private static readonly object padlock = new object();
    private GameObject currentTarget;

    public GameLoopManager()
    {
    }

    public static GameLoopManager Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new GameLoopManager();
                }
                return instance;
            }
        }
    }
    
    
    private void Start()
    {
        StartGame();
        Camera.main.transform.SetParent(rotationPlane.transform);
    }
    private void OnLoadScene()
    {
        if (secondsToFinishTheGame <= 0)
        {
            EndGame();
        }
    }

    private IEnumerator CountDownGameEnd()
    {
        for (int i =0; i < gameLoopDurationInSeconds; i++)
        {
            yield return new WaitForSeconds(1);
            secondsToFinishTheGame--;
            timerText.text = secondsToFinishTheGame.ToString();

            if(secondsToFinishTheGame < 10)
            {
                timerText.color = hurryTimerColor;
            }
        }
        EndGame();
    }
    private void StartGame()
    {
        secondsToFinishTheGame = gameLoopDurationInSeconds;
        timerText.color = defaultTimerColor;

        var randomLevelIndex = Mathf.FloorToInt(Random.value * (levels.Length-1));
        var randomLevel = levels[randomLevelIndex];
        currentTarget = Instantiate(randomLevel.InicialObject, targetObjectSlot.transform);
        levelValidator.SetTargetTexture(randomLevel.targetObjectSprite.texture);
        levelGoalRenderer.material.mainTexture = (randomLevel.targetObjectSprite.texture);
        rotationPlane.SetInputStatus(IsGameActive);

        StartCoroutine(CountDownGameEnd());
    }
    private void EndGame()
    {
        rotationPlane.SetInputStatus(IsGameActive);

        var levelRating = levelValidator.GetLevelRating();


        StartCoroutine(ResetGame());
    }
    private IEnumerator ResetGame()
    {
        yield return new WaitForSeconds(3);
        if (currentTarget != null)
        {
            Destroy(currentTarget);
        }
        StartGame();
    }
}