using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public class EventInfo : MonoBehaviour
{
    public Text txtEventName;
    public Text txtTime;
    public GameObject Rewards;

    public GameObject StarPlaceholder;
    public Sprite GoldStar;
    public Sprite GrayStar;

    private Events MyEvent;
    private float Timer = 0.0f;
    private float StartTime = 0.0f;

    public void StartEventInfo(Events myEvent)
    {
        gameObject.SetActive(true);
        MyEvent = myEvent;

        txtEventName.text = MyEvent.eventName;
        SetRewards();
        StartTime = Time.deltaTime;
        Timer = 0.0f;
        SetDifficulty();
    }

    //http://forum.unity3d.com/threads/generating-sprites-dynamically-from-png-or-jpeg-files-in-c.343735/
    public Sprite LoadNewSprite(string FilePath, float PixelsPerUnit = 100.0f)
    {
        // Load a PNG or JPG image from disk to a Texture2D, assign this texture to a new sprite and return its reference

        Sprite NewSprite = new Sprite();
        Texture2D SpriteTexture = LoadTexture(StateMachine.instance.ImagePath + FilePath);
        NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0), PixelsPerUnit);

        return NewSprite;
    }

    //http://forum.unity3d.com/threads/generating-sprites-dynamically-from-png-or-jpeg-files-in-c.343735/
    public Texture2D LoadTexture(string FilePath)
    {

        // Load a PNG or JPG file from disk to a Texture2D
        // Returns null if load fails

        Texture2D Tex2D;
        byte[] FileData;

        if (File.Exists(FilePath))
        {
            FileData = File.ReadAllBytes(FilePath);
            Tex2D = new Texture2D(2, 2);           // Create new "empty" texture
            if (Tex2D.LoadImage(FileData))           // Load the imagedata into the texture (size is set automatically)
                return Tex2D;                 // If data = readable -> return texture
        }
        return null;                     // Return null if load failed
    }

    public void SetDifficulty()
    {
        int difficulty = MyEvent.eventDifficulty;

        int i = 5 - MyEvent.eventDifficulty;
        foreach (Transform child in StarPlaceholder.transform)
        {
            if (i <= 0)
            {
                child.GetComponent<Image>().sprite = GoldStar;
            }
            else
            {
                child.GetComponent<Image>().sprite = GrayStar;
            }

            --i;
        }
    }

    public void SetRewards()
    {
        //this is so bad
        List<GameObject> rewards = new List<GameObject>();

        foreach (Transform child in Rewards.transform)
        {
            rewards.Add(child.gameObject);
        }

        int i = 0;
        foreach (Rewards r in MyEvent.SuccessRewards)
        {
            
            GameObject timer = rewards[i].transform.Find("Timer").gameObject;
            GameObject rewardImage = rewards[i].transform.Find("RewardImage").gameObject;

            timer.GetComponent<Text>().text = r.RewardTimer.ToString() + "s";
            //set image
            Sprite RewardImage = LoadNewSprite(r.RewardImagePath);
            rewardImage.GetComponent<Image>().sprite = RewardImage;

            ++i;
        }
    }

    public void EndEventInfo()
    {
        //gameObject.SetActive(false);
        //Resolve event
        //MyEvent.
    }

    public void SetTime(int Seconds)
    {
        int secs = Seconds % 60;
        int mins = Seconds / 60;
        string time = string.Format("{0:00}:{1:00}", mins, secs);

        txtTime.text = time;
    }

    public void StartThisEvent()
    {
        Events e = new Events(1, 1, 1);
        StartEventInfo(e);
    }
	
	// Update is called once per frame
	void Update ()
    {
        //http://answers.unity3d.com/questions/64498/time-counter-up.html
        Timer += Time.deltaTime;

        SetTime((int)Timer - (int)StartTime);
    }
}
