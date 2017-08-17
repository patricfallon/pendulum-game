using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelingUI : MonoBehaviour {

	public Image resilienceBar;
	public Image tenacityBar;
	public Image purposeBar;
	public Image shadowBar;
	public Text spiritText;

	public float resilience;
	public float maxResilience = 5;
	public float tenacity;
	public float maxTenacity = 5;
	public float purpose;
	public float maxPurpose = 5;
	public float shadow;
	public float maxShadow = 5;

	public float health;
	public float attack;
	public float speed;
	public float doubt;

	private float shadowIncrease = 1;
	private float doubtIncrease = -1f;
	private bool counterUp;

	public GameObject levelingMenu;
	public Animator animator;
	public AudioClip levelUp;
	public AudioClip doubtDown;
	public AudioClip returnClick;

	private PlayerStats playerStats;
	private PlayerController playerController;
	private EnemyStats enemyStats;
	private PlayMusic playMusic;
	private AudioSource levelMenuAudio;
	private CameraManager camManager;

	private int spiritPoints;

	void Awake () {

		PlayerBaseLevel playerBaseLevel = new PlayerBaseLevel ();

	 	playerStats = FindObjectOfType<PlayerStats> ();
		playerController = FindObjectOfType<PlayerController> ();
		playMusic = FindObjectOfType<PlayMusic> ();
		levelMenuAudio = GetComponent<AudioSource> ();
		camManager = FindObjectOfType<CameraManager> ();

		playerBaseLevel.Health = 1f;
		playerBaseLevel.Attack = 6;
		playerBaseLevel.Speed = 6.25f;
		playerBaseLevel.Doubt = 10f;

		health = playerBaseLevel.Health;
		attack = playerBaseLevel.Attack;
		speed = playerBaseLevel.Speed;
		doubt = playerBaseLevel.Doubt;

		playerBaseLevel.Resilience = 0f;
		playerBaseLevel.Tenacity = 0f;
		playerBaseLevel.Purpose = 0f;
		playerBaseLevel.Shadow = 0f;

		resilience = playerBaseLevel.Resilience;
		tenacity = playerBaseLevel.Tenacity;
		purpose = playerBaseLevel.Purpose;
		shadow = playerBaseLevel.Shadow;

	}
	
	void UpdateUI(){

		resilienceBar.fillAmount = resilience / maxResilience;
		tenacityBar.fillAmount = tenacity / maxTenacity;
		purposeBar.fillAmount = purpose / maxPurpose;
		shadowBar.fillAmount = shadow / maxShadow;

		spiritText.text = spiritPoints.ToString();

	}

	public void OpenLevelMenu(){

		enemyStats = FindObjectOfType<EnemyStats> ();

		levelingMenu.SetActive (true);

		if (enemyStats.health > 250 && counterUp == false) 
		{
			spiritPoints = 1;
			shadow += shadowIncrease;
			doubt += doubtIncrease;
			counterUp = true;
		} else if (enemyStats.health <= 250 && enemyStats.health > 125 && counterUp == false) 
		{
			spiritPoints = 2;
			shadow += shadowIncrease;
			doubt += doubtIncrease;
			counterUp = true;
		} else if (enemyStats.health <= 125 && enemyStats.health > 25 && counterUp == false) 
		{
			spiritPoints = 3;
			shadow += shadowIncrease;
			doubt += doubtIncrease;
			counterUp = true;
		} else if (enemyStats.health <= 25 && enemyStats.health > 0 && counterUp == false) 
		{
			spiritPoints = 4;
			shadow += shadowIncrease;
			doubt += doubtIncrease;
			counterUp = true;
		} else if (enemyStats.health < 24 && enemyStats.health >= 1 && counterUp == false) 
		{
			spiritPoints = 4;
			shadow += shadowIncrease;
			doubt += doubtIncrease;
			counterUp = true;
		}

		UpdateUI ();

		playMusic.FadeUp (0.05f);
		playMusic.PlaySelectedMusic (3);

	}

	public void SaveLevelUp()
	{
		if (camManager.cameraRisen == false) {

			levelMenuAudio.PlayOneShot (returnClick, 0.5f);

		} else if (camManager.cameraRisen == true) {

			OnClickReturn ();
		}

	}

	void OnClickReturn()
	{

		levelingMenu.SetActive (false);
		playerStats.LevelsUpdate ();

		animator.Play ("NoWeapon_Idle");
		animator.SetBool ("IsDead", false);
		playerController.isDead = false;

		counterUp = false;

		playMusic.FadeUp (0.05f);
		playMusic.PlaySelectedMusic (1);

		Time.timeScale = 1;
	}

	public void SetResilience(int amount)
	{
		if (spiritPoints >= 1 && resilience < maxResilience) {
			resilience += amount;
			health += 25;
			attack += 3;

			spiritPoints -= 1;

			levelMenuAudio.PlayOneShot (levelUp, 0.8f);
			UpdateUI ();
		} else if (spiritPoints < 1 || resilience == maxResilience) {
			levelMenuAudio.PlayOneShot (returnClick, 0.5f);
		}
			
	}

	public void SetTenacity(int amount)
	{
		if (spiritPoints >= 1 && tenacity < maxTenacity) {
			tenacity += amount;
			attack += 8;
			speed += 0.5f;

			spiritPoints -= 1;

			levelMenuAudio.PlayOneShot (levelUp, 0.8f);
			UpdateUI ();
		} else if (spiritPoints < 1 || tenacity == maxTenacity) {
			levelMenuAudio.PlayOneShot (returnClick, 0.5f);
		}
	}

	public void SetPurpose(int amount)
	{
		if (spiritPoints >= 1 && purpose < maxPurpose) {
			purpose += amount;
			speed += 1;
			health += 10;

			spiritPoints -= 1;

			levelMenuAudio.PlayOneShot (levelUp, 0.8f);
			UpdateUI ();
		} else if (spiritPoints < 1 || purpose == maxPurpose) {
			levelMenuAudio.PlayOneShot (returnClick, 0.5f);
		}
	}

	public void SetShadow(int amount)
	{
		if (spiritPoints >= 1) {
			shadow += amount;
			doubt += 1;

			spiritPoints -= 1;

			levelMenuAudio.PlayOneShot (doubtDown, 0.8f);
			UpdateUI ();
		} else if (spiritPoints < 1) {
			levelMenuAudio.PlayOneShot (returnClick, 0.5f);
		}
	}
}
