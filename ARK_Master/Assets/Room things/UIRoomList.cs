using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIRoomList : MonoBehaviour {

    public Image roomImage;
    public string roomPrice;

    public UIRoomList(Image i, string p)
    {
        roomImage = i;
        roomPrice = p;
    }

}
