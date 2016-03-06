using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//using UnityEngine.SceneManagement;

public class OpeningCinematicManager : MonoBehaviour
{
    public TextBoxManager textBoxManager;

    public GameObject[] images;
    public GameObject img1;
    public GameObject img2;
    public GameObject img3;
    public GameObject img4;
    public GameObject img5;
    public GameObject img6;
    public GameObject img7;
    public GameObject img8;

    private int count = 1;

    void Start()
    {
        textBoxManager = FindObjectOfType<TextBoxManager>();

        images = new GameObject[9];
        if (img1 != null) { images[0] = img1; }
        if (img2 != null) { images[1] = img2; }
        if (img3 != null) { images[2] = img3; }
        if (img4 != null) { images[3] = img4; }
        if (img5 != null) { images[4] = img5; }
        if (img6 != null) { images[5] = img6; }
        if (img7 != null) { images[6] = img7; }
        if (img8 != null) { images[7] = img8; }

        img1.SetActive(true);
    }

    void Update()
    {
        if(textBoxManager.currentLine > textBoxManager.endAtLine && !textBoxManager.isActive)
        {
            //SceneManager.LoadScene("GamePlay");
        }
    }

    public void UpdateImages()
    {
        if (textBoxManager.currentLine == 1 || textBoxManager.currentLine == 3 || textBoxManager.currentLine == 4 ||
               textBoxManager.currentLine == 5 || textBoxManager.currentLine == 6 || textBoxManager.currentLine == 8 || textBoxManager.currentLine == 11)
        {
            if (count < 8 && images.Length > 0 && images != null && images[count] != null)
            {
                if (count > 0) { images[count - 1].SetActive(false); }
                images[count].SetActive(true);
                Debug.Log("Showing Image: " + count);
                count++;
            }
        }

        
    }

}

