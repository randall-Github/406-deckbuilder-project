using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WolverineStateListener : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ChangeDialogueBasedOnState();
        UpdateDialogue();
    }

    private void ChangeDialogueBasedOnState()
    {
    
        GameState.NPCs.Wolverine.encountersCompleted.OnChange += OnEncounterComplete;
       
    }

    private void OnEncounterComplete()
    {
        //if you've completed the first encounter, then we want to initiate the next dialogue tree depending on whether you won or lost
        try {
            if (GameState.NPCs.Wolverine.encountersWon.Value == 1)
            {
                transform.GetComponent<NPC>().CurrentDialogueKey = "AfterEncounterWin";
            }
            else
            {
                transform.GetComponent<NPC>().CurrentDialogueKey = "AfterEncounterLoss";
            }

            transform.GetComponent<WolverineDialogueTrigger>().StartDialogue();

            if (GameState.NPCs.Wolverine.encountersWon.Value == 0)
            {
                transform.GetComponent<NPC>().CurrentDialogueKey = "Intro";
            }
        }
        catch (MissingReferenceException e)
        {
            e.Message.Contains("e");
            GameState.NPCs.Wolverine.encountersCompleted.OnChange -= OnEncounterComplete;
        }
        catch (NullReferenceException e)
        {
            e.Message.Contains("e");
            GameState.NPCs.Wolverine.encountersCompleted.OnChange -= OnEncounterComplete;
        }

    }

    private void UpdateDialogue()
    {
        if (GameState.NPCs.Wolverine.encountersWon.Value == 1)
        {
            transform.GetComponent<NPC>().CurrentDialogueKey = "AfterEncounterWin";
        }

        if (GameState.Player.location.Value == GameState.Player.Locations.Bar)
        {
            transform.GetComponent<NPC>().CurrentDialogueKey = "Bar";
        }

        if (GameState.Player.location.Value == GameState.Player.Locations.Boxcar)
        {
            //he doesn't hang around the boxcar after the player finishes whole event
            if (GameState.NPCs.Wolverine.encountersWon.Value == 1) {
                gameObject.SetActive(false);
            }
        }
       
    }

    

}

