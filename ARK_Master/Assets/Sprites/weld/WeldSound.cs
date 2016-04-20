using UnityEngine;
using System.Collections;

public class WeldSound : MonoBehaviour {

    static public WeldSound instance = null;

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
