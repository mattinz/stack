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

	private void Awake() {
		currentState = State.GAME_NOT_STARTED;
		reset();
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
