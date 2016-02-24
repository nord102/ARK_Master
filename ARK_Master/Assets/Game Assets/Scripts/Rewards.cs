using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Rewards : MonoBehaviour {

	public string RewardName;
	public Image RewardImage;
	public double RewardTimer;
	public int HPChange;
	public int EnergyChange;
	public int ShieldChange;
	public int ShipResourcesFound;
	public int SkillFound;
	public int CharacterUnlocked;

	public Rewards(int lootTable, int eventType)
	{
		//Grab random reward from database using this lootTable number (and maybe the event type? join a lookup table to determine which event types can get which rewards)

		//Parse the result set

		//Apply the results to the variables
	}

	public Rewards(string newRewardName, int newHPChange, int newEnergyChange, int newShieldChange, int newShipResourcesFound, int newSkillFound = -1, int newCharacterUnlocked = -1, Image newRewardImage = null, double newRewardTimer = 0)
	{
		RewardName = newRewardName;
		HPChange = newHPChange;
		EnergyChange = newEnergyChange;
		ShieldChange = newShieldChange;
		ShipResourcesFound = newShipResourcesFound;
		SkillFound = newSkillFound;
		CharacterUnlocked = newCharacterUnlocked;
		RewardImage = newRewardImage;
		RewardTimer = newRewardTimer;
	}

	public void ActivateReward()
	{
		StateMachine.instance.pInfo.SetHealth(HPChange);
		StateMachine.instance.pInfo.SetEnergy(EnergyChange);
		StateMachine.instance.pInfo.SetShield(ShieldChange);
		StateMachine.instance.sInfo.Resources += ShipResourcesFound;
		if (SkillFound != -1) {
			StateMachine.instance.AllAvailableSkills [SkillFound].isOwned = true;
		}
		if (CharacterUnlocked != -1) {
			//Add Character to statemachine...
		}

	}
}
