using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackController : MonoBehaviour {

	[SerializeField]
	private float baseTileSize = 1.0f;
	[SerializeField]
	private float tileHeight = 0.1f;
	[SerializeField]
	private float movementBounds = 3.0f;
	[SerializeField]
	private float movementSpeed = 1.0f;

	private Transform currentTile;
	private Transform previousTile;
	private int stackSize;
	private Vector3 movementDirection;
	private TileColorProvider colorProvider;

	private GameState gameState;

	// Use this for initialization
	void Start() {
		colorProvider = new TileColorProvider();
		gameState = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();

		resetStack();
	}

	// Update is called once per frame
	void Update() {
		GameState.State state = gameState.getGameState();
		if (state == GameState.State.GAME_RUNNING) {
			if (currentTile == null) {
				movementDirection = getMovementDirection();
				currentTile = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
				currentTile.SetParent(transform);
				currentTile.localPosition = movementDirection * movementBounds + new Vector3(previousTile.localPosition.x, tileHeight * (stackSize + 1), previousTile.localPosition.z);
				currentTile.localScale = previousTile.transform.localScale;
				currentTile.GetComponent<MeshRenderer>().material.color = colorProvider.getNextColor();
			} else {
				moveCurrentTile();
				handleInput();
			}
		} else if (state == GameState.State.GAME_STARTING) {
			resetStack();
			gameState.resetScore();
			gameState.setGameState(GameState.State.GAME_RUNNING);
		}
	}

	private Vector3 getMovementDirection() {
		Vector3 movementDirection = new Vector3();
		switch (stackSize % 2) {
			case 0:
				movementDirection.x = -1;
				break;
			case 1:
				movementDirection.z = 1;
				break;
				/*case 2:
                movementDirection.x = 1;
                break;
            case 3:
                movementDirection.z = 1;
                break;*/
		}

		return movementDirection;
	}

	public Rect getTileIntersection() {
		Rect r1 = new Rect(currentTile.localPosition.x - currentTile.localScale.x / 2, currentTile.localPosition.z - currentTile.localScale.z / 2, currentTile.localScale.x, currentTile.localScale.z);
		Rect r2 = new Rect(previousTile.localPosition.x - previousTile.localScale.x / 2, previousTile.localPosition.z - previousTile.localScale.z / 2, previousTile.localScale.x, previousTile.localScale.z);

		Rect area = new Rect();
		float x1 = Mathf.Min(r1.xMax, r2.xMax);
		float x2 = Mathf.Max(r1.xMin, r2.xMin);
		float y1 = Mathf.Min(r1.yMax, r2.yMax);
		float y2 = Mathf.Max(r1.yMin, r2.yMin);
		area.x = Mathf.Min(x1, x2);
		area.y = Mathf.Min(y1, y2);
		area.width = Mathf.Max(0.0f, x1 - x2);
		area.height = Mathf.Max(0.0f, y1 - y2);
		return area;
	}

	private void handleInput() {
		if (Input.GetButtonDown("PlaceTile") && isCurrentTileOverStack()) {
			Rect intersection = getTileIntersection();
			currentTile.localScale = new Vector3(intersection.width, tileHeight, intersection.height);
			currentTile.localPosition = new Vector3(intersection.x + intersection.width / 2, currentTile.localPosition.y, intersection.y + intersection.height / 2);

			Camera.main.transform.Translate(Vector3.up * tileHeight);

			previousTile = currentTile;
			currentTile = null;
			stackSize++;
			gameState.incrementScore();
		}
	}

	private bool isCurrentTileOverStack() {
		Rect currentTileBounds = new Rect(currentTile.localPosition.x - currentTile.localScale.x / 2, currentTile.localPosition.z - currentTile.localScale.z / 2, currentTile.localScale.x, currentTile.localScale.z);
		Rect previousTileBounds = new Rect(previousTile.localPosition.x - previousTile.localScale.x/2, previousTile.localPosition.z - previousTile.localScale.z / 2, previousTile.localScale.x, previousTile.localScale.z);
		return currentTileBounds.Overlaps(previousTileBounds);
	}

	private void moveCurrentTile() {
		currentTile.Translate(movementDirection * movementSpeed * -1.0f * Time.deltaTime);
	}

	private void resetStack() {
		currentTile = null;
		stackSize = 0;

		previousTile = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
		previousTile.transform.SetParent(transform);
		previousTile.transform.localPosition = Vector3.zero;
		previousTile.transform.localScale = new Vector3(baseTileSize, tileHeight, baseTileSize);
		previousTile.GetComponent<MeshRenderer>().material.color = colorProvider.getCurrentColor();
	}
}
