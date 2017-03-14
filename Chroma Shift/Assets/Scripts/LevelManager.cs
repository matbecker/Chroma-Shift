using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using DG.Tweening;
using DG.DemiLib;

public class LevelManager : Photon.MonoBehaviour {

	[System.Serializable]
	public class Levels
	{
		public int id;
		public string name;
		public float[] rankTimes;
	}
	[SerializeField] Levels[] levels;
	public Dictionary<string,float> levelTimeDict;
	private int levelIndex;
	public float levelTimer;
	[SerializeField] Transform[] spawnPoints;
	public const int LEVEL_BOTTOM = -10;
	public const int LEVEL_TOP = 10;
	public SpawnPoint currentSpawnPoint;
	public SpawnPoint startingPoint;
	private int spawnPointIndex;

	private Hero hero;
	public bool restart;
	public bool startTimer;
	public Action<Hero> OnHeroSpawned;
	public delegate void RestartLevel();
	public event RestartLevel Restart;

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
		levelTimeDict = new Dictionary<string, float>();
		levelIndex = LevelLoader.Instance.currentLevelId;
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
				startingPoint = ps;

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
			if(hero == null)
			{
				go = Instantiate(HeroManager.Instance.CurrentHeroPrefab, currentSpawnPoint.transform.position, Quaternion.identity) as GameObject;
				hero = go.GetComponentInChildren<Hero>();

				hero.colour.currentColourType = HeroManager.Instance.currentColorType;
				hero.colour.shadeIndex = HeroManager.Instance.currentShadeIndex;
			}
			else 
			{
				hero.transform.position = currentSpawnPoint.transform.position;
			}

			if(OnHeroSpawned != null) 
			{
				OnHeroSpawned(hero);
			}
			//PlayerUI.Instance.SetHero(hero);
			//TODO play player animation
			hero.OnHeroSpawn();
		}
		ColourWheel.Instance.Shift();

		startTimer = true;
		//var h = go.GetComponent<Hero>();
	}

	public void NextLevel()
	{
		Save();
		levelIndex++;
		LevelLoader.Instance.LoadLevel(levels[levelIndex].name);

		transform.DOScale(Vector3.one, 1.0f).OnComplete(() => {
			PlayerUI.Instance.timerAnim.SetBool("finish", false);
			hero.stats.currentHealth = hero.stats.maxHealth;
			hero.grounded = false;
			hero.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		});

	}
	public void FinishedLevel()
	{
		var levelTime = levels[levelIndex].rankTimes[3] - levelTimer;
		levelTimeDict.Add(levels[levelIndex].name, levelTime);

		startTimer = false;
	}
	public void Save()
	{
		var path = Application.streamingAssetsPath + "/LevelTimes/levelTimes.txt";

		if (path.Length != 0)
		{
			var sb = new System.Text.StringBuilder();

			sb.AppendLine(GetSaveString());

//			for (int i = 0; i < levelTimeDict.Count; i++)
//			{
//				
//			}
			System.IO.File.WriteAllText(path, sb.ToString());

		}
	}
	public string GetSaveString()
	{
		return string.Join("_", new []{levels[levelIndex].name, levelTimeDict[levels[levelIndex].name].ToString()});
	}
	// Update is called once per frame
	void Update () 
	{
		if (startTimer)
			levelTimer -= Time.deltaTime;

		if (levelTimer < 10.0f)
			PlayerUI.Instance.TimerFlash();
		
		if (levelTimer < 0.0f)
		{
			if (Restart != null)
			{
				currentSpawnPoint = startingPoint;
				currentSpawnPoint.PlayHeroEntry();
				ColourWheel.Instance.Shift();
				Restart();
				levelTimer = GetLevelTime();
			}
		}
		if (restart && Restart != null)
		{

			currentSpawnPoint = startingPoint;
			currentSpawnPoint.PlayHeroEntry();
			ColourWheel.Instance.Shift();
			Restart();
			levelTimer = GetLevelTime();
			restart = false;
		}
	}
	public float GetLevelTime()
	{
		return levels[levelIndex].rankTimes[3];
	}
}
