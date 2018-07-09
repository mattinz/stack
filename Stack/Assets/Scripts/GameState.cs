using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour {

	private const int NUM_HIGH_SCORES = 5;
	private const string HIGH_SCORES_KEY = "highScores";

	public enum State {
		GAME_NOT_STARTED,
		GAME_STARTING_PHASE_ONE,
		GAME_STARTING_PHASE_TWO,
		GAME_RUNNING,
		GAME_OVER
	}

	private State currentState;
	private int score;
	private int[] highScores;
	private float stackHeight;

	private TileColorProvider colorProvider;

	private void Awake() {
		//PlayerPrefs.DeleteKey(HIGH_SCORES_KEY);
		currentState = State.GAME_NOT_STARTED;
		resetScore();
		highScores = readHighScoresFromPrefs();

		colorProvider = new TileColorProvider();
	}

	public State getGameState() {
		return currentState;
	}

	public int[] getHighScores() {
		foreach(int x in highScores) {
			Debug.Log(x);
		}

		return highScores;
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

	public void resetScore() {
		score = 0;
		stackHeight = 0;
	}

	public void setGameState(State state) {
		currentState = state;

		if(state == State.GAME_OVER) {
			foreach(int x in highScores) {
				if(x < score) {
					highScores[highScores.Length - 1] = score;
					Array.Sort(highScores);
					Array.Reverse(highScores);
					writeHighScoresToPrefs(highScores);
					break;
				}
			}
		}
	}

	public void setStackHeight(float height) {
		stackHeight = height;
	}

	private int[] readHighScoresFromPrefs() {
		string highScoresString = PlayerPrefs.GetString(HIGH_SCORES_KEY, "");
		int[] scores = new int[NUM_HIGH_SCORES];
		if (highScoresString.Length != 0) {
			string[] scoreStrings = highScoresString.Split(' ');
			for (int x = 0; x < scoreStrings.Length && x < NUM_HIGH_SCORES; x++) {
				scores[x] = int.Parse(scoreStrings[x]);
			}
		}
		return scores;
	}

	private void writeHighScoresToPrefs(int[] scores) {
		string result = "";
		foreach(int score in scores) {
			result += score + " ";
		}
		result = result.Trim();
		PlayerPrefs.SetString(HIGH_SCORES_KEY, result);
	}
}
