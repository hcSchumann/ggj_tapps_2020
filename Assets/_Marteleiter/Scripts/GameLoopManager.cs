using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public sealed class GameLoopManager : MonoBehaviour
{
    [Header("Target Objects")]
    [SerializeField] GameObject targetObjectSlot;
    [SerializeField] GameObject[] targetObjects;

    [Header("Managed Objects")]
    [SerializeField] RotationPlane rotationPlane;

    [Header("Timer")]
    [SerializeField] Text timerText;
    [SerializeField] private Color defaultTimerColor;
    [SerializeField] private Color hurryTimerColor;

    public int secondsToFinishTheGame = 20;
    public bool IsGameActive { get { return secondsToFinishTheGame > 0;  } }

    private static GameLoopManager instance = null;
    private static readonly object padlock = new object();
    private const int gameLoopDurationInSeconds = 20;
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

        var randomTargetIndex = Mathf.FloorToInt(Random.value * targetObjects.Length);
        currentTarget = Instantiate(targetObjects[randomTargetIndex], targetObjectSlot.transform);

        rotationPlane.SetInputStatus(IsGameActive);

        StartCoroutine(CountDownGameEnd());
    }
    private void EndGame()
    {
        //validate shape

        rotationPlane.SetInputStatus(IsGameActive);

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