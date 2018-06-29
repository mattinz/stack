using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {

	[SerializeField] private GameObject prefab;
	[SerializeField] private int numInstances = 50;

	private List<GameObject> pooledObjects;

	// Use this for initialization
	void Start () {
		pooledObjects = new List<GameObject>();
		for(int x = 0; x < numInstances; x++) {
			pooledObjects.Add(instantiateGameObject(prefab));
		}
	}

	public GameObject getPooledObject() {
		GameObject result = null;
		foreach(GameObject gameObject in pooledObjects) {
			if(!gameObject.activeInHierarchy) {
				result = gameObject;
				
				break;
			}
		}

		if(result == null) {
			result = instantiateGameObject(prefab);
			pooledObjects.Add(result);
		}

		result.SetActive(true);
		return result;
	}

	public void returnObjectToPool(GameObject gameObject) {
		if(pooledObjects.Contains(gameObject)) {
			gameObject.SetActive(false);
			gameObject.transform.position = Vector3.zero;
			gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
			Rigidbody rigidBody = gameObject.GetComponent<Rigidbody>();
			rigidBody.velocity = Vector3.zero;
			rigidBody.angularVelocity = Vector3.zero;
			rigidBody.isKinematic = true;
		}
	}

	private GameObject instantiateGameObject(GameObject objectToInstantiate) {
		GameObject gameObject = Instantiate(objectToInstantiate);
		gameObject.SetActive(false);
		return gameObject;
	}
}
