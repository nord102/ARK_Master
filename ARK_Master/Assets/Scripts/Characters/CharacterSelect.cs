using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CharacterSelect : MonoBehaviour {

    public GameObject panel1;
    public GameObject panel2;

    public void Start()
    {
        if (GlobalVariables.unlockedCharacters.Contains(1))
        {
            //Unlock Firefighter
            panel1.SetActive(false);
        }
        if (GlobalVariables.unlockedCharacters.Contains(2))
        {
            //Unlock Soldier
            panel1.SetActive(true);
        }
    }

    public void CharacterSelected(int selectedCharacter)
    {
        GlobalVariables.selectedCharacter = selectedCharacter;
        SceneManager.LoadScene("UI");
    }
}
