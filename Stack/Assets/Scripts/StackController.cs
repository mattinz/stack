using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
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
	private BoxCollider outOfBoundsCollider;

	// Use this for initialization
	void Start() {
		colorProvider = new TileColorProvider();
		gameState = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();
		outOfBoundsCollider = GetComponent<BoxCollider>();

		resetStack();
	}

	// Update is called once per frame
	void Update() {
		GameState.State state = gameState.getGameState();
		if (state == GameState.State.GAME_RUNNING) {
			if (currentTile == null) {
				float stackHeight = getStackHeight();
				movementDirection = getMovementDirection();
				currentTile = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
				currentTile.SetParent(transform);
				currentTile.localPosition = movementDirection * movementBounds + new Vector3(previousTile.localPosition.x, stackHeight, previousTile.localPosition.z);
				currentTile.localScale = previousTile.transform.localScale;
				currentTile.GetComponent<MeshRenderer>().material.color = colorProvider.getNextColor();

				Rigidbody rigidBody = currentTile.gameObject.AddComponent<Rigidbody>();
				rigidBody.isKinematic = true;

				outOfBoundsCollider.center = movementDirection * -1 * movementBounds + new Vector3(0, stackHeight, 0);
			} else {
				handleInput();
				moveCurrentTile();
			}
		} else if (state == GameState.State.GAME_STARTING) {
			resetStack();
			gameState.resetScore();
			gameState.setGameState(GameState.State.GAME_RUNNING);
		}
	}

	private void OnTriggerEnter(Collider other) {
		Debug.Log("Collision");
		if (other.transform == currentTile) {
			gameState.setGameState(GameState.State.GAME_OVER);
		}
	}

	private Vector3 getMovementDirection() {
		Vector3 movementDirection = new Vector3();
		switch (stackSize % 4) {
			case 0:
				movementDirection.x = -1;
				break;
			case 1:
				movementDirection.z = -1;
				break;
			case 2:
				movementDirection.x = 1;
				break;
			case 3:
				movementDirection.z = 1;
				break;
		}

		return movementDirection;
	}

	public float getStackHeight() {
		return tileHeight * (stackSize + 1);
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

			Camera.main.transform.Translate(Vector3.up * tileHeight, Space.World);

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
		if (currentTile != null) {
			currentTile.Translate(movementDirection * movementSpeed * -1.0f * Time.deltaTime);
		}
	}

	private void resetStack() {
		foreach (Transform child in transform) {
			Destroy(child.gameObject);
		}

		currentTile = null;
		stackSize = 0;

		previousTile = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
		previousTile.transform.SetParent(transform);
		previousTile.transform.localPosition = Vector3.zero;
		previousTile.transform.localScale = new Vector3(baseTileSize, tileHeight, baseTileSize);
		previousTile.GetComponent<MeshRenderer>().material.color = colorProvider.getCurrentColor();
	}
}
