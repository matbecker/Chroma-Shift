using UnityEngine;
using System.Collections;
using System.Linq;

public class SpawnPoint : LevelObject {

	[SerializeField] GameObject spawnerManager;
	[SerializeField] Animator anim;
	// Use this for initialization
	void Awake()
	{
		if (!inEditor)
			spawnerManager = GameObject.FindGameObjectWithTag("SpawnerManager");
	}
	void Start () 
	{
		if (!inEditor)
			transform.parent = spawnerManager.transform;

	}

	public void PlayHeroEntry() 
	{
		anim.SetTrigger("heroEntry");
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			LevelManager.Instance.currentSpawnPoint = this;
		}
	}
	public override string GetSaveString ()
	{
		return string.Join(SPLIT_CHAR.ToString(), new []{objectID, transform.position.x, transform.position.y, transform.position.z}.Select(s => s.ToString()).ToArray());
	}
	public override void LoadSaveData (string input)
	{
		var data = input.Split(SPLIT_CHAR);

		transform.position = new Vector3(float.Parse(data[1]), float.Parse(data[2]), float.Parse(data[3]));
	}
	public void SpawnPlayer()
	{
		
	}
}
