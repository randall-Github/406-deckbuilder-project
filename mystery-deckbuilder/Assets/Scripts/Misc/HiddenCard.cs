using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenCard : MonoBehaviour
{
    [SerializeField] private int _card;
    [SerializeField] private GameObject _cardFoundPrefab;


    // Start is called before the first frame update
    void Start()
    {
        //to disallow player from picking it up again
        if (GameState.Player.collection.Value.Contains(_card))
        {
            gameObject.SetActive(false);
        }
    }

    public void PickUpCard()
    {
        GameState.Meta.secretFound.Raise();
        GameObject cardFound = Instantiate(_cardFoundPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity, transform.Find("Canvas").transform);
        cardFound.GetComponent<RewardDisplayController>().DisplayCardAsReward(_card);
        cardFound.GetComponent<RewardDisplayController>().SetParent(this.gameObject);
        
    }


}
