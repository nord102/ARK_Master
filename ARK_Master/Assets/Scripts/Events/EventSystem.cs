using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class EventSystem
{
	public static Events GenerateRoomEvent(int roomType, int numComponents)
	{
		//All rooms have default events that could happen (0:Fire, 1:Breach, 2:Enemy, 3:Sinister, 4:Fire+Alien, 5:Fire+Breach, 6:Breach+Alien)
		List<int> availableEvents = new List<int> (){0, 1, 2, 4, 5, 6};

        //Add Sinister Event
        if (Generate.instance.GetRoomGameObjectList().Count % (StateMachine.instance.numMaxRooms / 3) == 0)
        {
            return new Events(3, roomType, numComponents);
            //availableEvents.Add(3);
        }

		//Add a normal Event
		return new Events (availableEvents[Random.Range (0, availableEvents.Count)],roomType,numComponents);
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
}
