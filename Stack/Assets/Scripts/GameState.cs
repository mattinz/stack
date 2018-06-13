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

	private void Awake() {
		currentState = State.GAME_NOT_STARTED;
		score = 0;
	}

	public State getGameState() {
		return currentState;
	}

	public int getScore() {
		return score;
	}

	public void incrementScore() {
		score++;
	}

	public void resetScore() {
		score = 0;
	}

	public void setGameState(State state) {
		currentState = state;
	}
}
