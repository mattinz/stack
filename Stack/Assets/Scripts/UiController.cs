using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour {
	[SerializeField]
	private string highScoreListTitle;
	[SerializeField]
	private Text scoreText;
	[SerializeField]
	private Text highScoreListText;
	[SerializeField]
	private Text startPromptText;

	private GameState gameState;

	// Use this for initialization
	void Start() {
		gameState = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();
	}

	// Update is called once per frame
	void Update() {
		GameState.State state = gameState.getGameState();
		scoreText.text = gameState.getScore().ToString();
		switch (state) {
			case GameState.State.GAME_NOT_STARTED:
				scoreText.enabled = false;
				highScoreListText.enabled = false;
				startPromptText.enabled = true;
				checkForInput();
				break;
			case GameState.State.GAME_STARTING_PHASE_ONE:
			case GameState.State.GAME_STARTING_PHASE_TWO:
			case GameState.State.GAME_RUNNING:
				scoreText.enabled = true;
				highScoreListText.enabled = false;
				startPromptText.enabled = false;
				break;
			case GameState.State.GAME_OVER:
				scoreText.enabled = true;
				highScoreListText.text = getHighScoreListString(gameState.getHighScores(), highScoreListTitle);
				highScoreListText.enabled = true;
				startPromptText.enabled = true;
				checkForInput();
				break;
		}
	}

	private void checkForInput() {
		if (gameState.isInputDown()) {
			gameState.setGameState(GameState.State.GAME_STARTING_PHASE_ONE);
		}
	}

	private string getHighScoreListString(int[] highScores, string title) {
		string result = title + "\n";

		for(int i = 0; i < highScores.Length; i++) {
			result += i + 1 + ": " + highScores[i] + "\n";
		}

		return result;
	}
}
