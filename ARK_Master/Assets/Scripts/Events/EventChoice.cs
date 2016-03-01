using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventChoice : MonoBehaviour {

	//public int ChoiceID;
	public string choiceText;
	public double percentChance;
	public string succeedText;
	public string failText;
	public string FinalText;
	public int helpingSkill;
	public double percentSkillHelps;
	public List<Rewards> SuccessRewards;
	public List<Rewards> FailureRewards;

	public EventChoice(string newChoiceText, double newPercentChance, string newSucceedText, string newFailText, int newHelpingSkill, double newPercentSkillHelps, List<Rewards> success, List<Rewards> failure)
	{
		choiceText = newChoiceText;
		percentChance = newPercentChance;
		succeedText = newSucceedText;
		failText = newFailText;
		helpingSkill = newHelpingSkill;
		percentSkillHelps = newPercentSkillHelps;
		SuccessRewards = success;
		FailureRewards = failure;
	}

	public void ResolveEvent()
	{
		double result = Random.Range(0, 100);

		//Check if the player has this skill active
		if (helpingSkill != -1) {
			if (StateMachine.instance.AllAvailableSkills [helpingSkill].isActive) {
				result += percentSkillHelps;
			}
		}

		if (result >= percentChance) {
			//Randomly pick one of the rewards and activate it
			FinalText = succeedText;
			SuccessRewards[Random.Range(0, SuccessRewards.Count - 1)].ActivateReward();

		} else {
			//Randomly pick one of the penalties and activate it
			FinalText = failText;
			FailureRewards[Random.Range(0, FailureRewards.Count - 1)].ActivateReward();
		}
	}
}
