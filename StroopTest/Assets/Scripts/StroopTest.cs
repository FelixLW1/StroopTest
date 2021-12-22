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
    private int _score;
    private int _answerCount;

    [SerializeField] private int answersInRound;
    
    [Header("UI")] 
    [SerializeField] private TextMeshProUGUI answerText;
    [SerializeField] private TextMeshProUGUI scoreText;
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
    
    [SerializeField] private Timer timer;
    
    
    // Start is called before the first frame update
    private void Start()
    {
        menuCanvas.gameObject.SetActive(true);
        countdownCanvas.gameObject.SetActive(false);
        gameCanvas.gameObject.SetActive(false);
        endCanvas.gameObject.SetActive(false);

        if (!timer) timer = GetComponent<Timer>();
    }

    // You can't call async functions from unity buttons, so this is an in-between
    public void StartGame() => StartNewGame();
    
    private async Task StartNewGame()
    {
        menuCanvas.gameObject.SetActive(false);
        countdownCanvas.gameObject.SetActive(true);
        
        // Wait for the game countdown to finish
        await timer.StartGameCountdown();
        
        countdownCanvas.gameObject.SetActive(false);

        _score = 0;
        _answerCount = 0;
        NewColour();
        
        gameCanvas.gameObject.SetActive(true);
    }

    private void EndGame()
    {
        
    }

    public void NewColour()
    {
        _answer = GetRandomColour(_colours);

        // copy the colours into a new list
        var newList = new List<Colours>(_colours);

        newList.Remove(_answer);

        answerText.text = _answer.ToString();
        
        // Give the answer a random colour that isn't the colour of the answer
        _answer = newList[Random.Range(0, newList.Count)];
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
        // If the player answers correct, add to the score
        if (colour == _answer.ToString())
        {
            _score++;
            scoreText.text = _score.ToString();
        }

        _answerCount++;

        if (_answerCount == answersInRound)
        {
            EndGame(); 
            return;
        }
        
        NewColour();
    }
}
