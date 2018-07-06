using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
public class TileCollisionSound : MonoBehaviour {

	[SerializeField] private float maxVolumeCollisionVelocityThreshold = 4.0f;
	[SerializeField] private float minTimeBetweenCollisions = 0.1f;

	private AudioSource audioSource;
	private Rigidbody rigidBody;
	private float startingVolume;

	private float elapsedTime;
	private bool canPlaySound;

	private void Awake() {
		audioSource = GetComponent<AudioSource>();
		rigidBody = GetComponent<Rigidbody>();

		startingVolume = audioSource.volume;
		elapsedTime = 0.0f;
		canPlaySound = true;
	}

	private void Update() {
		if (!rigidBody.isKinematic) {
			elapsedTime += Time.deltaTime;
			if(elapsedTime >= minTimeBetweenCollisions) {
				elapsedTime = 0.0f;
				canPlaySound = true;
			}
		}
	}

	private void OnCollisionEnter(Collision collision) {
		if(!rigidBody.isKinematic && canPlaySound) {
			audioSource.volume = startingVolume * Mathf.Clamp01(collision.relativeVelocity.magnitude / maxVolumeCollisionVelocityThreshold);
			audioSource.Play();
			elapsedTime = 0.0f;
			canPlaySound = false;
		}
	}
}
