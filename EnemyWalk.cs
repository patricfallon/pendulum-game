using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyWalk : MonoBehaviour {

	private NavMeshAgent navMeshAgent;
	private EnemySight enemySight;
	private EnemyState enemyState;
	private Animator animator;
	private BoxCollider walkCollider;
	private BoxCollider hitCollider;

	public float enemySpeed;
	public float enemyCurrentSpeed;
	public bool facingRight;

	public GameObject spriteObject;
	public GameObject enemyStanding;
	public GameObject walkHitBox;

	void Awake () {

		navMeshAgent = GetComponent<NavMeshAgent> ();
		enemySight = GetComponent<EnemySight> ();
		enemyState = GetComponent <EnemyState> ();
		animator = spriteObject.GetComponent<Animator> ();
		walkCollider = gameObject.GetComponent<BoxCollider> ();
		hitCollider = walkHitBox.GetComponent<BoxCollider> ();

		navMeshAgent.speed = enemySpeed;
		
	}
	
	void Update () {

		if (enemySight.playerOnRight == true && !facingRight && enemyState.swiping == false && enemyState.tookDamage == false) {
			Flip ();
		} else if (enemySight.playerOnRight == false && facingRight == true && enemyState.swiping == false && enemyState.tookDamage == false) {
			Flip ();
		}

		if (enemyState.currentState == EnemyState.currentStateEnum.walk) {
			Walk ();
		} else if (enemyState.currentState == EnemyState.currentStateEnum.idle ||
		           enemyState.currentState == EnemyState.currentStateEnum.standing ||
		           enemyState.currentState == EnemyState.currentStateEnum.teleport) {
			Stop ();
		}

		if (enemySight.targetDistance < 3f) {
			navMeshAgent.SetDestination (enemySight.gameObject.transform.position);
			animator.SetBool ("IsStanding", true);
			walkCollider.enabled = false;
			hitCollider.enabled = false;
			enemyStanding.gameObject.SetActive (true);

		} else if (enemySight.targetDistance >= 4f) {
			animator.SetBool ("IsStanding", false);
			walkCollider.enabled = true;
			hitCollider.enabled = true;
			enemyStanding.gameObject.SetActive (false);
		}

	}

	void Walk ()
	{
		navMeshAgent.speed = enemySpeed;
		enemyCurrentSpeed = navMeshAgent.velocity.sqrMagnitude;
		navMeshAgent.SetDestination (enemySight.target.transform.position);
		navMeshAgent.updateRotation = false;
		
	}

	void Stop ()
	{
		navMeshAgent.ResetPath ();
	}

	void Flip()
	{
		facingRight = !facingRight;
		Vector3 thisScale = transform.localScale;
		thisScale.x *= -1;
		transform.localScale = thisScale;
	}
		
}
