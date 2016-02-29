using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/*
* The following code was taken directly from the online Youtube tutorial by user gamesplusjames.
* part 1: https://www.youtube.com/watch?v=ehmBIP5sjOM
* part 2: https://www.youtube.com/watch?v=7KNQYPcx-uU
* part 3: https://www.youtube.com/watch?v=vdSxOttY3zg
*
* Place this script on an empty game object and drag relevant files or assets into the script variables. 
* Must create a panel with the DialogBox sprite and a TextBox inside the panel. 
* Requires EventSystem to function.
*/
public class TextBoxManager : MonoBehaviour
{
    public GameObject textBox;          // the dialog box panel.
    public OpeningCinematicManager cinematic;

    public Text theText;                // the dialog box text box.

    public TextAsset textFile;          // the text file to read from.
    public string[] textLines;          // the lines of text read from the text file. 

    public int currentLine;             // the current line to read from.
    public int endAtLine;               // the final line to read. 

    public bool isActive;               // to make the dialog box appear immediately or stay hidden until activated. 
    public bool stopPlayerMovement;     // to freeze the player from moving when the dialog box is open.

    private bool isTyping = false;      // determines if the dialog is still scrolling.
    private bool cancelTyping = false;  // determines if the player cancels typing to skip the dialog.

    public float typeSpeed;             // how quickly the text appears in the dialog box. Measured in seconds, NOT milliseconds.

    //public PlayerController player;     // the player script (used for freezing the player object in place).

    // Use this for initialization
    void Start()
    {
        //player = FindObjectOfType<PlayerController>();
        cinematic = FindObjectOfType<OpeningCinematicManager>();

        // find the text file if there is one.
        if (textFile != null)
        {
            textLines = (textFile.text.Split('\n'));
        }

        // set the end line based on the number of lines in the file.
        // Only runs if endAtLine not set manually.
        if (endAtLine == 0)
        {
            endAtLine = textLines.Length - 1;
        }

        // determines whether to show or hide the dialog box on starting a scene.
        if (isActive)
        {
            EnableTextBox();
        }
        else
        {
            DisableTextBox();
        }
    }

    void Update()
    {
        // do not run this code if the dialog box is inactive.
        if (!isActive)
        {
            return;
        }

        // on pressing the enter key, show the text in the dialog box one line at a time. 
        // typing refers to the delay between showing individual letters. 
        // to cancel typing and skip to the end of the line, press enter a second time. 
        // press enter when the line finishes disaplying to move to the next line. 
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isTyping)
            {
                currentLine++;
                if (currentLine > endAtLine)
                {
                    DisableTextBox();
                }
                else
                {
                    // call ScrollText co-routine
                    // co-routines run separately from the main update loop.
                    StartCoroutine(TextScroll(textLines[currentLine]));
                }
            }
            else if (isTyping && !cancelTyping)
            {
                cancelTyping = true;
            }
        }
    }

    // the TextScroll co-routine. When called this function runs separately from the Update loop.
    // Does not break until it finishes running.
    // Co-routines have an IEnumerable return value.
    private IEnumerator TextScroll(string lineOfText)
    {
        int letter = 0;
        theText.text = "";
        isTyping = true;
        cancelTyping = false;

        if (cinematic != null) { cinematic.UpdateImages(); }

        while (isTyping && !cancelTyping && (letter < lineOfText.Length - 1))
        {
            theText.text += lineOfText[letter];
            letter++;
            // WaitForSeconds function can only run in a co-routine.
            // If WaitForSeconds were in a normal routine it would break the Update loop of the game. 
            yield return new WaitForSeconds(typeSpeed); 
        }

        // when finished typing or if typing is cancelled, display full line at once and reset isTyping and cancelTyping variables. 
        theText.text = lineOfText;
        isTyping = false;
        cancelTyping = false;       
    }

    // Called when Enabling the dialog box from within another script. 
    public void EnableTextBox()
    {
        textBox.SetActive(true);
        isActive = true;

        // call ScrollText co-routine
        StartCoroutine(TextScroll(textLines[currentLine]));
    }

    // Called when Disabling the dialog box from within another script. 
    public void DisableTextBox()
    {
        textBox.SetActive(false);
        isActive = false;
    }

    // Called when loading a new textfile from another script. 
    // Erases the previous lines from the string array and loads new lines. 
    public void ReloadScript(TextAsset theText)
    {
        if (theText != null)
        {
            textLines = new string[1];
            textLines = (theText.text.Split('\n'));
        }
    }
}