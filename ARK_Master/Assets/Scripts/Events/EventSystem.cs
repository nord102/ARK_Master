using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class EventSystem{

	public static Events GenerateRoomEvent(int roomType, int numComponents)
	{
		//All rooms have default events that could happen (0:Fire, 1:Breach, 2:Enemy)
		List<int> availableEvents = new List<int> (){0,1,2}; //+ 2

		//Add event type 3 (Encounter previous character) if there are dead previous characters, and other conditions are met?
		if (StateMachine.instance.PreviousPlayers.Count != 0) { // and anything else? time passed? events solved?
			availableEvents.Add(3);
		}

		//Add extra event types based on room type
		switch (roomType) {
		case 0: //Standard Room type, don't add anything
			break;
        case 1: //Med Bay
            availableEvents.Add(4); 
			break;
        case 2: //Engineering
            availableEvents.Add(5);
            break;
		}

		//Pick a random index number, that's the type of event
		return new Events (availableEvents[Random.Range (0, availableEvents.Count)],roomType,numComponents);
        //return new Events(0);

		//For Testing only - Generate only the fire event
		//return new Events (0);
	}

    /// <summary>
    /// Pass in the EventInfo part of the canvas
    /// </summary>
    /// <param name="thisEvent"></param>
    /// <param name="eventInfo"></param>
	public static void DisplayEventTimer(Events thisEvent, GameObject eventInfo)
	{
        //Display the event Timer panel and populate with event rewards
        //StateMachine.instance.EventDetails.SetActive(true);
        EventInfo eventInfoScript = eventInfo.GetComponent<EventInfo>();
        eventInfoScript.StartEventInfo(thisEvent);
	}

	//This belongs in the RoomEntering Code
	public static void PopulateRoom()
	{
		//Determine the bad guys
		//SpawnBadGuys(event.eventType, event.eventDifficulty);
		
		//Determine object and location of rewards
		//SpawnRewards(event.SuccessRewards);
	}

}
