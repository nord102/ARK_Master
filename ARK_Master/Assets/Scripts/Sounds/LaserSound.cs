using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class LaserSound : MonoBehaviour
{

	static public LaserSound instance = null;

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