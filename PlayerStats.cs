using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class PlayerStats : MonoBehaviour {

	public float health, maxHealth;
	public float playerAttack;
	public float playerSpeed;
	public float playerDoubt;

	private Image healthBar;
	private LevelingUI playerLevels;

	void Start () {

		playerLevels = FindObjectOfType<LevelingUI> ();

		playerDoubt = playerLevels.doubt * 0.1f;

		maxHealth = playerLevels.health * playerDoubt;
		health = maxHealth;

		playerAttack = playerLevels.attack * playerDoubt;

		playerSpeed = playerLevels.speed * playerDoubt;

		healthBar = transform.GetComponentInChildren<Image> ();

	}
	
	void Update () {

		healthBar.fillAmount = health / maxHealth;
		
	}

	public void LevelsUpdate()
	{
		playerDoubt = playerLevels.doubt * 0.1f;

		maxHealth = playerLevels.health * playerDoubt;
		health = maxHealth;

		playerAttack = playerLevels.attack * playerDoubt;

		playerSpeed = playerLevels.speed * playerDoubt;
	}
		
}
