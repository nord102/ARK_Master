using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShipInfo {

	public double Energy;
	public double Resources;
	public double LifeSupport;
	public double StageNumber;
	//public List<Rooms> RoomList;
	public string CurrentRoom;

    public void SetResources(double value)
    {
        if (Resources + value <= 0)
        {
            Resources = 0;
        }
        else
        {
            Resources += value;
        }

        // Set the health bar's value to the current health.
        StateMachine.instance.shipResources.text = Resources.ToString();
    }

}
