using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraMovementController : MonoBehaviour {

	[SerializeField] private float cameraBaseSpeed = 1.0f;
	[SerializeField] private float cameraSizeChangeRate = 1.0f;

	private GameState gameState;
	private Camera camera;
	private Vector3 startingPosition;
	private Vector3 targetCameraPosition;
	private float targetCameraSize;
	private float startingCameraSize;
	private float distanceFromStack;
	private float actualSpeed;

	// Use this for initialization
	void Start () {
		gameState = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();
		camera = GetComponent<Camera>();
		startingPosition = transform.position;
		targetCameraPosition = new Vector3(startingPosition.x, 0, startingPosition.z);
		alignCameraToStackHeight();

		startingCameraSize = camera.orthographicSize;
		targetCameraSize = startingCameraSize;
		distanceFromStack = Vector3.Distance(Vector3.zero, new Vector3(startingPosition.x, 0.0f, startingPosition.z));
		actualSpeed = cameraBaseSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		switch(gameState.getGameState()) {
			case GameState.State.GAME_OVER:
				//Roughly centers the stack in the camera's view.
				float cameraHeightOffset = distanceFromStack / Mathf.Tan((90 - transform.eulerAngles.x) * 0.0174533f); // The magic number here converts degrees to radians.
				targetCameraPosition.y = gameState.getStackHeight() / 2.0f + cameraHeightOffset;
				actualSpeed = cameraBaseSpeed * 2.0f;
				targetCameraSize = startingCameraSize * 1.5f;
				break;
			case GameState.State.GAME_STARTING_PHASE_TWO:
				alignCameraToStackHeight();
				//transform.position = targetCameraPosition;
				targetCameraSize = startingCameraSize;
				gameState.setGameState(GameState.State.GAME_RUNNING);
				break;
			default:
				alignCameraToStackHeight();
				actualSpeed = cameraBaseSpeed;
				break;
		}

		transform.position = Vector3.MoveTowards(transform.position, targetCameraPosition, actualSpeed * Time.deltaTime);
		if(camera.orthographicSize != targetCameraSize) {
			camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, targetCameraSize, cameraSizeChangeRate * Time.deltaTime);
		}
	}

	private void alignCameraToStackHeight() {
		targetCameraPosition.y = gameState.getStackHeight() + startingPosition.y;
	}
}
