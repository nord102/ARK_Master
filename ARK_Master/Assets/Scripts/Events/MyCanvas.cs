using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class MyCanvas : MonoBehaviour
{
    private Events MyEvent { get; set; }
	//public GameObject ChoicePrefab;
    //public GameObject Canvas;
    public Text txtTitle;
    public Text txtDescription;
    //public GameObject Choices;
    public GameObject MyImage;

    public GameObject RewardsParent;
    public GameObject RewardPrefab;
    public List<GameObject> RewardList;

    public Sprite Bronze;
    public Sprite Silver;
    public Sprite Gold;
    public Sprite GoldStar;
    public Sprite GrayStar;

    public GameObject StarPlaceholder;

    //private List<GameObject> txtChoices = new List<GameObject>();

    public MyCanvas()
    {
        RewardList = new List<GameObject>();
    }

    public void StartEvent(Events newEvent)
    {
        gameObject.SetActive(true);
        MyEvent = newEvent;
        StartEvent();
    }

    public void Close()
    {
        EmptyRewards();
        gameObject.SetActive(false);
    }

    public void EmptyRewards()
    {
        foreach (GameObject g in RewardList)
        {
            Destroy(g);
        }
        RewardList.Clear();
    }

    public void ResetDifficulty()
    {
        foreach (Transform child in StarPlaceholder.transform)
        {
            child.GetComponent<Image>().sprite = GrayStar;
        }
    }

    public void SetDifficulty()
    {
        int difficulty = MyEvent.eventDifficulty;

        //Debug.Log("ED: " + MyEvent.eventDifficulty);

        int i = 5 - MyEvent.eventDifficulty;
        foreach(Transform child in StarPlaceholder.transform)
        {
            if (i <= 0)
            {
                child.GetComponent<Image>().sprite = GoldStar;
            }
            else
            {
                child.GetComponent<Image>().sprite = GrayStar;
            }

            i--;
        }
    }

    private void StartEvent()
    {
        EmptyRewards();
        MyImage.GetComponent<Image>().sprite = MyEvent.eventImage;
        txtTitle.text = MyEvent.eventName;
        txtDescription.text = MyEvent.eventText;

        //Show Rewards
        GameObject newReward = null;
        int startX = 0;
        int startY = 30;
        int startZ = 0;

        int displaceY = -30;
        int rewardCount = 0;

        int yPos = 0;
        Sprite trophySprite = Bronze;
        foreach (Rewards r in MyEvent.SuccessRewards)
        {
            yPos = startY + displaceY * rewardCount;

            newReward = Instantiate(RewardPrefab) as GameObject;

            newReward.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            newReward.transform.SetParent(RewardsParent.transform, false);
            newReward.transform.position = new Vector3(0, 0, 0);
            newReward.transform.localPosition = new Vector3(startX, yPos, startZ); //Finally

            //Set Text and images
            GameObject timer = newReward.transform.Find("Timer").gameObject;
            GameObject trophy = newReward.transform.Find("Trophy").gameObject;
            GameObject rewardImage = newReward.transform.Find("RewardImage").gameObject;
            GameObject rewardValue = newReward.transform.Find("RewardValue").gameObject;

            timer.GetComponent<Text>().text = r.RewardTimer.ToString() + "s";
            rewardValue.GetComponent<Text>().text = r.RewardName;

            if(rewardCount == 0)
            {
                trophySprite = Bronze;
            }
            else if (rewardCount == 1)
            {
                trophySprite = Silver;
            }
            else if (rewardCount == 2)
            {
                trophySprite = Gold;
            }
            trophy.GetComponent<Image>().sprite = trophySprite;

            RewardList.Add(newReward);
            newReward = null;

            ++rewardCount;
        }

        SetDifficulty();

        gameObject.SetActive(true);

        //Used when this had different options to choose from
		/*GameObject newOption = null;
        Text text = null;
        EventTrigger eventTrigger = null;
        int index = 0;

		int displacementY = 5;
		 
        foreach(EventChoice ec in MyEvent.eventChoices)
        {
            ++index;
			newOption = Instantiate(ChoicePrefab) as GameObject;

			newOption.transform.SetParent(Choices.transform);
			newOption.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
			newOption.transform.position = new Vector3(-10, 0 + (displacementY * (index - 1)), Choices.transform.position.z);

            //eventTrigger = newOption.GetComponent<EventTrigger>();
            //EventTrigger.Entry clickCallback = new EventTrigger.Entry();
            text = newOption.GetComponent<Text>();


            //clickCallback.eventID = EventTriggerType.PointerClick;
            //clickCallback.callback.AddListener((eventdata) => ChoiceMade(index));
            text.text = index.ToString() + ") " + ec.choiceText;

			//newOption.GetComponent<RectTransform>().sizeDelta = new Vector2 (455, 18);


			Button b = newOption.GetComponent<Button>();
			b.onClick.AddListener(() => ChoiceMade (index - 1));


			txtChoices.Add(newOption);
        }*/

		//gameObject.SetActive (true);
    }
    
  //  public void ChoiceMade(int choice)
  //  {
		//for(int i = 0; i < txtChoices.Count; ++i)
		//{
		//	txtChoices[i].transform.SetParent(null);
		//	Destroy (txtChoices[i]);
		//}
		//txtChoices.Clear();

		//GameObject newOption = Instantiate(ChoicePrefab) as GameObject;
		//newOption.transform.SetParent(Choices.transform);
		//newOption.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
		//newOption.transform.position = new Vector3(-10, 0, Choices.transform.position.z);

		////MyEvent.eventChoices [choice].ResolveEvent ();
		//txtDescription.text = MyEvent.eventChoices [choice].FinalText;

		//Text text = newOption.GetComponent<Text>();
		//text.text = "Continue";

		//Button b = newOption.GetComponent<Button>();
		//b.onClick.AddListener(() => CloseCanvas());

		////gameObject.SetActive (false);
  //  }

	public void CloseCanvas()
	{
		Destroy (gameObject);
	}
}
