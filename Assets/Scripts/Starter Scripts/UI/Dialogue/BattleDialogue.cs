using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

//A script by Michael O'Connell, extended by Benjamin Cohen


public class BattleDialogue : MonoBehaviour
{
    //Attach this script to an empty gameobject with a 2D collider set to trigger
    BattleStartManager manager;
    public TextAsset TextFileAsset; // your imported text file for your NPC
    private Queue<string> dialogue = new Queue<string>(); // stores the dialogue (Great Performance!)
    public float waitTime = 0.5f; // lag time for advancing dialogue so you can actually read it
    private float nextTime = 0f; // used with waitTime to create a timer system
    public bool singleUseDialogue = false;
    [HideInInspector]
    public bool hasBeenUsed = false;
    bool inArea = false;

    public GameObject spriteObject;
    private SpriteRenderer spriteRenderer;

    public GameObject battleDialogue;
    public bool inBattle = false;
    public bool won = false;
    public bool interact;

    public GameObject BattleCamera;
    public BattleDialogBox battle;


    // public bool useCollision; // unused for now

    private void Start()
    {
        manager = FindObjectOfType<BattleStartManager>();
        spriteRenderer = spriteObject.GetComponent<SpriteRenderer>();
    }


    private void Update()
    {
        if (!hasBeenUsed && inArea && Input.GetMouseButtonDown(0) && nextTime < Time.timeSinceLevelLoad)
        {
            nextTime = Time.timeSinceLevelLoad + waitTime;
            manager.click.Play();
            if (!manager.IsDialogueActive())
            {
                TriggerDialogue();
            }
            else
            {
                manager.AdvanceDialogue(BattleCamera, battle);
            }
        }
    }

    /* Called when you want to start dialogue */
    void TriggerDialogue()
    {
        ReadTextFile(); // loads in the text file
        manager.StartDialogue(dialogue, BattleCamera, battle); // Accesses Dialogue Manager and Starts Dialogue
    }

    /* loads in your text file */
    private void ReadTextFile()
    {
        string txt = TextFileAsset.text;

        string[] lines = txt.Split(System.Environment.NewLine.ToCharArray()); // Split dialogue lines by newline

        SearchForTags(lines);

        dialogue.Enqueue("EndQueue");
    }

    /*Version 2: Introduces the ability to have multiple tags on a single line! Allows for more functions to be programmed
     * to unique text strings or general functions. 
     */
    private void SearchForTags(string[] lines)
    {
        foreach (string line in lines) // for every line of dialogue
        {
            if (!string.IsNullOrEmpty(line))// ignore empty lines of dialogue
            {
                if (line.StartsWith("[")) // e.g [NAME=Michael] Hello, my name is Michael
                {
                    string special = line.Substring(0, line.IndexOf(']') + 1); // special = [NAME=Michael]
                    string curr = line.Substring(line.IndexOf(']') + 1); // curr = Hello, ...
                    dialogue.Enqueue(special); // adds to the dialogue to be printed
                    string[] remainder = curr.Split(System.Environment.NewLine.ToCharArray());
                    SearchForTags(remainder);
                    //dialogue.Enqueue(curr);
                }

                else
                {
                    dialogue.Enqueue(line); // adds to the dialogue to be printed
                }
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && !hasBeenUsed)
        {
            manager.currentTrigger = this;
            if (Input.GetMouseButtonDown(0) || !interact)
            {
                manager.currentTrigger = this;
                TriggerDialogue();
            }
            else
            {
                spriteRenderer.enabled = true;
            }
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            inArea = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            manager.EndDialogueWalkAway();
            spriteRenderer.enabled = false;
            inArea = false;
        }
    }

}
