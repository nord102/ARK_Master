using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class EventSystem{

	public static Events GenerateRoomEvent(int roomType)
	{
		//All rooms have default events that could happen (Fire, Breach, Enemy)
		List<int> availableEvents = new List<int> (){0,1,2};

		//Add event type 3 (Encounter previous character) if there are dead previous characters, and other conditions are met?
		if (StateMachine.instance.PreviousPlayers.Count != 0) { // and anything else? time passed? events solved?
			availableEvents.Add(3);
		}

		//Add extra event types based on room type
		switch (roomType) {
		case 0: //Med Bay
			availableEvents.Add(4);
			break;
		case 1: //Engineering
			availableEvents.Add(5);
			break;
		}

		//Pick a random index number, that's the type of event
		return new Events (availableEvents[Random.Range (0, availableEvents.Count - 1)]);

		//For Testing only - Generate only the fire event
		//return new Events (0);
	}

	public static bool DisplayEventScreen(Events thisEvent)
	{
		//Display the event screen - Get user OK or Cancel
		bool returnValue = true;

		return returnValue;
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
