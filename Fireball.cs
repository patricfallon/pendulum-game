﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour {

	public float speed;

	public AudioClip hitBlast;
	public AudioClip launchBlast;

	private GameObject player;
	private float step;
	private AudioSource fireballAudio;

	void Start () {

		player = GameObject.FindGameObjectWithTag ("Player");
		fireballAudio = GetComponent<AudioSource> ();

		fireballAudio.PlayOneShot (launchBlast, 0.5f);

		DestroyObject (this.gameObject, 3f);
	}
	
	void Update () {

		step = speed * Time.deltaTime;

		transform.position = Vector3.MoveTowards(this.transform.position, player.transform.position, step);
		
	}

	void OnTriggerEnter(Collider other){

		fireballAudio.PlayOneShot (hitBlast, 0.5f);

//This doesn't work correctly. The object collides with the player, but isn't always destroyed. 
//I wonder if it's conflicting with the initial "DestroyObject" in the Start function.
		if (other.tag == "Player"){

			DestroyObject (this.gameObject);

		}

	}
		
}
