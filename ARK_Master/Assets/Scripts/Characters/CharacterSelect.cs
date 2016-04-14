using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Data;

public class CharacterSelect : MonoBehaviour {

    public GameObject panel1;
    public GameObject panel2;

    public void Start()
    {
        //if ((bool)GlobalVariables.unlockedCharacters.Select("CharacterID = 2")[0]["Unlocked"])
        //{
        //    //Unlock Firefighter
        //    panel1.SetActive(false);
        //}
        //if ((bool)GlobalVariables.unlockedCharacters.Select("CharacterID = 3")[0]["Unlocked"])
        //{
        //    //Unlock Soldier
        //    panel1.SetActive(true);
        //}
    }

    public void CharacterSelected(int selectedCharacter)
    {
        GlobalVariables.selectedCharacter = selectedCharacter;
        SceneManager.LoadScene("UI");
    }
}
