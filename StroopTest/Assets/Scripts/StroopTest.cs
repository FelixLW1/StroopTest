using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StroopTest : MonoBehaviour
{
    public enum Colours
    {
        Red,
        Blue,
        Green,
        Purple
    }

    private Colours answer;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void GetRandomColour()
    {
        answer = Colours.Blue;
    }

    public void RecieveAnswer(Colours colour)
    {
        if (colour == answer)
        {

            return;
        } 
        
        // Give feedback
    }
    
}
