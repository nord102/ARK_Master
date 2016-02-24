using UnityEngine;
using System.Collections;

/// \class PortalSound
/// \brief This class plays the portal sound.
///
[RequireComponent(typeof(AudioSource))]
public class AlienAttack : MonoBehaviour
{

    static public AlienAttack instance = null;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    public void PlaySound()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();
    }
}
