using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Events : MonoBehaviour {

	public string eventName;
	public Image eventImage;
	public string eventText;
	public int eventDifficulty;
	public List<Rewards> SuccessRewards = new List<Rewards> ();
	public List<int> Enemies = new List<int> ();
	//public List<EventChoice> eventChoices;

	public Events(int eventType)
	{
		//List<Rewards> FailureRewards = new List<Rewards>();
		//eventChoices = new List<EventChoice>();

		//Determine event difficulty - based on rooms explored, strength of main character...?
		eventDifficulty = StateMachine.instance.DetermineEventDifficulty ();

		//Use difficulty to determine rewards
		//10 loot tables
		//difficulty = loot table * 2 - 1: example: difficulty 4 = loot table 7
		//bronze reward = loot table with 25% chance to be loot table - 1
		//silver reward = loot table with 5% chance to be loot table + 1
		//gold reward = loot table + 1

		//need a loot table database, randomly pull required data out of database

		int lootTableBronze = (eventDifficulty * 2) - 1;
		int lootTableSilver = (eventDifficulty * 2) - 1;
		int lootTableGold = (eventDifficulty * 2);
		
		//Determine loot tables
		if (lootTableBronze != 1 && Random.value <= .25) {
			lootTableBronze -= 1;
		}
		if (Random.value >= .95) {
			lootTableSilver += 1;
		}

		switch (eventType) {
		case 0:
			eventName = "Fire";
			eventText = "The room, The room, The room is on fire!";
			eventImage = (Image)Resources.Load ("Images/flames");
			break;
		}

		//Determine the 3 rewards
		SuccessRewards.Add (new Rewards (lootTableBronze, eventType));
		SuccessRewards.Add (new Rewards (lootTableSilver, eventType));
		SuccessRewards.Add (new Rewards (lootTableGold, eventType));

		//Determine the bad guys based on room type, difficulty?
		Enemies.Add (1);
		Enemies.Add (1);
		Enemies.Add (2);

//		switch (eventType) {
//		case 0:
//			eventName = "Fire";
//				eventText = "The room is on fire!";
//			eventImage = (Image)Resources.Load("Images/flames");
//
//			SuccessRewards.Add(new Rewards("You find a medkit that heals you (+10 HP)", 10, 0, 0, 0));
//			FailureRewards.Add(new Rewards("You take some damage trying to put out the fire (-10 HP)", -10, 0, 0, 0));
//			eventChoices.Add(new EventChoice("Try to put it out", 50, "You successfully put the fire out", "You are unable to put the fire out", 0, 50, SuccessRewards, FailureRewards));
//			break;
//		case 1:
//			eventName = "Breach";
//			eventText = "There's a hole in the hull!";
//			eventImage = (Image)Resources.Load("Images/flames");
//			
//			SuccessRewards.Add(new Rewards("While repairing the room, you spot an Extinguisher Module (Module Found: Extinguisher)", 0, 0, 0, 0, 0));
//			SuccessRewards.Add(new Rewards("You find a medkit that heals you (+10 HP)", 10, 0, 0, 0));
//			FailureRewards.Add(new Rewards("The strain of trying to repair the room has left you weaker (-10 HP)", -10, 0, 0, 0));
//			FailureRewards.Add(new Rewards("You were almost sucked out into space! (-20 HP)", -20, 0, 0, 0));
//			eventChoices.Add(new EventChoice("Patch the hull", 50, "You successfully patch the hole in the hull", "You are unable to get the hull patched", -1, 0, SuccessRewards, FailureRewards));
//			break;
//		case 2:
//			eventName = "Enemy";
//			eventText = "A being of pure energy materializes in front of you!";
//			eventImage = (Image)Resources.Load ("Images/flames");
//
//			SuccessRewards.Add(new Rewards("The enemy has dropped it's laser gun module - it's still functional! (Module Found: Laser)", 0, 0, 0, 0, 1));
//			SuccessRewards.Add(new Rewards("The enemy's weapon shatters - you think you can make use of the scraps (+20 Ship Resources)", 0, 0, 0, 20, 0));
//			SuccessRewards.Add(new Rewards("The enemy is destroyed - you're just thankful to be alive", 0, 0, 0, 0));
//
//			FailureRewards.Add(new Rewards("You've been hit by the enemy's weapon! (-20 HP)", -20, 0, 0, 0));
//			FailureRewards.Add(new Rewards("The enemy's weapon was set to stun - You feel your energy being sapped! (-10 Energy)", 0, -10, 0, 0));
//
//			eventChoices.Add(new EventChoice("Fight the enemy!", 50, "The enemy has been destroyed", "The enemy takes a shot back at you!", 1, 25, SuccessRewards, FailureRewards));
//
//			break;
//		case 3:
//
//			//Randomly pick one of the currently dead previous players - don't need to check if there's anyone in the list - they have to be to get to this point
//			PlayerInfo p = StateMachine.instance.PreviousPlayers[Random.Range(0, StateMachine.instance.PreviousPlayers.Count - 1)];
//
//			eventName = "Sinister";
//			eventText = "Something is lurching towards you... It's nametag says... " + p.PlayerName + "!" ;
//			eventImage = (Image)Resources.Load("Images/flames");
//
//			SuccessRewards.Add(new Rewards("Generic win condition message", 0, 0, 0, 0));
//			FailureRewards.Add(new Rewards("Generic fail condition message", 0, 0, 0, 0));
//
//			//Need to add something to event choice class to remove this guy from the PreviousPlayers if it is destroyed
//			//We now have access to the previous players skills, what should we do with them...?
//			eventChoices.Add(new EventChoice("Fight the enemy!", 25, "The enemy has been destroyed", "The enemy takes a shot back at you!", 1, 25, SuccessRewards, FailureRewards));
//
//			break;
//		case 4: 
//			break;
//		}
	}
}
