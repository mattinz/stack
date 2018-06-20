using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraBackgroundColorController : MonoBehaviour {

	[SerializeField]
	private float rate = 3.0f;

	private Camera cam;
	private GameState gameState;

	// Use this for initialization
	void Start () {
		cam = GetComponent<Camera>();
		cam.clearFlags = CameraClearFlags.SolidColor;
		gameState = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();
	}
	
	// Update is called once per frame
	void Update () {
		TileColorProvider colorProvider = gameState.getTileColorProvider();
		Color fromColor = setColorLook(colorProvider.getPreviousColor());
		Color toColor = setColorLook(colorProvider.getCurrentColor());

		cam.backgroundColor = Color.Lerp(fromColor, toColor, Mathf.PingPong(Time.time, rate) / rate);
	}

	private Color setColorLook(Color color) {
		float hue;
		float saturation;
		float value;
		Color.RGBToHSV(color, out hue, out saturation, out value);

		saturation *= 0.25f;
		value *= 0.5f;

		return Color.HSVToRGB(hue, saturation, value);
	}
}
