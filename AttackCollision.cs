using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollision : MonoBehaviour {

	public bool knockDownAttack;
	public float attackStrength;
	public float playerAttackStrength;

	private GameObject otherObject;
	private PlayerController playerState;
	private PlayerStats playerStats;
	private EnemyStats enemyStats;
	private EnemyState enemyState;

	void Start (){

		playerStats = FindObjectOfType<PlayerStats> ();
		playerAttackStrength = playerStats.playerAttack;

	}

	void FixedUpdate(){

		playerAttackStrength = playerStats.playerAttack;
		
	}

	void OnTriggerEnter (Collider other)
	{
		if (gameObject.tag == "PunchBox" && other.tag == "EnemyHitBox") {
			EnemyTakeDamage (other.gameObject);
		} else if (gameObject.tag == "KickBox" && other.tag == "EnemyHitBox") {
			EnemyTakeDamage (other.gameObject);
		} else if (gameObject.tag == "SwipeBox" && other.tag == "PlayerHitBox") {
			PlayerTakeDamage (other.gameObject);
		} else if (gameObject.tag == "Fireball" && other.tag == "PlayerHitBox") {
			PlayerTakeDamage (other.gameObject);
		} else
			return;
	}

	void EnemyTakeDamage (GameObject other)
	{

		otherObject = other.transform.parent.gameObject;
		enemyState = otherObject.GetComponent<EnemyState> ();
		enemyStats = otherObject.GetComponent<EnemyStats> ();

		enemyStats.health = enemyStats.health - playerAttackStrength;

		enemyState.tookDamage = true;
	}

	void PlayerTakeDamage(GameObject other)
	{
		otherObject = other.transform.parent.gameObject;
		playerState = otherObject.GetComponent<PlayerController> ();
		playerStats = otherObject.GetComponent<PlayerStats> ();

		playerStats.health = playerStats.health - attackStrength;
		playerState.tookDamage = true;

		if (knockDownAttack == true) {
			playerState.knockedDown = true;
		} 
	}

}
