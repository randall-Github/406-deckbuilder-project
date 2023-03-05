
/*
 * author(s): Gabriel LePoudre, William Metivier
 * 
 * The class that acts as the model for the current encounter, and uses the PrefabController
 * 
 */

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Encounter
{
    /* Static method to launch an Encounter from wherever as long as you have a vblid config */
    public static Encounter StartEncounter(EncounterConfig config)
    {
        if (GameState.Meta.activeEncounter.Value != null)
        {
            Debug.LogError("Tried to initialize an encounter while one was active");
            return null;
        }
        else
        {
            Encounter encounter = new Encounter(config);
            GameState.Meta.activeEncounter.Value = encounter;
            return encounter;
        }
    }

    /* Static method to end an Encounter from wherever as long as there is an Encounter to close*/
    public static void EndEncounter()
    {
        if (GameState.Meta.activeEncounter.Value == null)
        {
            Debug.LogError("Tried to close an encounter when none were active");
        }
        else
        {
            //TODO
        }
    }


    // end of statics

    private GameObject _encounterPrefab;
    private EncounterPrefabController _encounterController;
    private List<IExecutableEffect> globalEffects = new();

    private List<Card> _hand = new();

    // statistics
    public StatisticsClass Statistics = new StatisticsClass();

    /* Statistics is used to hold information about the GameState and make it easier to implement conditionals */
    public class StatisticsClass
    {
        public int NumberOfPlays { get; set; } = 0;
        public int NumberOfDraws { get; set; } = 0;
        public int IntimidationCardsPlayed { get; set; } = 0;
        public int SympathyCardsPlayed { get; set; } = 0;
        public int PersuasionCardsPlayed { get; set; } = 0;
        public int PreparationCardsPlayed { get; set; } = 0;
        public int IntimidationCardsInHand { get; set; } = 0;
        public int SympathyCardsInHand { get; set; } = 0;
        public int PersuasionCardsInHand { get; set; } = 0;
        public int PreparationCardsInHand { get; set; } = 0;
    }

    /* An effect (as seen by E prefix) for an NPC weakness */
    public class EElementWeakness : Effect, IExecutableEffect
    {
        private string _element;
        public EElementWeakness(string element): base(99)
        {
            _element = element;
        }

        /* Executes the effect. A conditional may be called within */
        public void Execute(Encounter enc)
        {
            List<Card> hand = enc.GetHand();
            foreach (Card c in hand)
            {
                if (c.GetElement() == _element)
                {
                    c.StackableComplianceMod += 0.5f;
                }
            }
        }
    }

    /* An effect (as seen by E prefix) for an NPC strength */
    public class EElementResistance : Effect, IExecutableEffect
    {
        private string _element;
        public EElementResistance(string element) : base(99)
        {
            _element = element;
        }

        /* Executes the effect. A conditional may be called within */
        public void Execute(Encounter enc)
        {
            List<Card> hand = enc.GetHand();
            foreach (Card c in hand)
            {
                if (c.GetElement() == _element)
                {
                    c.StackableComplianceMod -= 0.5f;
                }
            }
        }
    }

    /* Constructor with config */
    public Encounter(EncounterConfig config)
    {
        GameObject prefabReference = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Encounter/Encounter.prefab");
        Statistics = new StatisticsClass(); // we reinitialize this, just in case

        _encounterPrefab = GameObject.Instantiate(prefabReference);

        _encounterController = _encounterPrefab.GetComponent<EncounterPrefabController>();

        _encounterController.Initialize(config);
    }

    /* Draw a card, if we can. Draws trigger "OnChange" which recalculates all card values */
    public void DrawCard()
    {
        if (_encounterController.PlaceMatFull())
        {
            Debug.Log("Can't draw, there is no place for the card");
        }
        else if (GameState.Player.dailyDeck.Value.Count == 0)
        {
            Debug.Log("Daily deck is empty!");
        }
        else
        {
            int draw_idx = Mathf.RoundToInt((Random.value * (GameState.Player.dailyDeck.Value.Count-1)));

            int draw_value = GameState.Player.dailyDeck.Value[draw_idx];
            GameState.Player.dailyDeck.Value.RemoveAt(draw_idx);
            GameState.Player.dailyDeck.Raise();  // we manually raise the change because list changes are too deep to be registered automatically

            Card draw = (Card)Cards.CreateCardWithID(draw_value);
            if (draw == null)
            {
                Debug.LogError("Drew a card with an invalid index");
            }
            else
            {
                // quick statistics
                switch (draw.GetElement())
                {
                    case "Intimidation":
                        Statistics.IntimidationCardsInHand += 1;
                        Statistics.NumberOfDraws += 1;
                        break;
                    case "Sympathy":
                        Statistics.SympathyCardsInHand += 1;
                        Statistics.NumberOfDraws += 1;
                        break;
                    case "Persuasion":
                        Statistics.PersuasionCardsInHand += 1;
                        Statistics.NumberOfDraws += 1;
                        break;
                    case "Preparation":
                        Statistics.PreparationCardsInHand += 1;
                        Statistics.NumberOfDraws += 1;
                        break;
                }

                _hand.Add(draw);
                _encounterController.PlaceCard(draw);

                OnChange(); // we call this on all draws and plays

                _encounterController.SetPatience(_encounterController.GetPatience() - 1);
            }
        }
    }

    /* Resolves global effects, as part of OnChange */
    private void ResolveGlobals()
    {
        if (globalEffects.Count == 0)
        {
            // TODO: REMOVE, this should not be hard coded
            globalEffects.Add(new EElementWeakness("Sympathy"));
            globalEffects.Add(new EElementResistance("Intimidation"));
        }
        List<IExecutableEffect> toRemove = new(); 
        foreach (IExecutableEffect e in globalEffects)
        {
            if (e.GetTerminationPlay() < Statistics.NumberOfPlays)
            {
                toRemove.Add(e);
            }
            else
            {
                e.Execute(this);
            }
        }
        foreach(IExecutableEffect e in toRemove)
        {
            globalEffects.Remove(e);
        }
    }

    /* Reset and recalculate all cards on any change (draw or play) */
    private void OnChange()
    {
        // wipe all card stuff, resolve all cards on change
        foreach (Card c in _hand)
        {
            c.StackableComplianceMod = 0;
            c.UnstackableComplianceMod = 0;
            c.StackablePatienceMod = 0;
            c.UnstackableComplianceMod = 0;
            c.OnChange();
        }
        ResolveGlobals();
    }

    /* Play a card given it's position on the board (stored internally to the card class if Initialized properly)*/
    public void PlayCard(int position)
    {
        // find card
        Card card = null;

        foreach(Card c in _hand)
        {
            if (c.GetPosition() == position)
            {
                card = c;
            }
        }
        if (card == null)
        {
            Debug.Log("Card at position " + position.ToString() + "could not be found in deck");
        }

        // quick statistics
        switch (card.GetElement())
        {
            case "Intimidation":
                Statistics.IntimidationCardsInHand -= 1;
                Statistics.NumberOfPlays += 1;
                break;
            case "Sympathy":
                Statistics.SympathyCardsInHand -= 1;
                Statistics.NumberOfPlays += 1;
                break;
            case "Persuasion":
                Statistics.PersuasionCardsInHand -= 1;
                Statistics.NumberOfPlays += 1;
                break;
            case "Preparation":
                Statistics.PreparationCardsInHand -= 1;
                Statistics.NumberOfPlays += 1;
                break;
        }

        int totalCompliance = card.GetTotalCompliance();
        int totalPatience = card.GetTotalPatience();
        card.OnPlay();

        _encounterController.SetCompliance(_encounterController.GetCompliance() + totalCompliance);
        _encounterController.SetPatience(_encounterController.GetPatience() - totalPatience);

        // remove from hand for now, we don't want to apply effects to a played card
        _hand.Remove(card);

        OnChange(); // we call this on all draws and plays

        // remove card
        _encounterController.RemoveCard(card);
    }

    /* Exposes the controller */
    public EncounterPrefabController GetEncounterController()
    {
        return _encounterController;
    }

    /* Exposes the hand (used mostly in conditionals) */
    public List<Card> GetHand()
    {
        return _hand;
    }
}