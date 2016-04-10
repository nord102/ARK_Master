using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static Vector3 roomPos = new Vector3(-5f,25f,10f); //Starting Camera Pos

    void LateUpdate ()
    {
        transform.position = Vector3.Lerp(transform.position, roomPos, 1.0f * Time.deltaTime);
    }
}
