using UnityEngine;
using System.Collections;

/*
* The following code was taken directly from the online Youtube tutorial by user gamesplusjames.
* part 1: https://www.youtube.com/watch?v=ehmBIP5sjOM
* part 2: https://www.youtube.com/watch?v=7KNQYPcx-uU
* part 3: https://www.youtube.com/watch?v=vdSxOttY3zg
*
* Attach this script to any in game object that triggers dialog. 
* This script can be attached many times
*/
public class ActivateTextAtLine : MonoBehaviour {

    public TextAsset theText;               // The text file to read from.

    public int startLine;                   // the line to start reading.
    public int endLine;                     // the line to end reading.

    public TextBoxManager theTextManager;   // The TextBoxManager object.

    public bool destroyWhenActivated;       // Ensures that the same dialog isn't read many times.
    public bool requiredButtonPress;        // Used to activate dialog box only when user presses a button.
    private bool waitForPress;              // partner variable used with requiredButtonPress.

	// Use this for initialization
	void Start () {
        theTextManager = FindObjectOfType<TextBoxManager>();
	}
	
	// Update is called once per frame
	void Update () {
        // only activate the text box here if the box needed to be activated by button press
	    if (waitForPress && Input.GetKeyDown(KeyCode.Space))
        {
            theTextManager.ReloadScript(theText);
            theTextManager.currentLine = startLine;
            theTextManager.endAtLine = endLine;
            theTextManager.EnableTextBox();

            // if the dialog should not be repeated, delete the containing object from the scene.
            if (destroyWhenActivated)
            {
                Destroy(gameObject);
            }
        }
	}

    // used for activating dialog boxes when the player object moves within a certain area.
    // must have a box collider.
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger entered");
        if(other.name == "Player")
        {
            // if the script requires a button press, skip this function and set waitForPress instead.
            if (requiredButtonPress)
            {
                waitForPress = true;
                return;
            }

            // run this code if dialog needed to be activated by proximity rather than button press.
            theTextManager.ReloadScript(theText);
            theTextManager.currentLine = startLine;
            theTextManager.endAtLine = endLine;
            theTextManager.EnableTextBox();

            // if the dialog should not be repeated, delete the containing object from the scene.
            if (destroyWhenActivated)
            {
                Destroy(gameObject);
            }
        }
    }

    // when the player leaves the dialog box collider reset the waitForPress variable.
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            waitForPress = false;
        }
    }
}
