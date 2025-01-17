using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DBDeckUIController : MonoBehaviour
{
    public UnityEngine.Object sceneOnComplete;

    public bool isDebug = false;

    public Text cardTypeOnHighlight;
    public Text numCardsInDeck;

    public GameObject[] deckContainers;
    public GameObject[] collectionContainers;

    public GameObject noEncounterCardPrefab;

    public GameObject plusOne;
    public GameObject minusOne;

    private List<int> _deckOnStart; 
    public Transform previewCardSpawn;
    private GameObject _previewedCard = null;
    private bool _previewedCardIsDeck;
    private int _previewedCardID;

    public int DeckPage
    {
        get
        {
            return _deckPage;
        }
        set
        {
            _deckPage = value;
            Debug.Log("_page swapped to " + _deckPage);
            DisplayDeckCards();
        }
    }
    private int _deckPage = 1;

    public int CollectionPage
    {
        get
        {
            return _collectionPage;
        }
        set
        {
            _collectionPage = value;
            Debug.Log("_page swapped to " + _collectionPage);
            DisplayCollectionCards();
        }
    }
    private int _collectionPage = 1;

    private List<Text> _deckQuantities = new();
    private List<DBDeckCardContainerController> _deckContainerControllers = new();
    private List<GameObject> _currentDeckCardInstantiations = new();

    private List<Text> _collectionQuantities = new();
    private List<DBCollectionCardContainerController> _collectionContainerControllers = new();
    private List<GameObject> _currentCollectionCardInstantiations = new();

    private int GetNumberDeckCardsOfID(int card_id)
    {
        int numberOfCardsWithId = 0;
        foreach (int card in GameState.Player.fullDeck.Value)
        {
            if (card == card_id)
            {
                numberOfCardsWithId += 1;
            }
        }
        return numberOfCardsWithId;
    }

    private List<(int, int, int)> GetDeckCards()
    {
        List<(int, int, int)> cards = new();

        for (int card_id = 1; card_id <= Cards.totalCardCount; card_id++)
        {
            int numberOfCardsWithId = 0;
            int maxNumberOfCards = 3;
            foreach(int card in GameState.Player.fullDeck.Value)
            {
                if (card == card_id)
                {
                    numberOfCardsWithId += 1;
                }
            }
            if (numberOfCardsWithId != 0)  // we found some
            {
                cards.Add((card_id, numberOfCardsWithId, maxNumberOfCards));
            }
        }

        return cards;
    }

    private List<(int, int, int)> GetCollectionCards()
    {
        List<(int, int, int)> cards = new();

        for (int card_id = 1; card_id <= Cards.totalCardCount; card_id++)
        {
            int numberOfCardsWithId = 0;
            int maxNumberOfCards = 3;
            foreach (int card in GameState.Player.collection.Value)
            {
                if (card == card_id)
                {
                    numberOfCardsWithId += 1;
                }
            }
            if (numberOfCardsWithId != 0)  // we found some
            {
                cards.Add((card_id, numberOfCardsWithId, maxNumberOfCards));
            }
        }

        return cards;
    }

    public void DeckPageUp()
    {
        if (CanMoveDeckPageUp())
        {
            Debug.Log("Went up in deck");
            DeckPage -= 1;
            GameState.Meta.pageUpTrigger.Raise();
        }
        Debug.Log("Failed to go up in deck");
    }

    public void DeckPageDown()
    {
        if (CanMoveDeckPageDown())
        {
            Debug.Log("Went down in deck");
            DeckPage += 1;
            GameState.Meta.pageDownTrigger.Raise();
        }
        Debug.Log("Failed to go down in deck");
    }

    private bool CanMoveDeckPageUp()
    {
        if (_deckPage != 1)
        {
            return true;
        }
        return false;
    }

    private bool CanMoveDeckPageDown()
    {
        Debug.Log(_currentDeckCardInstantiations.Count);
        if (_currentDeckCardInstantiations.Count != 6)
        {
            return false;
        }
        return true;
    }

    public void CollectionPageUp()
    {
        if (CanMoveCollectionPageUp())
        {
            CollectionPage -= 1;
            GameState.Meta.pageUpTrigger.Raise();
        }
    }

    public void CollectionPageDown()
    {
        if (CanMoveCollectionPageDown())
        {
            CollectionPage += 1;
            GameState.Meta.pageDownTrigger.Raise();
        }
    }

    private bool CanMoveCollectionPageUp()
    {
        if (_collectionPage != 1)
        {
            return true;
        }
        return false;
    }

    private bool CanMoveCollectionPageDown()
    {
        if (_currentCollectionCardInstantiations.Count != 6)
        {
            return false;
        }
        return true;
    }

    public void DisplayDeckCards()
    {
        try
        {
            if (_currentDeckCardInstantiations.Count != 0)
            {
                foreach (GameObject card in _currentDeckCardInstantiations)
                {
                    Destroy(card);
                }
                _currentDeckCardInstantiations.Clear();
            }

            List<(int, int, int)> ordered_cards = GetDeckCards();

            for (int card_section = -6 + (DeckPage * 6); card_section < -6 + ((DeckPage + 1) * 6) && card_section <= ordered_cards.Count - 1; card_section++)
            {

                int normalized_idx = card_section - ((DeckPage - 1) * 6);
                (int, int, int) cardData = ordered_cards[card_section];
                int cardIdx = cardData.Item1;
                (int, int) quant = (cardData.Item2, cardData.Item3);

                Card card = (Card)Cards.CreateCardWithID(cardIdx, true);
                GameObject _cardPrefabInstance = null;
                _cardPrefabInstance = Instantiate(noEncounterCardPrefab, _deckContainerControllers[normalized_idx].spawn.position, _deckContainerControllers[normalized_idx].spawn.rotation, this.gameObject.transform);

                _currentDeckCardInstantiations.Add(_cardPrefabInstance);

                NoEncounterCardPrefabController c = _cardPrefabInstance.GetComponent<NoEncounterCardPrefabController>();
                c.EnableDeckbuildingMode();
                c.onlyAddInDeckbuilding = this;
                c.onlyAddInDeckbuildingIsDeck = true;
                c.makeBiggerTransform = previewCardSpawn;
                _cardPrefabInstance.transform.localScale = new Vector3(_cardPrefabInstance.transform.localScale.x - 0.43f, _cardPrefabInstance.transform.localScale.y - 0.43f, _cardPrefabInstance.transform.localScale.z);
                card.SetAndInitializeNoEncounterFrontendController(c);
                _deckContainerControllers[normalized_idx].SetQuantity(quant);
            }
        }
        catch (MissingReferenceException e)
        {
            e.Message.Contains("e");
            GameState.Player.fullDeck.OnChange -= DisplayDeckCards;
            GameState.Player.dailyDeck.OnChange -= DisplayDeckCards;
        }
        catch (NullReferenceException e)
        {
            e.Message.Contains("e");
            GameState.Player.fullDeck.OnChange -= DisplayDeckCards;
            GameState.Player.dailyDeck.OnChange -= DisplayDeckCards;
        }
    }

    public void DisplayCollectionCards()
    {
        try
        {
            if (_currentCollectionCardInstantiations.Count != 0)
            {
                foreach (GameObject card in _currentCollectionCardInstantiations)
                {
                    Destroy(card);
                }
                _currentCollectionCardInstantiations.Clear();
            }

            List<(int, int, int)> ordered_cards = GetCollectionCards();

            for (int card_section = -6 + (CollectionPage * 6); card_section < -6 + ((CollectionPage + 1) * 6) && card_section <= ordered_cards.Count - 1; card_section++)
            {
                //Debug.Log("BBBBB2 " + (ordered_cards.Count - 1));
                int normalized_idx = card_section - ((CollectionPage - 1) * 6);

                (int, int, int) cardData = ordered_cards[card_section];
                int cardIdx = cardData.Item1;
                (int, int) quant = (cardData.Item2, cardData.Item3);

                Card card = (Card)Cards.CreateCardWithID(cardIdx, true);
                GameObject _cardPrefabInstance = null;
                _cardPrefabInstance = Instantiate(noEncounterCardPrefab, _collectionContainerControllers[normalized_idx].spawn.position, _collectionContainerControllers[normalized_idx].spawn.rotation, this.gameObject.transform);

                _currentCollectionCardInstantiations.Add(_cardPrefabInstance);

                NoEncounterCardPrefabController c = _cardPrefabInstance.GetComponent<NoEncounterCardPrefabController>();
                c.EnableDeckbuildingMode();
                c.onlyAddInDeckbuilding = this;
                c.onlyAddInDeckbuildingIsDeck = false;
                c.makeBiggerTransform = previewCardSpawn;
                _cardPrefabInstance.transform.localScale = new Vector3(_cardPrefabInstance.transform.localScale.x - 0.43f, _cardPrefabInstance.transform.localScale.y - 0.43f, _cardPrefabInstance.transform.localScale.z);
                card.SetAndInitializeNoEncounterFrontendController(c);
                _collectionContainerControllers[normalized_idx].SetQuantity(quant);
            }
        }
        catch (MissingReferenceException e)
        {
            e.Message.Contains("e");
            GameState.Player.collection.OnChange -= DisplayCollectionCards;
        }
        catch(NullReferenceException e)
        {
            e.Message.Contains("e");
            GameState.Player.collection.OnChange -= DisplayCollectionCards;
        }
    }

    public void RevertDeck()
    {
        GameState.Player.fullDeck.Value = new(_deckOnStart.ToArray());
        GameState.Player.dailyDeck.Value = new(GameState.Player.fullDeck.Value.ToArray());
        ShowProperControlsForHighlightedCard();
    }

    public void ClearDeck()
    {
        GameState.Player.fullDeck.Value = new(new int[] { });
        GameState.Player.dailyDeck.Value = new(GameState.Player.fullDeck.Value.ToArray());
        GameState.Meta.dbCardRemovedFromDeck.Raise();
        ShowProperControlsForHighlightedCard();
    }


    public void AddOneOfPreviewedCard()
    {
        GameState.Meta.dbCardMovedToDeck.Raise();
        for (int x = 0; x <= 1 -1; x++)
        {
            GameState.Player.fullDeck.Value.Add(_previewedCardID);
        }
        GameState.Player.fullDeck.Raise();
        GameState.Player.dailyDeck.Value = new(GameState.Player.fullDeck.Value.ToArray());
        ShowProperControlsForHighlightedCard();
    }

    public void AddTwoOfPreviewedCard()
    {
        for (int x = 0; x <= 2 - 1; x++)
        {
            GameState.Player.fullDeck.Value.Add(_previewedCardID);
        }
        GameState.Player.fullDeck.Raise();
        GameState.Player.dailyDeck.Value = new(GameState.Player.fullDeck.Value.ToArray());
        ShowProperControlsForHighlightedCard();
    }

    public void AddThreeOfPreviewedCard()
    {
        for (int x = 0; x <= 3 - 1; x++)
        {
            GameState.Player.fullDeck.Value.Add(_previewedCardID);
        }
        GameState.Player.fullDeck.Raise();
        GameState.Player.dailyDeck.Value = new(GameState.Player.fullDeck.Value.ToArray());
        ShowProperControlsForHighlightedCard();
    }

    public void RemoveOneOfPreviewedCard()
    {
        GameState.Meta.dbCardRemovedFromDeck.Raise();
        for (int x = 0; x <= 1 - 1; x++)
        {
            GameState.Player.fullDeck.Value.Remove(_previewedCardID);
        }
        GameState.Player.fullDeck.Raise();
        GameState.Player.dailyDeck.Value = new(GameState.Player.fullDeck.Value.ToArray());
        ShowProperControlsForHighlightedCard();
    }

    public void RemoveTwoOfPreviewedCard()
    {
        for (int x = 0; x <= 2 - 1; x++)
        {
            GameState.Player.fullDeck.Value.Remove(_previewedCardID);
        }
        GameState.Player.fullDeck.Raise();
        GameState.Player.dailyDeck.Value = new(GameState.Player.fullDeck.Value.ToArray());
        ShowProperControlsForHighlightedCard();
    }

    public void RemoveThreeOfPreviewedCard()
    {
        for (int x = 0; x <= 3 - 1; x++)
        {
            GameState.Player.fullDeck.Value.Remove(_previewedCardID);
        }
        GameState.Player.fullDeck.Raise();
        GameState.Player.dailyDeck.Value = new(GameState.Player.fullDeck.Value.ToArray());
        ShowProperControlsForHighlightedCard();
    }

    public void ShowCardInHighlight(int cardID, bool isDeck)
    {
        Card card = (Card)Cards.CreateCardWithID(cardID, true);
        GameObject _cardPrefabInstance = null;
        _cardPrefabInstance = Instantiate(noEncounterCardPrefab, previewCardSpawn.position, previewCardSpawn.rotation, this.gameObject.transform);

        NoEncounterCardPrefabController c = _cardPrefabInstance.GetComponent<NoEncounterCardPrefabController>();
        c.makeBiggerTransform = previewCardSpawn;
        c.DisableInteractions();
        _cardPrefabInstance.transform.localScale = new Vector3(_cardPrefabInstance.transform.localScale.x + 0.43f, _cardPrefabInstance.transform.localScale.y + 0.43f, _cardPrefabInstance.transform.localScale.z);
        card.SetAndInitializeNoEncounterFrontendController(c);
        this.cardTypeOnHighlight.text = "Type: " + card.GetElement();
        if (_previewedCard != null)
        {
            Destroy(_previewedCard);
        }
        _previewedCard = _cardPrefabInstance;
        _previewedCardID = cardID;
        _previewedCardIsDeck = isDeck;
        ShowProperControlsForHighlightedCard();
    }

    public void ShowProperControlsForHighlightedCard()
    {
        int num_of_card = GetNumberDeckCardsOfID(_previewedCardID);
        numCardsInDeck.gameObject.SetActive(true);
        numCardsInDeck.text = num_of_card.ToString();
        // show based on amount
        switch (num_of_card) 
        {
            case < 1:
                plusOne.SetActive(true);
                minusOne.SetActive(false);
                break;
            case < 2:
                plusOne.SetActive(true);
                minusOne.SetActive(true);
                break;
            case < 3:
                plusOne.SetActive(true);
                minusOne.SetActive(true);
                break;
            case < 4:
                minusOne.SetActive(true);
                plusOne.SetActive(false);
                break;
        }
        
        // remove based on max size
        switch(GameState.Player.maximumCardsAllowedInDeck.Value - GameState.Player.fullDeck.Value.Count)
        {
            case < 1:
                plusOne.SetActive(false);
                break;

        }
        
    }

    public void LaunchIntoScene()
    {
        if (isDebug)
        {
            this.gameObject.GetComponentInParent<Canvas>().gameObject.SetActive(false);
            return;
        }
        GameState.Meta.justSlept.Value = true;
        GameState.Meta.withinDream.Value = false;
        SceneManager.LoadScene("DaysUntil");
    }

    public void Start()
    {
        foreach(GameObject deckContainer in deckContainers)
        {
            _deckQuantities.Add(deckContainer.GetComponentInChildren<Text>());
            _deckContainerControllers.Add(deckContainer.GetComponent<DBDeckCardContainerController>());
        }
        foreach (GameObject collectionContainer in collectionContainers)
        {
            _collectionQuantities.Add(collectionContainer.GetComponentInChildren<Text>());
            _collectionContainerControllers.Add(collectionContainer.GetComponent<DBCollectionCardContainerController>());
        }
        DisplayDeckCards();
        DisplayCollectionCards();
        GameState.Player.dailyDeck.OnChange += DisplayDeckCards;
        GameState.Player.fullDeck.OnChange += DisplayDeckCards;
        GameState.Player.collection.OnChange += DisplayCollectionCards;

        _deckOnStart = new(GameState.Player.fullDeck.Value.ToArray());
        GameState.Player.dailyDeck.Value = new(GameState.Player.fullDeck.Value.ToArray());
        GameState.Meta.withinDream.Value = true;
    }
}
