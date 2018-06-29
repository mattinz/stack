using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjectCollider : MonoBehaviour {
	private ObjectPool objectPool;

	private void Start() {
		objectPool = GameObject.FindGameObjectWithTag("ObjectPool").GetComponent<ObjectPool>();
	}

	private void OnTriggerEnter(Collider other) {
		objectPool.returnObjectToPool(other.gameObject);
	}
}
