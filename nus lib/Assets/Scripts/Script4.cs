using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;




public class Script4 : MonoBehaviour
{
    [SerializeField]
    private TextAsset _InkJsonFile;
    private Story _StoryScript;

    public TMP_Text dialogueBox;
    public TMP_Text nameTag;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        portraitLeft.gameObject.SetActive(false);
        portraitRight.gameObject.SetActive(false); 
        LoadStory();
        DisplayNextLine();
    }

    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame) //Pressing space button goes to new dialogue
        {
            if (isTyping)
            {
                // skip to full line instantly
                StopCoroutine(typingCoroutine);
                dialogueBox.text = _StoryScript.currentText.Trim();
                isTyping = false;
            }
            else
            {
                DisplayNextLine();
            }
        }



    }

    void LoadStory()
    {
        _StoryScript = new Story(_InkJsonFile.text);
        _StoryScript.BindExternalFunction("Name", (string charName) => ChangeName(charName), false);
    }

    [System.Serializable]
    public struct PortraitEntry
    {
        public string tag;      // this is what you'll type in the Inspector, matches your ink tag exactly
        public Sprite sprite;   // drag the sprite here, whatever it's named
    }

    public PortraitEntry[] portraitLibrary;
    private bool storyEnded = false;
    public void DisplayNextLine()
    {
        if (_StoryScript.canContinue) 
        {
            string text = _StoryScript.Continue();
            text = text?.Trim(); 
            
            HandleTags(_StoryScript.currentTags);

            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            typingCoroutine = StartCoroutine(TypeLine(text));
        }
        else if (_StoryScript.currentChoices.Count == 0 && !storyEnded)
        {
            storyEnded = true;
            StartCoroutine(EndStory());
        }
    }

    public float typeSpeed = 0.03f; // seconds per character, tweak to taste
    private Coroutine typingCoroutine;
    private bool isTyping = false;

    void HandleTags(List<string> tags)
    {
        foreach (string tag in tags)
        {
            string[] split = tag.Split(':');
            if (split.Length != 2) continue;

            string key = split[0].Trim();
            string value = split[1].Trim();

            if (key == "portrait")
            {
                string[] parts = value.Split(',');
                if (parts.Length != 2) continue;

                string side = parts[0].Trim();
                string spriteName = parts[1].Trim();

                ChangePortrait(side, spriteName);
            }
        }
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueBox.text = "";

        foreach (char c in line)
        {
            dialogueBox.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }

        isTyping = false;
    }
    public Image portraitLeft;
    public Image portraitRight;

    public Vector2 leftPos = new Vector2(-300, 0);
    public Vector2 rightPos = new Vector2(300, 0);
    public Vector2 centerPos = new Vector2(0, 0);

    private bool leftActive = false;
    private bool rightActive = false;
    void ChangePortrait(string side, string spriteName)
    {
        if (spriteName == "none")
        {
            if (side == "left") { leftActive = false; portraitLeft.gameObject.SetActive(false); }
            else if (side == "right") { rightActive = false; portraitRight.gameObject.SetActive(false); }
            UpdateLayout();
            return;
        }

        Sprite found = null;
        foreach (var entry in portraitLibrary)
        {
            if (entry.tag == spriteName) { found = entry.sprite; break; }
        }

        if (found == null)
        {
            Debug.LogWarning($"No sprite found tagged {spriteName}");
            return;
        }

        if (side == "left")
        {
            portraitLeft.sprite = found;
            portraitLeft.gameObject.SetActive(true);
            leftActive = true;
        }
        else if (side == "right")
        {
            portraitRight.sprite = found;
            portraitRight.gameObject.SetActive(true);
            rightActive = true;
        }

        UpdateLayout();

    }

    public float moveSpeed = 0.25f; // seconds to slide into position

    private Coroutine leftMoveCoroutine;
    private Coroutine rightMoveCoroutine;

    void UpdateLayout()
    {
        if (leftActive && rightActive)
        {
            MovePortrait(portraitLeft, leftPos, true);
            MovePortrait(portraitRight, rightPos, false);
        }
        else if (leftActive && !rightActive)
        {
            MovePortrait(portraitLeft, centerPos, true);
        }
        else if (!leftActive && rightActive)
        {
            MovePortrait(portraitRight, centerPos, false);
        }
    }

    void MovePortrait(Image target, Vector2 destination, bool isLeftSlot)
    {
        // stop whichever coroutine is already animating this slot, so rapid tag changes don't fight each other
        if (isLeftSlot)
        {
            if (leftMoveCoroutine != null) StopCoroutine(leftMoveCoroutine);
            leftMoveCoroutine = StartCoroutine(SlideTo(target.rectTransform, destination));
        }
        else
        {
            if (rightMoveCoroutine != null) StopCoroutine(rightMoveCoroutine);
            rightMoveCoroutine = StartCoroutine(SlideTo(target.rectTransform, destination));
        }
    }

    IEnumerator SlideTo(RectTransform rt, Vector2 destination)
    {
        Vector2 start = rt.anchoredPosition;
        float elapsed = 0f;

        while (elapsed < moveSpeed)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / moveSpeed;
            rt.anchoredPosition = Vector2.Lerp(start, destination, t);
            yield return null;
        }

        rt.anchoredPosition = destination; // snap exactly to target so it doesn't end up slightly off
    }

    public void ChangeName(string name)
    {
        string SpeakerName = name;

        nameTag.text = SpeakerName;
    }

    IEnumerator EndStory()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(3);
    }
}
