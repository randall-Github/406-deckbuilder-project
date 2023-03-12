using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkStateListener : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ChangeDialogueBasedOnState();
    }

    private void ChangeDialogueBasedOnState()
    {
        
        GameState.NPCs.Mark.encountersCompleted.OnChange += OnEncounterComplete;
    }

    private void OnEncounterComplete()
    {
        //if you've completed the first encounter, then we want to initiate the next dialogue tree depending on whether you won or lost
        
        if (GameState.NPCs.Mark.encountersWon.Value == 1)
        {
            transform.GetComponent<NPC>().CurrentDialogueKey = "AfterEncounterWin";
        }
        else
        {
            transform.GetComponent<NPC>().CurrentDialogueKey = "AfterEncounterLoss";
        }

        transform.GetComponent<NPCDialogueTrigger>().StartDialogue();

        if (GameState.NPCs.Mark.encountersCompleted.Value == 0)
        {
            transform.GetComponent<NPC>().CurrentDialogueKey = "Intro";
        }

        
    }

    

   

}