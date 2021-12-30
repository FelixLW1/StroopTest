using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
// ReSharper disable once CollectionNeverQueried.Global

public class StroopTest : MonoBehaviour
{
    private enum Colours
    {
        Red,
        Blue,
        Green,
        Purple
    }

    private readonly List<Colours> _colours = new List<Colours>() {Colours.Red, Colours.Blue, Colours.Green, Colours.Purple};
    private Colours _answer;
    private Colours _lastAnswer;
    private int _score;
    private int _answerCount;
    private bool _inCountdown = false;

    [Header("Game Settings")]
    [SerializeField] private int answersInRound;  
    [SerializeField] private int secondsToAnswer;

    [Header("UI")] 
    [SerializeField] private TextMeshProUGUI answerText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI endScoreText;
    [Space] 
    [SerializeField] private Canvas menuCanvas;
    [SerializeField] private Canvas countdownCanvas;
    [SerializeField] private Canvas gameCanvas;
    [SerializeField] private Canvas endCanvas;

    [Header("Colours")] 
    [SerializeField] private Color red;
    [SerializeField] private Color blue;
    [SerializeField] private Color green;
    [SerializeField] private Color purple;

    [Space] 
    
    [Header("References")]
    [SerializeField] private Timer timer;

    private void OnEnable()
    {
        Timer.RoundTimerFinished += NewColour;
    }

    private void OnDisable()
    {
        Timer.RoundTimerFinished -= NewColour;
    }

    // Start is called before the first frame update
    private void Start()
    {
        GoToMainMenu();

        if (!timer) timer = GetComponent<Timer>();
    }

    private void Update()
    {
        // I know the new input system is better, but the old one is just too quick and easy not to use in this case, where there is only 1 input.
        if (Input.GetKeyDown(KeyCode.R) && !_inCountdown) StartGame();
    }

    // You can't call async functions from unity buttons in the inspector, so this is an in-between
    public void StartGame() => StartNewGame();
    
    private async Task StartNewGame()
    {
        timer.StopAllCoroutines();
        
        menuCanvas.gameObject.SetActive(false);
        gameCanvas.gameObject.SetActive(false);
        countdownCanvas.gameObject.SetActive(true);
        endCanvas.gameObject.SetActive(false);

        _inCountdown = true;
        
        // Wait for the game countdown to finish
        await timer.StartGameCountdown();

        _inCountdown = false;
        
        countdownCanvas.gameObject.SetActive(false);

        _score = 0;
        _answerCount = 0;
        NewColour();
        
        gameCanvas.gameObject.SetActive(true);
    }

    private void EndGame()
    {
        menuCanvas.gameObject.SetActive(false);
        countdownCanvas.gameObject.SetActive(false);
        gameCanvas.gameObject.SetActive(false);
        endCanvas.gameObject.SetActive(true);

        endScoreText.text = $"Score: {_score}";

        _score = 0;
    }
    
    private void NewColour()
    {
        timer.StartCoroutine(timer.StartRoundCountdown(secondsToAnswer));
        
        _answer = GetRandomColour(_colours);

        // copy the colours into a new list
        var newList = new List<Colours>(_colours);

        newList.Remove(_answer);
        newList.Remove(_lastAnswer);

        answerText.text = _answer.ToString();
        
        // Give the answer a random colour that isn't the colour of the answer
        _answer = newList[Random.Range(0, newList.Count)];
        _lastAnswer = _answer;
        answerText.color = GetColour(_answer);
    }
    
    private Colours GetRandomColour(ICollection list)
    {
        return _colours[Random.Range(0, list.Count)];
    }

    /// <summary>
    /// Returns the Color value of whichever Colour is passed in
    /// </summary>
    private Color GetColour(Colours colour)
    {
        return colour switch
        {
            Colours.Red => red,
            Colours.Blue => blue,
            Colours.Green => green,
            Colours.Purple => purple,
            _ => throw new ArgumentOutOfRangeException(nameof(colour), colour, null)
        };
    }

    public void RecieveAnswer(string colour)
    {
        _answerCount++;
        
        // If the player answers correct, add to the score
        if (colour == _answer.ToString())
        {
            _score += 250;
            // Add bonus score for how quickly the player guessed correctly
            _score += (int)(timer.calculatedScore * 0.05f);
        }
        // If they answered incorrectly, remove some points
        else
        {
            _score -= 100;

            if (_score < 0) _score = 0;
        } 
        
        scoreText.text = "Score: " + _score.ToString();
        
        if (_answerCount == answersInRound)
        {
            EndGame(); 
            return;
        }
        
        // Stop the last colour timer
        timer.StopAllCoroutines();
        
        NewColour();
    }

    public void GoToMainMenu()
    {
        menuCanvas.gameObject.SetActive(true);
        gameCanvas.gameObject.SetActive(false);
        countdownCanvas.gameObject.SetActive(false);
        endCanvas.gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
