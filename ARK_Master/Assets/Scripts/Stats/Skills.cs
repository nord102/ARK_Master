using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Skills {

	public int skillID;
	public Sprite symbol;
	public string skillName;
	public double activationCost;
	public double coolDownLength;
	public double currentCoolDown = 0;
	public bool isOwned = false;
	public bool isActive = false;

	public Skills(int newSkillID, Sprite newSymbol, string newName, double newActivationCost, double newCoolDownLength)
	{
		skillID = newSkillID;
		symbol = newSymbol;
		skillName = newName;
		activationCost = newActivationCost;
		coolDownLength = newCoolDownLength;
	}

}
