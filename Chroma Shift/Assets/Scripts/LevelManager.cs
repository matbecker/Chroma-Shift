﻿using UnityEngine;
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
	public Levels[] levels;
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

	private bool inMenu;

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

	private void Start()
	{
		DontDestroyOnLoad(gameObject);

		if (FindObjectsOfType(GetType()).Length > 1)
		{
			Destroy(gameObject);
		}
		levelTimeDict = new Dictionary<string, float>();
		LevelLoader.Instance.OnLevelLoaded += OnLevelLoaded;
		inMenu = true;
		LoadLevelTimes();
	}

	void OnLevelLoaded(Dictionary<int, List<LevelObject>> objectLists) 
	{
		inMenu = false;
		levelIndex = LevelLoader.Instance.currentLevelId;
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
		SaveLevelTimes();
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
		var levelName = levels[levelIndex].name;
		var levelTime = levels[levelIndex].rankTimes[3] - levelTimer;

		if(levelTimeDict.ContainsKey(levelName)) {
			if(levelTime < levelTimeDict[levelName])
				levelTimeDict[levelName] = levelTime;
		} else 
			levelTimeDict.Add(levelName, levelTime);

		//PlayerPrefs.SetFloat("Level_" + levels[levelIndex], levelTime);
		startTimer = false;
	}
	public void SaveLevelTimes()
	{
		var path = Application.streamingAssetsPath + "/LevelTimes/levelTimes.txt";

		var sb = new System.Text.StringBuilder();

		foreach(var level in levelTimeDict)
			sb.AppendLine(level.Key + "_" + level.Value);
		
		System.IO.File.WriteAllText(path, sb.ToString());
	}

	public void LoadLevelTimes()
	{
		var path = Application.streamingAssetsPath + "/LevelTimes/levelTimes.txt";

		levelTimeDict.Clear();

		var file = System.IO.File.ReadAllLines(path);
		foreach(var line in file)
		{
			if (string.IsNullOrEmpty(line)) continue;
			var data = line.Split('_');
			levelTimeDict.Add(data[0], float.Parse(data[1]));
		}
	}

	// Update is called once per frame
	void Update () 
	{
		if (inMenu){
			return;
		}
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
