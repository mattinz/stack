using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour {

	public enum State {
		GAME_NOT_STARTED,
		GAME_STARTING,
		GAME_RUNNING,
		GAME_OVER
	}

	private State currentState;
	private int score;
	private float stackHeight;

	private TileColorProvider colorProvider;

	private void Awake() {
		currentState = State.GAME_NOT_STARTED;
		reset();

		colorProvider = new TileColorProvider();
	}

	public State getGameState() {
		return currentState;
	}

	public int getScore() {
		return score;
	}

	public float getStackHeight() {
		return stackHeight;
	}

	public TileColorProvider getTileColorProvider() {
		return colorProvider;
	}

	public bool isInputDown() {
		return Input.GetButtonDown("PlaceTile") || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began);
	}

	public void incrementScore() {
		score++;
	}

	public void reset() {
		score = 0;
		stackHeight = 0;
	}

	public void setGameState(State state) {
		currentState = state;
	}

	public void setStackHeight(float height) {
		stackHeight = height;
	}
}
