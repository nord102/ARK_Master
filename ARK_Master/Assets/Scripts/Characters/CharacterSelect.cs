using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Data;

public class CharacterSelect : MonoBehaviour {

    public GameObject panel1;
    public GameObject panel2;

    public void Start()
    {
        if ((string)GlobalVariables.unlockedCharacters.Select("CharacterID = 2")[0][3] == "1")
        {
            //Unlock Firefighter
            panel1.SetActive(false);
        }
        if ((string)GlobalVariables.unlockedCharacters.Select("CharacterID = 3")[0][3] == "1")
        {
            //Unlock Soldier
            panel2.SetActive(false);
        }
    }

    public void CharacterSelected(int selectedCharacter)
    {
        GlobalVariables.selectedCharacter = selectedCharacter;
        SceneManager.LoadScene("UI");
    }
}
