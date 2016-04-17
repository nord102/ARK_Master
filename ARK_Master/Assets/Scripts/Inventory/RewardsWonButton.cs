using UnityEngine;
using System.Collections;

public class RewardsWonButton : MonoBehaviour
{
    public void MyOnClick()
    {
        gameObject.SetActive(false);
        StateMachine.instance.PlayerControl = true;
        StateMachine.instance.eventActive = false;
    }
}
