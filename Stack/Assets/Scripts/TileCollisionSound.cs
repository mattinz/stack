using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
public class TileCollisionSound : MonoBehaviour {

	private AudioSource audioSource;
	private Rigidbody rigidBody;

	private void Awake() {
		audioSource = GetComponent<AudioSource>();
		rigidBody = GetComponent<Rigidbody>();
	}

	private void OnCollisionEnter(Collision collision) {
		if(!rigidBody.isKinematic) {
			audioSource.Play();
		}
	}
}
