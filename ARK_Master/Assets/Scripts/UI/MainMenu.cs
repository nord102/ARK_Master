using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public GameObject MainMenuObj;
    public GameObject AboutMenuObj;
    public GameObject OptionsMenuObj;

    public void Start()
    {
        MainMenuObj.SetActive(true);
        AboutMenuObj.SetActive(false);
        OptionsMenuObj.SetActive(false);
    }

	public void StartButton()
    {
        SceneManager.LoadScene("OpeningCinematic");        
    }

    public void ContinueButton()
    {
        // Load last played game state
       // SceneManager.LoadScene("GamePlay"); 
    }

    public void AboutButton()
    {
        MainMenuObj.SetActive(false);
        AboutMenuObj.SetActive(true);
        OptionsMenuObj.SetActive(false);   
    }

    public void OptionsButton()
    {
        MainMenuObj.SetActive(false);
        AboutMenuObj.SetActive(false);
        OptionsMenuObj.SetActive(true); 
    }

    public void BackButton()
    {
        MainMenuObj.SetActive(true);
        AboutMenuObj.SetActive(false);
        OptionsMenuObj.SetActive(false);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

}
