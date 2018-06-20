using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileColorProvider {
	private const int NUM_STEPS = 5;

	private Color baseColor;
	private Color targetColor;
	private float currentStep;

	private Color previousColor;

	public TileColorProvider() {
		targetColor = getRandomColor();
		initColorPair();

		previousColor = baseColor;
	}

	public Color getCurrentColor() {
		return Color.Lerp(baseColor, targetColor, currentStep);
	}

	public Color getPreviousColor() {
		return previousColor;
	}

	public Color getNextColor() {
		previousColor = getCurrentColor();
		currentStep += 1.0f / NUM_STEPS;
		if (currentStep >= 1.0f) {
			initColorPair();
		}
		return getCurrentColor();
	}

	private Color getRandomColor() {
		return Random.ColorHSV(0, 1, 0.65f, 0.75f, 1, 1); //TODO: Fine tune the inputs to this function to get the desired  A E S T H E T I C.
	}

	private void initColorPair() {
		baseColor = targetColor;
		targetColor = getRandomColor();
		currentStep = 0.0f;
	}
}
