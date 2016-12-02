using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			LevelManager.Instance.currentSpawnPoint = gameObject.transform;
		}
	}
}
