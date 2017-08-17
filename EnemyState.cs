using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyState : MonoBehaviour {

	public GameObject spriteObject;
	public GameObject fireball;
	public Transform fireballSpawnLeft;
	public Transform fireballSpawnRight;

	public enum currentStateEnum {idle=0, walk=1, standing=2, attack=3, teleport=4, hit=5};
	public currentStateEnum currentState;

	public bool tookDamage;
	public bool swiping;

	public float stunTime;
	public float knockBack;
	public float lungeTime;
	public float lungeForce;

	private NavMeshAgent navMeshAgent;
	private EnemySight enemySight;
	private EnemyAttack enemyAttack;
	private EnemyWalk enemyWalk;
	private Animator animator;
	private AnimatorStateInfo currentStateInfo;
	private PlayerController playerController;
	private Rigidbody rb;
	private ParticleSystem blood;
	private AudioSource enemyAudio;
	private EnemyStats enemyStats;
	private GameObject player;

	private float shootWaitTime;
	private float shootTimer;
	private float swipeTimer;
	private float timeBetweenSwipes;
	private float shakeDuration;
	private float shakeMagnitude;
	private float step;

	static int currentAnimState;
	static int idleState = Animator.StringToHash ("Base Layer.Idle");
	static int walkState = Animator.StringToHash ("Base Layer.Walk");
	static int standingIdleState = Animator.StringToHash ("Base Layer.Standing_Idle");
	static int attackState = Animator.StringToHash ("Base Layer.Swipe");
	static int hitState = Animator.StringToHash ("Base Layer.Hit");
	static int teleportState = Animator.StringToHash ("Base Layer.Teleport");

	void Awake () {

		player = GameObject.FindGameObjectWithTag ("Player");
		navMeshAgent = GetComponent<NavMeshAgent> ();
		enemySight = GetComponent<EnemySight> ();
		enemyAttack = GetComponent<EnemyAttack> ();
		enemyWalk = GetComponent<EnemyWalk> ();
		rb = GetComponent<Rigidbody> ();
		animator = spriteObject.GetComponent<Animator> ();
		playerController = FindObjectOfType<PlayerController> ();
		blood = GetComponentInChildren<ParticleSystem> ();

		shootWaitTime = 5;

		timeBetweenSwipes = 3;

		shakeDuration = 0.5f;
		shakeMagnitude = 0.6f;
		
	}
	
	void Update () {

		shootTimer += Time.deltaTime;
		swipeTimer += Time.deltaTime;

		if (tookDamage == true) {
			if (enemyWalk.facingRight == false && transform.position.x >= player.transform.position.x) {
				rb.AddForce (transform.right * (knockBack *= 0.8f), ForceMode.VelocityChange);
			} else if (enemyWalk.facingRight == true && transform.position.x >= player.transform.position.x) {
				rb.AddForce (transform.right * (knockBack *= 0.8f), ForceMode.VelocityChange);
			} else if (enemyWalk.facingRight == true && transform.position.x < player.transform.position.x + + 0.5f) {
				rb.AddForce (-transform.right * (knockBack *= 0.85f), ForceMode.VelocityChange);
			} else if (enemyWalk.facingRight == false && transform.position.x < player.transform.position.x + 0.5f) {
				rb.AddForce (-transform.right * (knockBack *= 0.85f), ForceMode.VelocityChange);
			} 

			if (blood != null) {
				blood.Emit (80);
			}

			StartCoroutine (CameraShake ());
			StartCoroutine (TookDamage ());
		} else if (tookDamage == false &&
		           enemySight.playerInSight == true &&
		           enemySight.targetDistance < enemyAttack.attackRange &&
		           navMeshAgent.velocity.sqrMagnitude < enemyAttack.attackStartDelay &&
		           playerController.knockedDown == false &&
		           playerController.tookDamage == false &&
				   playerController.isDead == false) {
			animator.SetBool ("Teleport", false);
			animator.SetBool ("IsSwiping", true);
			animator.SetBool ("Walk", false);

			swipeTimer = 0;

		} else if (tookDamage == false && enemySight.playerInSight == true) {
			animator.SetBool ("Teleport", false);
			animator.SetBool ("Walk", true);
			animator.SetBool ("IsSwiping", false);
		} else if (tookDamage == false && enemySight.playerInSight == false && enemySight.targetDistance < enemySight.teleportDistance) {
			animator.SetBool ("Teleport", false);
			animator.SetBool ("Walk", false);
			animator.SetBool ("IsSwiping", false);
		} else if (tookDamage == false && enemySight.playerInSight == false && enemySight.targetDistance >= enemySight.teleportDistance) {
			animator.SetBool ("Teleport", true);
			animator.SetBool ("Walk", false);
			animator.SetBool ("IsSwiping", false);

			swipeTimer = 0;
		}

		if (currentAnimState == idleState) {

			currentState = currentStateEnum.idle;
			swiping = false;
			
		} else if (currentAnimState == walkState) {

			currentState = currentStateEnum.walk;
			swiping = false;
			
		} else if (currentAnimState == standingIdleState) {

			currentState = currentStateEnum.standing;
			swiping = false;
			
		} else if (currentAnimState == attackState) {

			currentState = currentStateEnum.attack;
			swiping = true;
			
		} else if (currentAnimState == teleportState) {

			currentState = currentStateEnum.teleport;
			swiping = false;

		} else if (currentAnimState == hitState) {

			currentState = currentStateEnum.hit;
			swiping = false;

		}

		if (tookDamage == false && 
			playerController.knockedDown == false &&
			playerController.isDead == false &&
			enemySight.targetDistance > enemyAttack.attackRange && 
			enemySight.targetDistance < enemySight.teleportDistance &&
			enemySight.targetDistance > enemyAttack.fireballRange &&
			shootTimer > shootWaitTime){

			Shoot ();
		}

		if (tookDamage == false && 
			playerController.knockedDown == false &&
			playerController.isDead == false &&
			enemySight.targetDistance >= enemyAttack.attackRange && 
			enemySight.targetDistance < enemySight.teleportDistance && 
			enemySight.targetDistance < enemyAttack.fireballRange &&
			enemySight.targetDistance <= enemyAttack.lungeRange &&
			swipeTimer >= timeBetweenSwipes) {

			StartCoroutine (Lunge ());
		}

		currentStateInfo = animator.GetCurrentAnimatorStateInfo (0);
		currentAnimState = currentStateInfo.fullPathHash;

	}

	void Shoot(){

		if(enemyWalk.facingRight == false){

			Instantiate (fireball, fireballSpawnLeft.position, fireballSpawnLeft.rotation);
		} else if (enemyWalk.facingRight == true){

			Instantiate (fireball, fireballSpawnRight.position, fireballSpawnRight.rotation);
		}
			
		shootTimer = 0;
	}

	IEnumerator CameraShake()
	{

			float elapsed = 0.0f;

			Vector3 originalCamPos = Camera.main.transform.position;

			while (elapsed < shakeDuration) {

				elapsed += Time.deltaTime;          

				float percentComplete = elapsed / shakeDuration;         
				float damper = 1f - Mathf.Clamp (2f * percentComplete - 0f, 0f, 0f);

				// map value to [-1, 1]
				float x = Random.value * 2.0f - 1.0f;
				float y = Random.value * 2.0f - 1.0f;
				x *= shakeMagnitude * damper;
				y *= shakeMagnitude * damper;

				Camera.main.transform.position = new Vector3 ((x + originalCamPos.x), (y + originalCamPos.y), originalCamPos.z);

				yield return null;
			}

			Camera.main.transform.position = originalCamPos;
		
	}

	IEnumerator Lunge (){
		animator.SetBool ("IsSwiping", true);

		if (enemySight.playerOnRight == true) {
			rb.AddForce (transform.right * (lungeForce *= 0.8f), ForceMode.VelocityChange);
		} else if (enemySight.playerOnRight == false) {
			rb.AddForce (-transform.right * (lungeForce *= 0.8f), ForceMode.VelocityChange);
		}
			
		yield return new WaitForSeconds (lungeTime);

		animator.SetBool ("IsSwiping", false);
		swiping = false;
		lungeForce = 25;
		swipeTimer = 0;
	}

	IEnumerator TookDamage()
	{
		animator.SetBool ("IsHit", true);

		yield return new WaitForSeconds (stunTime);

		animator.SetBool ("IsHit", false);
		animator.SetBool ("Teleport", false);

		tookDamage = false;

		knockBack = 20;
		
	}
}
