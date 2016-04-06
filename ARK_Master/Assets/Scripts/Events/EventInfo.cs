using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

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
            //rewardImage.

            ++i;
        }
    }

    public void EndEventInfo()
    {
        gameObject.SetActive(false);
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
