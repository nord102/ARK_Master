using UnityEngine;
using System.Collections;

public class Audio : MonoBehaviour
{    
    //Audio
    public AudioClip[] audioClip;
    public AudioSource audioSource;
    //public AudioEchoFilter echo;

    /************************************************************************************
    * FUNCTION :       PlaySound
    *
    * DESCRIPTION :    This function assigns the AudioSource a clip and then plays that clip.                
    *
    * PARAMETER :      int clip:   Represents the clip position in the AudioClip array.                                                             
    *************************************************************************************/
    public void PlaySound(int clip)
    {
        audioSource.clip = audioClip[clip];
        audioSource.Play();
    }

    /************************************************************************************
     * FUNCTION :       ControlEcho
     *
     * DESCRIPTION :    This function enables or disables the Audio Echo Filter.               
     *
     * PARAMETER :      bool isEcho:    Represents whether or not the Audio Echo Filter 
     *                                  should be enabled or disabled.                                                            
     *************************************************************************************/
    //public void ControlEcho(bool isEcho)
    //{
    //    if (isEcho)
    //    {
    //        echo.enabled = true;
    //    }
    //    else
    //    {
    //        echo.enabled = false;
    //    }
    //}
}
