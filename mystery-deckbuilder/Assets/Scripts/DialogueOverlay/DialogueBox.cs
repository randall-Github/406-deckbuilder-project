/*
 *
 * author(s) Ehsan Soltan
 *
 * This script contains the DialogueBox class, responsible for visually displaying dialogue
 * 
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


/*
 * The class responsible for expressing the behavior of the DialogueBox game object
 * Contains public methods for setting the nametag, displaying a sentence, and destroying itself
*/
public class DialogueBox : MonoBehaviour
{
    private string text;

    //the speed with which it will move to the middle of the screen when instantiated
    private float speed = 25.0f;

    private bool inPosition = false;
    private bool finished = false;

    [SerializeField] private GameObject _optionBoxPrefab;
    [SerializeField] private GameObject _optionButtonPrefab;

    private GameObject _optionBox;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (!inPosition && !finished) 
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(0, -1.8f, 0), speed * Time.deltaTime);
        }
        if (!finished && transform.position == new Vector3(0, 0, 0))
        {
            inPosition = true;
        }
        if (finished)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(0, -7f, 0), speed * Time.deltaTime);
        }

    }

    /* Sets the text for the nametag portion of the dialogue box */
    public void SetName(string name)
    {
        transform.Find("Canvas").Find("Name").GetComponent<TextMeshProUGUI>().SetText(name);
    }

    /* Displays a given sentence string */
    public void DisplaySentence(string sentence)
    {
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    /* Sets finished to true, which will begin moving the box down, and will Destroy after a delay */
    public void DestroyDialogueBox() 
    {
        finished = true;
        Destroy(gameObject, 2);
    }

    /* Will be invoked as a coroutine to display the characters of the sentence one-by-one */
    IEnumerator TypeSentence (string sentence)
    {
        text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            text += letter;
            transform.Find("Canvas").Find("Message").GetComponent<TextMeshProUGUI>().SetText(text);
            yield return new WaitForSeconds(0.03f);
        }
    }

    public void SpawnOptionBox(OptionNode optionNode)
    {
        _optionBox = Instantiate(_optionBoxPrefab, new Vector3(0f, -1.5f, 0f), Quaternion.identity, transform);
        SpawnOptionButtons(optionNode);
    }

    public void DestroyOptionBox()
    {
        Destroy(_optionBox);
        foreach(Transform child in transform.Find("Canvas").transform.Find("Canvas").transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void SpawnOptionButtons(OptionNode optionNode)
    {
        int counter = 0;
        foreach(string option in optionNode.options)
        {
            
            GameObject optionButton = Instantiate(_optionButtonPrefab, new Vector3(5.3f, 0.7f - counter * 0.7f, 0f), 
            Quaternion.identity, transform.Find("Canvas").transform.Find("Canvas"));
            optionButton.GetComponentInChildren<Text>().text = option;

            int i = counter;
            optionButton.GetComponentInChildren<Button>().onClick.AddListener(() => {
                DialogueBoxManager.Instance.NextNodeByOptionIndex(i);
                DestroyOptionBox();
            });

            counter += 1;

        }
    }

}
