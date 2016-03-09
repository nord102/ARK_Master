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

	//private List<GameObject> txtChoices = new List<GameObject>();

	//for button click event
	//http://answers.unity3d.com/questions/791573/46-ui-how-to-apply-onclick-handler-for-button-gene.html
    public MyCanvas()
    {

    }

    public void StartEvent(Events newEvent)
    {
        MyEvent = newEvent;
        StartEvent();
    }

    private void StartEvent()
    {
        //MyImage.GetComponent<Image>().sprite = MyEvent.eventImage.sprite;
        txtTitle.text = MyEvent.eventName;
        txtDescription.text = MyEvent.eventText;

        /*Used when this had different options to choose from
		GameObject newOption = null;
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
