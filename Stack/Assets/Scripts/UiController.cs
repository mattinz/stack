using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour {

	[SerializeField] private Text scoreText;
	[SerializeField] private Text gameOverText;
	[SerializeField] private Text startPromptText;

	private GameState gameState;

	// Use this for initialization
	void Start () {
		gameState = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();
	}
	
	// Update is called once per frame
	void Update () {
		GameState.State state = gameState.getGameState();
		scoreText.text = gameState.getScore().ToString();
		switch(state) {
			case GameState.State.GAME_NOT_STARTED:
				scoreText.enabled = false;
				gameOverText.enabled = false;
				startPromptText.enabled = true;
				checkForInput();
				break;
			case GameState.State.GAME_RUNNING:
				scoreText.enabled = true;
				gameOverText.enabled = false;
				startPromptText.enabled = false;
				break;
			case GameState.State.GAME_OVER:
				scoreText.enabled = true;
				gameOverText.enabled = true;
				startPromptText.enabled = true;
				checkForInput();
				break;
		}
	}

	private void checkForInput() {
		if(Input.GetButtonDown("PlaceTile")) {
			gameState.setGameState(GameState.State.GAME_RUNNING);
		}
	}
}
