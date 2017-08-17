using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

	public Transform cameraHolder;
	public Transform playerTransform;
	public Transform enemyTransform;

	//public List<Transform> characters = new List<Transform>();

	Vector3 middlePoint;
	Vector3 playerXPos;
	Vector3 cameraBuffer;

	float targetZ;

	public float zMin;
	public float zMax;
	public float cameraSpeed;
	public bool isFollowing;
	public bool cameraRisen;

	public Camera cam;
	public GameObject playerTarget;

	private PlayerController playerController;
	private GameManager gameManager;

	void Start () {

		cam = Camera.main;
		cameraHolder = cam.transform.parent;

		playerTarget = GameObject.FindGameObjectWithTag ("Player");
		playerController = FindObjectOfType <PlayerController> ();
		gameManager = FindObjectOfType<GameManager> ();

	}
	
	void FixedUpdate () {

		playerXPos = new Vector3 (playerTarget.transform.position.x, 0f, playerTarget.transform.position.z);

		cameraBuffer = new Vector3 (0f, 0f, 10f);

		if (isFollowing == true && playerController.isDead == false && cameraRisen == false && gameManager.gameOver == false && gameManager.gameWon == false) {

			enemyTransform = null;

			cam.transform.localPosition = new Vector3 (0, 2, -25);

			cameraHolder.transform.position = Vector3.Lerp (cameraHolder.transform.position, playerXPos + cameraBuffer, cameraSpeed);
			
		}

		if (isFollowing == false && cameraRisen == false && gameManager.gameOver == false && gameManager.gameWon == false) {

			playerTransform = GameObject.FindGameObjectWithTag ("Player").GetComponent<Transform> ();
			enemyTransform = GameObject.FindGameObjectWithTag ("Enemy").GetComponent<Transform> ();

			float distance = Vector3.Distance (playerTransform.position, enemyTransform.position);
			float half = (distance / 2);

			middlePoint = (enemyTransform.position - playerTransform.position).normalized * half;
			middlePoint += playerTransform.position;

			targetZ = -(2 * half);

			if (Mathf.Abs (targetZ) < Mathf.Abs (zMin)) {
				targetZ = zMin;
			}

			if (Mathf.Abs (targetZ) > Mathf.Abs (zMax)) {
				targetZ = zMax;
			}

			cam.transform.localPosition = new Vector3 (0, 2, targetZ);

			cameraHolder.transform.position = Vector3.Lerp (cameraHolder.transform.position, middlePoint, Time.deltaTime * 4);
		
		}

		if (isFollowing == true && playerController.isDead == true && cameraRisen == false
			&& gameManager.gameOver == false && gameManager.gameWon == false) {
			StartCoroutine (CameraRise ());
		} 

		if (isFollowing == true && playerController.isDead == false && cameraRisen == true 
			&& gameManager.gameOver == false && gameManager.gameWon == false) 
		{
			StopCoroutine (CameraRise ());
			StartCoroutine (CameraDown ());
		}

		if (isFollowing == false && playerController.isDead == true && cameraRisen == false 
			&& gameManager.gameOver == true && gameManager.gameWon == false) {
			StartCoroutine (CameraGORise ());
		}

		if (cameraRisen == false && gameManager.gameOver == false && gameManager.gameWon == true) {

			StartCoroutine (CameraWinRise ());
		}
	}

	public void CameraFall()
	{ 
			StartCoroutine (CameraDown ());
	}

	IEnumerator CameraRise()
	{
		Vector3 addHeight = new Vector3 (0f, 0.25f, 0f);

		cam.transform.position += addHeight;

		yield return new WaitForSeconds (3);

		cameraRisen = true;

	}

	IEnumerator CameraGORise()
	{
		yield return new WaitForSeconds (3);
		
		Vector3 addHeight = new Vector3 (0f, 0.2f, 0f);

		cam.transform.position += addHeight;

		yield return new WaitForSeconds (5);

		cameraRisen = true;
	}

	IEnumerator CameraWinRise()
	{
		yield return new WaitForSeconds (3);

		Vector3 addHeight = new Vector3 (0f, 0.2f, 0f);

		cam.transform.position += addHeight;

		yield return new WaitForSeconds (7);

		cameraRisen = true;
	}

	IEnumerator CameraDown()
	{

		Vector3 addHeight = new Vector3 (0f, 0.25f, 0.15f);

		cam.transform.position += -addHeight;

		yield return new WaitForSeconds (3);

		cameraRisen = false;

	}
}
