using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerInfo {

	public string PlayerName = string.Empty;
	public double CurrentPlayer;
	public double MaxHealth = 100;
	public double CurrentHealth = 100;
	public double MaxShield = 100;
	public double CurrentShield = 0;
	public double MaxEnergy = 10;
	public double CurrentEnergy = 10;
	public double ATKDamage = 1;

	//This is unused for the current player - it is only used for the PreviousPlayers to remember what skills they had
	public List<Skills> CurrentSkills;

	public PlayerInfo(int newCurrentPlayer)
	{
		CurrentPlayer = newCurrentPlayer;
		//CurrentSkills = new List<Skills> ();
	}

	public void SetHealth(double value)
	{
		if (CurrentHealth + value > MaxHealth) {
			CurrentHealth = MaxHealth;
		}
		else if ( CurrentHealth + value <= 0)
		{
			CurrentHealth = 0;
			StateMachine.instance.GameOver();
			Debug.Log("GAME OVER");

		} else {
			CurrentHealth += value;
		}

        // Set the health bar's value to the current health.
        StateMachine.instance.playerHealthBar.fillAmount = (float)CurrentHealth / 100;
        if (StateMachine.instance.playerHealthBar.fillAmount <= .25)
        {
            StateMachine.instance.playerHealthBar.color = Color.red;
        }
        else if (StateMachine.instance.playerHealthBar.fillAmount <= .50)
        {
            StateMachine.instance.playerHealthBar.color = Color.yellow;
        }
        else
        {
            StateMachine.instance.playerHealthBar.color = Color.green;
        }
	}

	public void SetShield(double value)
	{
		if (CurrentShield + value > MaxShield) {
			CurrentShield = MaxShield;
		} else if (CurrentShield + value <= 0) {
			SetHealth(CurrentShield);
			CurrentShield = 0;
		} else {
			CurrentShield += value;
		}
        // Set the health bar's value to the current health.
        StateMachine.instance.playerShieldBar.fillAmount = (float)CurrentShield / 100;
	}

	public void SetEnergy(double value)
	{
		if (CurrentEnergy + value > MaxEnergy) {
			CurrentEnergy = MaxEnergy;
		} else if (CurrentEnergy - value <= 0) {
			CurrentEnergy = 0;
		} else {
			CurrentEnergy += value;
		}
	}

}
