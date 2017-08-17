using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour {

	public bool playerInSight;
	public bool playerOnRight;
	public float targetDistance;
	public float frontTargetDistance;
	public float backTargetDistance;
	public float teleportDistance;

	public GameObject player;
	public GameObject target;
	public GameObject frontTarget;
	public GameObject backTarget;

	public Vector3 playerRelativePosition;

	void Awake () {

		player = GameObject.FindGameObjectWithTag ("Player");
		frontTarget = GameObject.Find ("EnemyFrontTarget");
		backTarget = GameObject.Find ("EnemyBackTarget");
		
	}
	
	void Update () {

		frontTargetDistance = Vector3.Distance (frontTarget.transform.position, gameObject.transform.position);
		backTargetDistance = Vector3.Distance (backTarget.transform.position, gameObject.transform.position);

		playerRelativePosition = player.transform.position - gameObject.transform.position;

		if (playerRelativePosition.x > 3f) {
			playerOnRight = true;
		} else if (playerRelativePosition.x < -3f) {
			playerOnRight = false;
		}

		if (frontTargetDistance < backTargetDistance) {
			target = frontTarget;
		}
		else if (frontTargetDistance > backTargetDistance)
		{
			target = backTarget;
		}

		targetDistance = Vector3.Distance (target.transform.position, gameObject.transform.position);
		
	}

	void OnTriggerStay (Collider other)
	{
		if (other.gameObject == player) {
			playerInSight = true;
		}	
	}

	void OnTriggerExit (Collider other)
	{
		if (other.gameObject == player) {
			playerInSight = false;
		}
	}
}
