using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
// ReSharper disable once CollectionNeverQueried.Local

public class StroopTest : MonoBehaviour
{
    public enum Colours
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

    [Header("Colours")] 
    [SerializeField] private Color red;
    [SerializeField] private Color blue;
    [SerializeField] private Color green;
    [SerializeField] private Color purple;
    
    
    // Start is called before the first frame update
    private void Start()
    {
        StartGame();
    }

    private void StartGame()
    {
        _score = 0;
        _answerCount = 0;
        NewColour();
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
        print($"{colour} + {_answer.ToString()}");
        
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
