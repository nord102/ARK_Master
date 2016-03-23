using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class EventInfo : MonoBehaviour
{
    public Text txtEventName;
    public Text txtTime;
    public GameObject Rewards;

    private Events MyEvent;
    private bool Count = false;
    private int Seconds = 0;

    public void StartEventInfo(Events myEvent)
    {
        gameObject.SetActive(true);
        MyEvent = myEvent;

        Count = true;
        StartCoroutine(IncTime());

        SetRewards();
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
            ++i;
        }
    }

    public void EndEventInfo()
    {
        gameObject.SetActive(false);
        Count = false;
        //Resolve event
    }

    public IEnumerator IncTime()
    {
        while (Count)
        {
            IncreaseTime();
            yield return new WaitForSeconds(3);
        }
    }

    public void IncreaseTime()
    {
        ++Seconds;

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
	
	}
}
