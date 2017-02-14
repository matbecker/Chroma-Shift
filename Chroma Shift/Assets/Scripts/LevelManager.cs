using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : Photon.MonoBehaviour {

	[System.Serializable]
	public class Levels
	{
		public int id;
		public string name;
		public float[] rankTimes;
	}
	[SerializeField] Levels[] levels;
	private int levelIndex;
	private float levelTimer;
	[SerializeField] Transform[] spawnPoints;
	public const int levelBottom = -10;
	public SpawnPoint currentSpawnPoint;
	private int spawnPointIndex;
	private bool isInit;

	public Action<Hero> OnHeroSpawned;

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




	private void Awake()
	{
		//spawnPointIndex = 0;
		//spawnPoints[spawnPointIndex] = GameObject.FindGameObjectWithTag("Spawner").transform;
		LevelLoader.Instance.OnLevelLoaded += OnLevelLoaded;
	}

	void OnLevelLoaded(Dictionary<int, List<LevelObject>> objectLists) 
	{
		levelTimer = levels[levelIndex].rankTimes[3];
		currentSpawnPoint = null;//spawnPoints[spawnPointIndex];

		var playerSpawners = objectLists[LevelObject.PLAYER_SPAWNER];
		foreach(SpawnPoint ps in playerSpawners)
		{
			if(currentSpawnPoint == null || currentSpawnPoint.transform.position.x > ps.transform.position.x)
			{
				currentSpawnPoint = ps;
			}
		}
		currentSpawnPoint.PlayHeroEntry();

		//get the name of the colorwheels current gradient
		//colourWheelFaceColours[0].name;

		GameObject go;

		if (!PhotonNetwork.offlineMode)
		{
			var hm = HeroManager.Instance;
			go = PhotonNetwork.Instantiate(HeroManager.Instance.CurrentHeroPrefab.name, currentSpawnPoint.transform.position, Quaternion.identity, 0, 
				new object[]{
				hm.currentColorType,
				hm.currentShadeIndex
			}) as GameObject;
		} 
		else
		{
			go = Instantiate(HeroManager.Instance.CurrentHeroPrefab, currentSpawnPoint.transform.position, Quaternion.identity) as GameObject;
			var hero = go.GetComponent<Hero>();

			hero.colour.currentColourType = HeroManager.Instance.currentColorType;
			hero.colour.shadeIndex = HeroManager.Instance.currentShadeIndex;

			if(OnHeroSpawned != null) 
			{
				OnHeroSpawned(hero);
			}
			//PlayerUI.Instance.SetHero(hero);
			//TODO play player animation
			// hero.PlayIntro hero.OnSpawn whatev
		}
		//var h = go.GetComponent<Hero>();



	}
	private void NextLevel()
	{
		
	}
	// Update is called once per frame
	void Update () 
	{
		levelTimer -= Time.deltaTime;
	}
}
