using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : MonoBehaviour {

	public float attackRange;
	public float fireballRange;
	public float lungeRange;
	public float attackStartDelay;
	public GameObject spriteObject;

	public GameObject attackOneBox, attackTwoBox;
	public Sprite attackOneHF, attackTwoHF;
	public Sprite currentSprite;

	private NavMeshAgent navMeshAgent;
	private EnemyState enemyState;

	void Awake () {

		navMeshAgent = GetComponent<NavMeshAgent> ();
		enemyState = GetComponent<EnemyState> ();

	}
	
	void Update () {

		currentSprite = spriteObject.GetComponent<SpriteRenderer> ().sprite;

		if (enemyState.currentState == EnemyState.currentStateEnum.attack) {
			Attack ();
		}

	}

	void Attack ()
	{
		navMeshAgent.ResetPath ();

		if (currentSprite == attackOneHF) {
			attackOneBox.gameObject.SetActive (true);
		} else {
			attackOneBox.gameObject.SetActive (false);
		}

		if (currentSprite == attackTwoHF) {
			attackTwoBox.gameObject.SetActive (true);
		} else {
			attackTwoBox.gameObject.SetActive (false);
		}

	}
}
