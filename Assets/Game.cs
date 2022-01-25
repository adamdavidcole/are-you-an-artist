using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public GameObject bananaTimerBar;

    public GameObject gameOverlay;
    public GameObject WarholImage;
    public GameObject paintingCanvas;
    public GameObject speechBubble;
    public GameObject warholText;
    public GameObject easel;
    public GameObject instructions;

    public AudioSource hmmAudio;
    public AudioSource mainAudio;
    public AudioSource winGameAudio;
    public AudioSource loseGameAudio;
    public AudioSource buttonClickAudio;

    public GameObject gameStartCanvas;
    public GameObject gameSuccessCanvas;
    public GameObject gameFailureCanvas;

    private bool isGameRunning = false;
    private bool isGameOver = false;
    private float playTime = 20.0f;
    private float startTime = 0.0f;
    private RectTransform bananaTimerBarMask;
    private Vector2 initialBananaTimeMaskSizeDelta;

    

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;

        bananaTimerBarMask = bananaTimerBar.GetComponent<RectTransform>();
        initialBananaTimeMaskSizeDelta = bananaTimerBarMask.sizeDelta;

    }

    // Update is called once per frame
    void Update()
    {
        if (isGameRunning && !isGameOver)
        {
            if (Time.time > startTime + playTime)
            {
                Debug.Log("GAME OVER");

                isGameOver = true;

                bananaTimerBarMask.sizeDelta = new Vector2(initialBananaTimeMaskSizeDelta.x, 0);

                isGameRunning = false;

                paintingCanvas paintingCanvasInstance = paintingCanvas.GetComponent<paintingCanvas>();

                paintingCanvasInstance.setIsEnabled(false);
                instructions.SetActive(false);

                int spriteCount = paintingCanvasInstance.getCount();
                bool didWin = spriteCount % 2 != 0;

                Debug.Log("Did player win? " + didWin);

                string[] warholTextArray = didWin ? winGameStrings : loseGameStrings;
                int index = Random.Range(0, warholTextArray.Length);
                string nextWarholText = warholTextArray[index];

                nextWarholText = nextWarholText.Replace("@", System.Environment.NewLine);

                warholText.GetComponent<Text>().text = nextWarholText;

                gameOverlay.SetActive(true);
                WarholImage.SetActive(true);
                speechBubble.SetActive(true);
                warholText.SetActive(true);

                mainAudio.volume = 0.15f;
                hmmAudio.Play();

                float timePerChar = UiTypewriter.timePerChar;
                float durationToWait = (nextWarholText.Length + 10) * timePerChar;
                StartCoroutine(showGameOverScreenAfterTime(durationToWait, didWin));

            }
            else
            {
                float timeLeft = startTime - Time.time + playTime;
                float newMaskHeight = (timeLeft / playTime) * initialBananaTimeMaskSizeDelta.y;

                bananaTimerBarMask.sizeDelta = new Vector2(initialBananaTimeMaskSizeDelta.x, newMaskHeight);
            }
        }
    }

    public void startGame()
    {
        Debug.Log("Start game");

        gameStartCanvas.SetActive(false);
        gameSuccessCanvas.SetActive(false);
        gameFailureCanvas.SetActive(false);

        easel.SetActive(true);
        paintingCanvas.SetActive(true);
        bananaTimerBar.SetActive(true);
        instructions.SetActive(true);
        paintingCanvas.GetComponent<paintingCanvas>().setIsEnabled(true);

        isGameRunning = true;
        isGameOver = false;
        startTime = Time.time;

        buttonClickAudio.Play();
    }

    IEnumerator showGameOverScreenAfterTime(float time, bool didWin)
    {
        yield return new WaitForSeconds(time);

        // Code to execute after the delay
        Debug.Log("executed after seconds: " + time);

        WarholImage.SetActive(false);
        speechBubble.SetActive(false);
        warholText.SetActive(false);
        paintingCanvas.SetActive(false);
        bananaTimerBar.SetActive(false);
        gameOverlay.SetActive(false);
        easel.SetActive(false);
        instructions.SetActive(false);
        paintingCanvas.GetComponent<paintingCanvas>().hideAllPaintedSprites();


        if (didWin)
        {
            winGameAudio.Play();
            gameSuccessCanvas.SetActive(true);
        } else
        {
            loseGameAudio.Play();
            gameFailureCanvas.SetActive(true);
        }

        yield return new WaitForSeconds(1);
        mainAudio.volume = 0.25f;
    }

    private string[] winGameStrings = {
        "Absolutely derivative.@@@...@@@@I love it!",
        "Total trash.@@@...@@@It's perfect!",
    };
    private string[] loseGameStrings = {
        "A totally original work.@@@...@@@I hate it!",
        "This belongs in the Met@@@...@@@With the other trash. Next!",
        "You can tell this one came from the heart.@@@...@@@Yuck! Burn it.",
        "I've never seen anything like it..@@@...@@@And hopefully never will again. Pass!"
    };

}

