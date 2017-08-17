using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;

	public GameObject enemy;
	public CameraManager camManager;

	public GameObject gameOverScreen;
	public GameObject playerWinScreen;
	public GameObject tieScreen;

	public bool gameOver;
	public bool gameWon;

	private LevelingUI playerLevels;
	private StartOptions startOptions;
	private Achievements achievements;

	void Awake () {

		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}

		camManager = GameObject.FindGameObjectWithTag ("LevelManager").GetComponent<CameraManager> ();
		playerLevels = FindObjectOfType<LevelingUI> ();
		startOptions = FindObjectOfType<StartOptions> ();
		achievements = playerWinScreen.GetComponent<Achievements> ();

	}
	
	void Update () {

		enemy = GameObject.FindGameObjectWithTag ("Enemy");

		if (enemy == null) {
			camManager.isFollowing = true;
		} else if (enemy != null){
			camManager.isFollowing = false;
		}
		
	}

	public void CheckIfGameOver(){

		EnemyStats enemyStats = enemy.GetComponent<EnemyStats> ();

		if (playerLevels.shadow == playerLevels.maxShadow && gameWon == false) 
		{
			startOptions.GameOver ();
			gameOver = true;
			StartCoroutine (GameOverScreen ());
		} 

		if (playerLevels.shadow == playerLevels.maxShadow && enemyStats.health <= 0 && gameWon == true) 
		{
			startOptions.GameOver ();
			gameOver = true;
			StartCoroutine (TieScreen ());
		}
			
	}

	public void PlayerWins()
	{
		startOptions.WinGame ();
		gameWon = true;
		StartCoroutine (WinScreen ());
		
	}

	IEnumerator GameOverScreen()
	{
		Time.timeScale = 0.5f;

		yield return new WaitForSeconds (5);

		gameOverScreen.SetActive(enabled);

		yield return new WaitForSeconds (8);

		startOptions.LoadMenu ();

		Time.timeScale = 1;

	}

	IEnumerator WinScreen()
	{
		Time.timeScale = 0.5f;

		yield return new WaitForSeconds (8);

		playerWinScreen.SetActive(enabled);

		yield return new WaitForSeconds (1);

		achievements.CheckForAchievements ();

		Time.timeScale = 1;
	}

	IEnumerator TieScreen()
	{
		Time.timeScale = 0.5f;

		yield return new WaitForSeconds (5);

		tieScreen.SetActive(enabled);

		yield return new WaitForSeconds (8);

		startOptions.LoadMenu ();

		Time.timeScale = 1;

	}
		
}
