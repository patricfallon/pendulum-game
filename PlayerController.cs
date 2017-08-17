using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float walkMovementSpeed;
	public float attackMovementSpeed;
	public float knockedDownTime;
	public float knockBack;
	public float dodgeMove;
	public float punchThrust;
	public float kickThrust;
	public float xMin, xMax, zMin, zMax;
	public float stunTime;
	public bool tookDamage;
	public bool knockedDown;
	public bool facingRight;
	public bool isDead = false;
	public bool canMove = true;

	public GameObject noWeaponPunchBox, noWeaponKickBox;
	public Sprite noWeaponPunchHF, noWeaponKickHF;
	public GameObject enemy;
	public Camera cam;
	public AudioClip punch;
	public AudioClip kick;
	public AudioClip dodge;
	public AudioClip[] hitSounds;
	public int hitClips;
	public AudioClip[] deathSounds;
	public int deathClips;

	private float movementSpeed;
	private float timeSlowDown = 0.5f;
	private float deathTime = 3;

	private Transform enemyTransform;
	private SpriteRenderer currentSprite;
	private Rigidbody rb;
	private Animator animator;
	private	AnimatorStateInfo currentStateInfo;
	private PlayerStats playerStats;
	private ParticleSystem blood;
	private ParticleSystem attackSplash;
	private LevelingUI levelingUI;
	private AudioSource playerAudio;
	private PedestalInteractions pedestal;
	private GameManager gameManager;

	static int currentState;
	static int noWeaponIdleState = Animator.StringToHash ("NoWeapon_Idle");
	static int noWeaponRunState = Animator.StringToHash ("NoWeapon_Run");
	static int noWeaponPunchState = Animator.StringToHash ("NoWeapon_Punch");
	static int noWeaponKickState = Animator.StringToHash ("NoWeapon_Kick");
	static int noWeaponDodgeState = Animator.StringToHash ("NoWeapon_Dodge");
	static int noWeaponHitState = Animator.StringToHash ("NoWeapon_Hit");
	static int noWeaponFallState = Animator.StringToHash ("NoWeapon_Fall");
	static int noWeaponDeathState = Animator.StringToHash ("NoWeapon_Death");

	void Awake () {

		currentSprite = GetComponent<SpriteRenderer> ();
		rb = GetComponent<Rigidbody> ();
		animator = GetComponent<Animator> ();
		blood = GetComponentInChildren<ParticleSystem> ();
		attackSplash = GameObject.FindGameObjectWithTag("AttackSplash").GetComponent<ParticleSystem> ();
		enemyTransform = enemy.GetComponent<Transform> ();
		cam = Camera.main;
		playerStats = GetComponent<PlayerStats> ();
		levelingUI = FindObjectOfType<LevelingUI> ();
		playerAudio = GetComponent<AudioSource> ();
		pedestal = FindObjectOfType<PedestalInteractions> ();
		gameManager = FindObjectOfType<GameManager> ();

	}
	
	void Update () {

		walkMovementSpeed = playerStats.playerSpeed;

		movementSpeed = walkMovementSpeed;

		currentStateInfo = animator.GetCurrentAnimatorStateInfo (0);
		currentState = currentStateInfo.fullPathHash;

		if (currentState == noWeaponIdleState)
			Debug.Log ("No Weapon Idle State");

		if (currentState == noWeaponRunState)
			Debug.Log ("No Weapon Run State");

		if (currentState == noWeaponPunchState)
			Debug.Log ("No Weapon Punch State");

		if (currentState == noWeaponKickState)
			Debug.Log ("No Weapon Kick State");

		if (currentState == noWeaponDodgeState)
			Debug.Log ("No Weapon Dodge State");

		if (currentState == noWeaponHitState)
			Debug.Log ("No Weapon Hit State");

		if (currentState == noWeaponFallState)
			Debug.Log ("No Weapon Fall State");

		if (currentState == noWeaponDeathState)
			Debug.Log ("No Weapon Death State");
		
	}

	void FixedUpdate(){

		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0f, moveVertical);

		if (tookDamage == false && knockedDown == false && isDead == false) {
			canMove = true;
		}

		if (canMove == true) {
			rb.velocity = movement * movementSpeed;
		} else if (canMove == false) {
			rb.velocity = movement * attackMovementSpeed;
		}

		rb.position = new Vector3 (Mathf.Clamp(rb.position.x, xMin, xMax), 
									transform.position.y, 
										Mathf.Clamp(rb.position.z, zMin,zMax));

		if (canMove == true && moveHorizontal < 0 && !facingRight) {
			Flip ();
		} else if (canMove == true && moveHorizontal > 0 && facingRight) {
			Flip ();
		}

		animator.SetFloat ("Speed", rb.velocity.sqrMagnitude);

		//// No Weapon Punch--------------------------------

		if (Input.GetKeyDown (KeyCode.J)) {

			if (tookDamage == false && knockedDown == false && isDead == false){
				
				animator.SetBool ("AttackOne", true);
				attackSplash.Play (true);
				playerAudio.PlayOneShot (punch);

				if (facingRight == true && isDead == false) {
					rb.AddForce (-transform.right * (punchThrust * movementSpeed), ForceMode.VelocityChange);
				}
				else if (!facingRight == true && isDead == false){
					rb.AddForce (transform.right * (punchThrust * movementSpeed), ForceMode.VelocityChange);
				}
			}
		} else {
			animator.SetBool ("AttackOne", false);
		}

		if (currentSprite.sprite == noWeaponPunchHF) {
			noWeaponPunchBox.gameObject.SetActive (true);
		} else {
			noWeaponPunchBox.gameObject.SetActive (false);
		}

		////No Weapon Kick----------------------------------

		if (Input.GetKeyDown (KeyCode.K)) {

			if (tookDamage == false && knockedDown == false && isDead == false) {
				
				animator.SetBool ("AttackTwo", true);
				attackSplash.Play (true);
				playerAudio.PlayOneShot (kick);

				if (facingRight == true && isDead == false) {
					rb.AddForce (-transform.right * (kickThrust * movementSpeed), ForceMode.VelocityChange);
				}
				else if (!facingRight == true && isDead == false){
					rb.AddForce (transform.right * (kickThrust * movementSpeed), ForceMode.VelocityChange);
				}
			}
		} else {
			animator.SetBool ("AttackTwo", false);
		}

		if (currentSprite.sprite == noWeaponKickHF) {
			noWeaponKickBox.gameObject.SetActive (true);
		} else {
			noWeaponKickBox.gameObject.SetActive (false);
		}
			
		////Dodge-------------------------------------------
	
		if (Input.GetKeyDown (KeyCode.L)) {
			if (tookDamage == false && knockedDown == false && isDead == false) {
				animator.SetBool ("Dodge", true);
				playerAudio.PlayOneShot (dodge, 0.8f);
				Dodge ();
			}
		} else {
			animator.SetBool ("Dodge", false);
		}

		////Jump--------------------------------------------


		////Hit---------------------------------------------

		if (tookDamage == true && isDead == false) {

			if (blood != null) {
				blood.Emit (60);
			}

			if (playerStats.health > 0) {
				int randomHit = Random.Range (0, hitClips);
				PlayRandomHit (randomHit);
			} else if (playerStats.health <= 0) {
				int randomDeath = Random.Range (0, deathClips);
				PlayRandomDeath (randomDeath);
			}

			StartCoroutine (TookDamage ());
			StartCoroutine (CameraTilt ());
		} 

		////Fall--------------------------------------------
		if (knockedDown == true && isDead == false) {

			if (facingRight == true && transform.position.x < enemyTransform.position.x + 0.5f) {
				rb.AddForce (-transform.right * (knockBack *= 0.7f), ForceMode.VelocityChange);
			} else if (facingRight == false && transform.position.x < enemyTransform.position.x + 0.5f) {
				rb.AddForce (-transform.right * (knockBack *= 0.7f), ForceMode.VelocityChange);
			} else if (facingRight == false && transform.position.x >= enemyTransform.position.x) {
				rb.AddForce (transform.right * (knockBack *= 0.6f), ForceMode.VelocityChange);
			} else if (facingRight == true && transform.position.x >= enemyTransform.position.x) {
				rb.AddForce (transform.right * (knockBack *= 0.6f), ForceMode.VelocityChange);
			} 

			StartCoroutine (KnockedDown ());

		}

	}

	void PlayRandomHit(int clip)
	{
		playerAudio.clip = hitSounds [clip];
		playerAudio.PlayOneShot (playerAudio.clip);
	}

	void PlayRandomDeath(int clip)
	{
		playerAudio.clip = deathSounds [clip];
		playerAudio.PlayOneShot (playerAudio.clip);
	}

	void Dodge(){
		if (facingRight == true && isDead == false) {
			rb.AddForce (transform.right * (dodgeMove * movementSpeed), ForceMode.VelocityChange);
		}
		else if (!facingRight == true && isDead == false){
			rb.AddForce (-transform.right * (dodgeMove * movementSpeed), ForceMode.VelocityChange);
		}
	}

	void Flip()
	{
		facingRight = !facingRight;
		Vector3 thisScale = transform.localScale;
		thisScale.x *= -1;
		transform.localScale = thisScale;
	}

	void CheckIfDead(){

		if (playerStats.health <= 0) {

			StartCoroutine (Death());
			StopCoroutine (KnockedDown ());
			StopCoroutine (TookDamage ());
			gameManager.CheckIfGameOver ();

		} else if (playerStats.health > 0)
			return;

	}

	IEnumerator Death()
	{
		isDead = true;
		canMove = false;

		Time.timeScale = timeSlowDown;

		animator.SetBool ("KnockedDown", false);
		animator.SetBool ("IsHit", false);
		animator.SetBool ("IsDead", true);

		GameObject enemy = GameObject.FindGameObjectWithTag ("Enemy");

		yield return new WaitForSeconds (deathTime);

		if (gameManager.gameOver == false && gameManager.gameWon == false) {
			levelingUI.OpenLevelMenu ();
			DestroyObject (enemy.gameObject);
			pedestal.RestartEncounter ();
		} 
	}

	IEnumerator TookDamage()
	{
		canMove = false;

		animator.SetBool ("IsHit", true);

		yield return new WaitForSeconds (stunTime);

		tookDamage = false;

		animator.SetBool ("IsHit", false);

	}

	IEnumerator KnockedDown()
	{
		canMove = false;

		animator.Play ("NoWeapon_Fall");
		animator.SetBool ("KnockedDown", true);

		yield return new WaitForSeconds (knockedDownTime);

		tookDamage = false;
		knockedDown = false;
		knockBack = 200;

		animator.SetBool ("KnockedDown", false);

	}

	IEnumerator CameraTilt()
	{

			if (facingRight == true) {
				Vector3 addRotation = new Vector3 (0f, 1f, 0.5f);

			cam.transform.eulerAngles += addRotation * 1.5f;

				yield return new WaitForSeconds (0.5f);

			cam.transform.eulerAngles -= addRotation * 1.5f;

				yield return new WaitForSeconds (0.5f);

			} else if (facingRight == false) {
				Vector3 addRotation = new Vector3 (0f, 1f, 0.5f);

			cam.transform.eulerAngles -= addRotation * 1.5f;

				yield return new WaitForSeconds (0.5f);

			cam.transform.eulerAngles += addRotation * 1.5f;

				yield return new WaitForSeconds (0.5f);
			}
	}
}
