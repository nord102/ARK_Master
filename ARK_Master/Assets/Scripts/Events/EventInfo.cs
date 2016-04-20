using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class EventInfo : MonoBehaviour
{

    public Text txtEventName;
    public Text txtTime;
    public GameObject rewards;

    #region Difficulty Display
    public GameObject StarPlaceholder;
    public Sprite GoldStar;
    public Sprite GrayStar;
    #endregion

    private Events MyEvent;
    private float timer = 0.0f;
    private float startTime = 0.0f;

    public void StartEventInfo(Events myEvent)
    {
        gameObject.SetActive(true);
        MyEvent = myEvent;

        txtEventName.text = MyEvent.eventName;
        SetRewards();

        //Set Times
        startTime = Time.deltaTime;
        timer = 0.0f;

        //Set the Difficulty Stars
        SetDifficulty();
    }
    #region Loading Sprite and Texture
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
        byte[] FileData = new byte[5];

        if (File.Exists(FilePath))
        {
            FileData = File.ReadAllBytes(FilePath);
            Tex2D = new Texture2D(2, 2);           // Create new "empty" texture
            if (Tex2D.LoadImage(FileData))           // Load the imagedata into the texture (size is set automatically)
                return Tex2D;                 // If data = readable -> return texture
        }
        return null;                     // Return null if load failed
    }
    #endregion

    //Assigning how many stars to display based on the event difficulty
    public void SetDifficulty()
    {
        int difficultyCount = MyEvent.eventDifficulty;

        foreach (Transform child in StarPlaceholder.transform)
        {
            if (difficultyCount > 0)
            {
                child.GetComponent<Image>().sprite = GoldStar;
            }
            else
            {
                child.GetComponent<Image>().sprite = GrayStar;
            }

            difficultyCount--;            
        }
    }

    public void SetRewards()
    {
        //this is so bad
        List<GameObject> rewardGameObjectList = new List<GameObject>();

        foreach (Transform child in rewards.transform)
        {
            rewardGameObjectList.Add(child.gameObject);
        }

        int i = 0;
        foreach (Rewards r in MyEvent.SuccessRewards)
        {
            
            GameObject timer = rewardGameObjectList[i].transform.Find("Timer").gameObject;
            GameObject rewardImage = rewardGameObjectList[i].transform.Find("RewardImage").gameObject;

            timer.GetComponent<Text>().text = r.RewardTimer.ToString() + "s";
            //set image
            Sprite RewardImage = LoadNewSprite(r.RewardImagePath);
            rewardImage.GetComponent<Image>().sprite = RewardImage;

            ++i;
        }
    }

    public void EndEventInfo()
    {
        gameObject.SetActive(false);
        //Resolve event
        ResolveRewards();
    }

    public void ResolveRewards()
    {
        List<Rewards> rewardsWon = GetRewardsWon();
        foreach(Rewards reward in rewardsWon)
        {
            reward.ActivateReward();
        }
    }

    //Creates a list of Rewards that the Player Won from the event
    public List<Rewards> GetRewardsWon()
    {
        int secondsPassed = (int)timer - (int)startTime;
        return MyEvent.SuccessRewards.Where(x => secondsPassed <= x.RewardTimer).ToList();
    }

    //Updates the timer on the Event Info display
    public void SetTime(int Seconds)
    {
        int secs = Seconds % 60;
        int mins = Seconds / 60;
        string time = string.Format("{0:00}:{1:00}", mins, secs);
        txtTime.text = time;
    }
    
	//Ticks the timer
	void Update ()
    {
        timer += Time.deltaTime;

        SetTime((int)timer - (int)startTime);
    }
}
