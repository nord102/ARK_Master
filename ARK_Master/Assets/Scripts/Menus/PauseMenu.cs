/****************************************************************************
Project:		Marble Madness

Student Name:	Jordan Thompson

File Name:		PauseMenu.cs

Date:			November 6, 2015

Description:    This class defines the behaviour for the Pause Menu.
*****************************************************************************/

using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    //Audio
    public Audio audioSource;

    //Canvases
    public Canvas pauseMenu;
    public Canvas optionsMenu;
    public Canvas quitMenu;

    private bool isPaused;
    private bool inPauseMenu;

    /************************************************************************************
     * FUNCTION :		Start
     *
     * DESCRIPTION :    This function initializes the Canvas Components and disables
     *                  the Pause Menu, Options Menu, and Quit Menu.
     *************************************************************************************/
    void Start()
    {
        //Initialize the Canvas Components
        pauseMenu = pauseMenu.GetComponent<Canvas>();
        optionsMenu = optionsMenu.GetComponent<Canvas>();
        quitMenu = quitMenu.GetComponent<Canvas>();

        //Disable all Menus
        pauseMenu.enabled = false;
        optionsMenu.enabled = false;
        quitMenu.enabled = false;
    }

    /************************************************************************************
     * FUNCTION :		Update
     *
     * DESCRIPTION :    This function pauses or unpauses the game when "ESC" is pressed.
     *************************************************************************************/
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && !isPaused) //Pause
        {
            Pause();
        }
        else if (Input.GetButtonDown("Cancel") && isPaused && inPauseMenu) //Unpause
        {
            UnPause();
        }
    }

    /************************************************************************************
     * FUNCTION :		MenuOption
     *
     * DESCRIPTION :    This function resumes the game, restarts the current level, displays
     *                  the options menu, returns to the start menu, or exits the game.               
     *
     * PARAMETER :      int menuOption:     Represents a specifc menu option.                                                             
     *************************************************************************************/
    public void MenuOption(int menuOption)
    {
        if (menuOption == 1)            //Resume
        {
            UnPause();
        }        
        else if (menuOption == 2)       //Options
        {
            optionsMenu.enabled = true;
            pauseMenu.enabled = false;
            inPauseMenu = false;
        }
        else if (menuOption == 3)       //Start Menu
        {
            Time.timeScale = 1f;
            Application.LoadLevel(0);
        }
        else if (menuOption == 4)       //Exit
        {
            quitMenu.enabled = true;
            pauseMenu.enabled = false;
            inPauseMenu = false;
        }
        else if (menuOption == 5)
        {
            GlobalVariables.LoadGlobalUnlocks();
            Application.LoadLevel(1);
        }
    }

    /************************************************************************************
     * FUNCTION :		Pause
     *
     * DESCRIPTION :    This function pauses the game and enables the pause menu.
     *************************************************************************************/
    public void Pause()
    {
        //audioSource.PlaySound(0);
        pauseMenu.enabled = true;
        isPaused = true;
        inPauseMenu = true;
        Time.timeScale = 0f;

        //Disable Player Control
        StateMachine.instance.PlayerControl = false;
    }

    /************************************************************************************
     * FUNCTION :		UnPause
     *
     * DESCRIPTION :    This function unpauses the game and disables the pause menu.
     *************************************************************************************/
    public void UnPause()
    {
        //audioSource.PlaySound(1);
        pauseMenu.enabled = false;
        isPaused = false;
        inPauseMenu = false;
        Time.timeScale = 1f;

        //Enable Player Control
        StateMachine.instance.PlayerControl = true;
    }

    /************************************************************************************
     * FUNCTION :		EnablePauseMenu
     *
     * DESCRIPTION :    This function enables the Pause Menu and disables the other menus.
     *************************************************************************************/
    public void EnablePauseMenu()
    {
        optionsMenu.enabled = false;
        quitMenu.enabled = false;
        pauseMenu.enabled = true;
        inPauseMenu = true;
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
}
