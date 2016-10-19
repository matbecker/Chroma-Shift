using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {

	private static LevelManager instance;
	public static LevelManager Instance
	{
		get
		{
			if (!instance)
				instance = GameObject.FindObjectOfType(typeof(LevelManager)) as LevelManager;

			return instance;
		}
	}


	public float levelCompletionTimer;
	public List<Hero> heroes;

	// Use this for initialization
	void Start () 
	{
		foreach (Hero hero in heroes)
		{
			heroes.Add(hero);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
