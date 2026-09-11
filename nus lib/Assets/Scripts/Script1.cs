using UnityEngine;
using UnityEngine.InputSystem;
using Ink.Runtime;
using TMPro;


public class Script1 : MonoBehaviour
{
    [SerializeField]
    private TextAsset _InkJsonFile;
    private Story _StoryScript;

    public TMP_Text dialogueBox;
    public TMP_Text nameTag;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadStory();
    }

    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame) //Pressing space button goes to new dialogue
        {
            DisplayNextLine();
        }

    }

    void LoadStory()
    {
        _StoryScript = new Story(_InkJsonFile.text);
        _StoryScript.BindExternalFunction("Name", (string charName) => ChangeName(charName));
    }

    public void DisplayNextLine()
    {
        if (_StoryScript.canContinue) 
        {
            string text = _StoryScript.Continue();
            text = text?.Trim(); 
            dialogueBox.text = text; 
        }
        else
        {
            dialogueBox.text = " ";
        }
    }

    public void ChangeName(string name)
    {
        string SpeakerName = name;

        nameTag.text = SpeakerName;
    }

}
