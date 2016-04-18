using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Data;

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
    public int BuildingUnlocked;
    public int LootTableValue;
    public int EventType;

    public Image RewardImage;

    //Used by db
    public Rewards(int Id, string RewardName, string RewardImagePath, int RewardTimer, int HPChange, int EnergyChange, int ShieldChange, int ShipResourcesFound, int SkillFound, int CharacterUnlocked, int BuildingUnlocked, int LootTableValue, int EventType)
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
        this.BuildingUnlocked = BuildingUnlocked;
        this.LootTableValue = LootTableValue;
        this.EventType = EventType;
    }

    public Rewards(string newRewardName, int newHPChange, int newEnergyChange, int newShieldChange, int newShipResourcesFound, int newSkillFound = -1, int newCharacterUnlocked = -1, int newBuildingUnlocked = -1, Image newRewardImage = null, int newRewardTimer = 0)
    {
        RewardName = newRewardName;
        HPChange = newHPChange;
        EnergyChange = newEnergyChange;
        ShieldChange = newShieldChange;
        ShipResourcesFound = newShipResourcesFound;
        SkillFound = newSkillFound;
        CharacterUnlocked = newCharacterUnlocked;
        BuildingUnlocked = newBuildingUnlocked;
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
                //StateMachine.instance.db.ExecuteNonQuery("Update CharacterAvailability Set LocalUnlocked = 1 Where CharacterID = " + CharacterUnlocked);
            }
            if (BuildingUnlocked != -1)
            {
                //Add Building to statemachine...
                foreach (DataRow d in GlobalVariables.roomAvailability.Rows)
                {
                    if ((string)d[6] == string.Empty + BuildingUnlocked)
                    {
                        d[4] = "1";
                        break;
                    }
                }
                //StateMachine.instance.db.ExecuteNonQuery("Update RoomAvailability Set LocalUnlocked = 1 Where RoomID = " + BuildingUnlocked);
            }
        }
        catch
        {
            //The event screwed up, activating a skill we don't own maybe?
            Debug.Log("Error activating Reward");
        }

    }
}
