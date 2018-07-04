using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraMovementController : MonoBehaviour {

	[SerializeField] private float cameraSpeed = 1.0f;

	private GameState gameState;
	private Vector3 startingPosition;
	private Vector3 targetCameraPosition;
	private float startingCameraSize;
	private float distanceFromStack;

	// Use this for initialization
	void Start () {
		gameState = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();
		startingPosition = transform.position;
		targetCameraPosition = new Vector3(startingPosition.x, 0, startingPosition.z);
		alignCameraToStackHeight();

		startingCameraSize = GetComponent<Camera>().orthographicSize;
		distanceFromStack = Vector3.Distance(Vector3.zero, new Vector3(startingPosition.x, 0.0f, startingPosition.z));
	}
	
	// Update is called once per frame
	void Update () {
		switch(gameState.getGameState()) {
			case GameState.State.GAME_OVER:
				//Roughly centers the stack in the camera's view.
				float cameraHeightOffset = distanceFromStack / Mathf.Tan((90 - transform.eulerAngles.x) * 0.0174533f); // The magic number here converts degrees to radians.
				targetCameraPosition.y = gameState.getStackHeight() / 2.0f + cameraHeightOffset;
				break;
			case GameState.State.GAME_STARTING_PHASE_TWO:
				//Move camera in
				gameState.setGameState(GameState.State.GAME_RUNNING);
				break;
			default:
				alignCameraToStackHeight();
				break;
		}

		transform.position = Vector3.MoveTowards(transform.position, targetCameraPosition, cameraSpeed * Time.deltaTime);
	}

	private void alignCameraToStackHeight() {
		targetCameraPosition.y = gameState.getStackHeight() + startingPosition.y;
	}
}
