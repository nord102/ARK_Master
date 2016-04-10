using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CharacterSelect : MonoBehaviour {

    public GameObject character;

    public void CharacterSelected(int selectedCharacter)
    {
        SelectedCharacter.selectedCharacter = selectedCharacter;
        SceneManager.LoadScene("UI");
    }
}
