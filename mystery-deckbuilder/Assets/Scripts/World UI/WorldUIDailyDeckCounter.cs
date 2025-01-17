using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class WorldUIDailyDeckCounter : MonoBehaviour
{
    public int goRedThreshold = 5;

    public Text GetTextElement()
    {
        return gameObject.GetComponent<Text>();
    }

    public void DailyDeckChanged()
    {
        try
        {
            if (GameState.Player.dailyDeck.Value.Count <= goRedThreshold)
            {
                GetTextElement().color = new Color(1, 0.1f, 0.1f);
            }
            GetTextElement().text = GameState.Player.dailyDeck.Value.Count + "";
        }
        catch (MissingReferenceException e)  // oops! This script doesn't exist any more
        {
            e.Message.Contains("e");  // we use e erroniously to sidestep Unity warning
            GameState.Player.dailyDeck.OnChange -= DailyDeckChanged;  // remove it from the method list
        }
        catch (NullReferenceException e)
        {
            e.Message.Contains("e");
            GameState.Player.dailyDeck.OnChange -= DailyDeckChanged;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        DailyDeckChanged();
        GameState.Player.dailyDeck.OnChange += DailyDeckChanged;
    }

    public void Update()
    {
        if(gameObject == null)
        {
            Debug.Log("Ouch!");
        }
    }
}
