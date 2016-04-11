﻿using UnityEngine;
using System.Collections;

public class ExtinguishSpray : MonoBehaviour {

	public Animator animator;
	Vector3 direction;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log (this.direction);
		if (this.animator.isActiveAndEnabled== false) {
			Destroy (this.gameObject);
		}
	}

	public void Trajectory(Vector3 angle)
	{
        //this.direction = v;
        //this.gameObject.transform.rotation = Quaternion.Angle(this.gameObject.transform.position, angle);
        this.gameObject.SetActive(true);

	}
}