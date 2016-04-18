/****************************************************************************
Project:		Marble Madness

Student Name:	Jordan Thompson

File Name:		MainMenu.cs

Date:			November 6, 2015

Description:    This class defines the behaviour for the Main Menu.
*****************************************************************************/

using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    //Canvases
    public Canvas optionsMenu;
    public Canvas aboutMenu;
    public Canvas aboutMenu_2;
    public Canvas helpMenu;
    public Canvas helpMenu_2;
    public Canvas quitMenu;

    //Buttons
    public Button startText;
    public Button optionsText;
    public Button aboutText;
    public Button helpText;
    public Button exitText;

    /************************************************************************************
     * FUNCTION :		Start
     *
     * DESCRIPTION :    This function initializes the Canvas and Text Components and enables
     *                  the Start Menu.
     *************************************************************************************/
    void Start()
    {
        //Initialize the Canvas Components
        optionsMenu = optionsMenu.GetComponent<Canvas>();
        aboutMenu = aboutMenu.GetComponent<Canvas>();
        aboutMenu_2 = aboutMenu_2.GetComponent<Canvas>();
        helpMenu = helpMenu.GetComponent<Canvas>();
        helpMenu_2 = helpMenu_2.GetComponent<Canvas>();
        quitMenu = quitMenu.GetComponent<Canvas>();

        //Initialize the Button Components
        startText = startText.GetComponent<Button>();
        optionsText = optionsText.GetComponent<Button>();
        aboutText = aboutText.GetComponent<Button>();
        helpText = helpText.GetComponent<Button>();
        exitText = exitText.GetComponent<Button>();

        //Enable the Start Menu Text Buttons
        EnableStartMenu();

        //Load Global Settings
        GlobalVariables.LoadGlobalUnlocks();
    }

    /************************************************************************************
     * FUNCTION :		MenuOption
     *
     * DESCRIPTION :    This function starts the game, displays a specified menu, or exits
     *                  the game.               
     *
     * PARAMETER :      int menuOption:     Represents a specifc menu option.                                                          
     *************************************************************************************/
    public void MenuOption(int menuOption)
    {
        if (menuOption == 1)             //Start Game
        {
            Application.LoadLevel(1);
        }
        else if (menuOption == 2)       //Options Menu
        {
            optionsMenu.enabled = true;
            DisableStartMenu();
        }
        else if (menuOption == 3)       //About Menu
        {
            aboutMenu_2.enabled = false;
            aboutMenu.enabled = true;
            DisableStartMenu();
        }
        else if (menuOption == 4)       //About_Extra Menu
        {
            aboutMenu.enabled = false;
            aboutMenu_2.enabled = true;
            DisableStartMenu();
        }
        else if (menuOption == 5)       //Help Menu
        {
            helpMenu_2.enabled = false;
            helpMenu.enabled = true;
            DisableStartMenu();
        }
        else if (menuOption == 6)       //Help_Extra Menu
        {
            helpMenu.enabled = false;
            helpMenu_2.enabled = true;
            DisableStartMenu();
        }
        else if (menuOption == 7)       //Exit Menu
        {
            quitMenu.enabled = true;
            DisableStartMenu();
        }
    }

    /************************************************************************************
     * FUNCTION :		ExitGame
     *
     * DESCRIPTION :    This function quits the application.
     *************************************************************************************/
    public void ExitGame()
    {
        Application.Quit();
    }

    /************************************************************************************
     * FUNCTION :		EnableStartMenu
     *
     * DESCRIPTION :    This function disables all non-start menus and enables all start 
     *                  menu options.
     *************************************************************************************/
    public void EnableStartMenu()
    {
        optionsMenu.enabled = false;
        aboutMenu.enabled = false;
        aboutMenu_2.enabled = false;
        helpMenu.enabled = false;
        helpMenu_2.enabled = false;
        quitMenu.enabled = false;

        startText.enabled = true;
        optionsText.enabled = true;
        aboutText.enabled = true;
        helpText.enabled = true;
        exitText.enabled = true;
    }

    /************************************************************************************
    * FUNCTION :		DisableStartMenu
    *
    * DESCRIPTION :     This function disables all start menu options.
    *************************************************************************************/
    public void DisableStartMenu()
    {
        startText.enabled = false;
        optionsText.enabled = false;
        aboutText.enabled = false;
        helpText.enabled = false;
        exitText.enabled = false;
    }
}
