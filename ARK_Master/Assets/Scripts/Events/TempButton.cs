using UnityEngine;
using System.Collections;

public class TempButton : MonoBehaviour {

	public void DoIt()
	{
        StateMachine.instance.FireEvent();
	}

    public void StartEvent()
    {
        StateMachine.instance.StartEvent();
    }
}
