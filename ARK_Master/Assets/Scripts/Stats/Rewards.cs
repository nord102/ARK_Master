using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Rewards : MonoBehaviour
{

    public int Id;
    public string RewardName;
    public string RewardImagePath;
    public int RewardTimer;
    public int HPChange;
    public int EnergyChange;
    public int ShieldChange;
    public int ShipResourcesFound;
    public int SkillFound;
    public int CharacterUnlocked;

    public int LootTableValue;
    public int EventType;

    public Image RewardImage;

    //Used by db
    public Rewards(int Id, string RewardName, string RewardImagePath, int RewardTimer, int HPChange, int EnergyChange, int ShieldChange, int ShipResourcesFound, int SkillFound, int CharacterUnlocked, int LootTableValue, int EventType)
    {   //sublime baby
        this.Id = Id;
        this.RewardName = RewardName;
        this.RewardImagePath = RewardImagePath;
        this.RewardTimer = RewardTimer;
        this.HPChange = HPChange;
        this.EnergyChange = EnergyChange;
        this.ShieldChange = ShieldChange;
        this.ShipResourcesFound = ShipResourcesFound;
        this.SkillFound = SkillFound;
        this.CharacterUnlocked = CharacterUnlocked;
        this.LootTableValue = LootTableValue;
        this.EventType = EventType;
    }

    public Rewards(string newRewardName, int newHPChange, int newEnergyChange, int newShieldChange, int newShipResourcesFound, int newSkillFound = -1, int newCharacterUnlocked = -1, Image newRewardImage = null, int newRewardTimer = 0)
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
        try
        {
            Debug.Log("Health: " + HPChange + " Energy: " + EnergyChange + " Shield: " + ShieldChange + " Money: " + ShipResourcesFound);
            StateMachine.instance.pInfo.SetHealth(HPChange);
            StateMachine.instance.pInfo.SetEnergy(EnergyChange);
            StateMachine.instance.pInfo.SetShield(ShieldChange);
            StateMachine.instance.sInfo.SetResources(ShipResourcesFound);
            if (SkillFound != -1)
            {
                StateMachine.instance.AllAvailableSkills[SkillFound].isOwned = true;
            }
            if (CharacterUnlocked != -1)
            {
                //Add Character to statemachine...
            }
        }
        catch
        {
            //The event screwed up, activating a skill we don't own maybe?
            Debug.Log("Error activating Reward");
        }

    }
}
