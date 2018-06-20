using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementController : MonoBehaviour {

	private GameState gameState;
	private Vector3 startingPosition;

	// Use this for initialization
	void Start () {
		gameState = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();
		startingPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = startingPosition + new Vector3(0, gameState.getStackHeight(), 0);
	}
}
