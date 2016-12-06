using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class LevelManager : Photon.MonoBehaviour {

	//[SerializeField] Transform[] spawnPoints;
	[SerializeField] GameObject[] heroes;
	[SerializeField] Transform startPoint;
	[SerializeField] Text levelTimerBottom;
	[SerializeField] Text levelTimerTop;
	public Transform currentSpawnPoint;
	private int spawnPointIndex;
	public int levelBottom;
	private bool isInit;
	private int slowAnimCounter;
	private bool shrink;

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

	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}

	// Use this for initialization
	void Start () 
	{
		spawnPointIndex = 0;
		currentSpawnPoint = startPoint;

		//get the name of the colorwheels current gradient
		//colourWheelFaceColours[0].name;

		GameObject go;

		if (!PhotonNetwork.offlineMode)
		{
			var hm = HeroManager.Instance;
			go = PhotonNetwork.Instantiate(HeroManager.Instance.CurrentHeroPrefab.name, currentSpawnPoint.position, Quaternion.identity, 0, 
				new object[]{
				hm.currentColorType,
				hm.currentShadeIndex
			}) as GameObject;
		} 
		else
		{
			go = Instantiate(HeroManager.Instance.CurrentHeroPrefab, currentSpawnPoint.position, Quaternion.identity) as GameObject;
		}
		var colour = go.GetComponent<ColourManager>();

		colour.currentColourType = HeroManager.Instance.currentColorType;
		colour.shadeIndex = HeroManager.Instance.currentShadeIndex;



		//spawnPointIndex++;
		isInit = true;

	}
	
	// Update is called once per frame
	void Update () 
	{
		if (isInit)
		{
			heroes = GameObject.FindGameObjectsWithTag("Player");
			isInit = false;
		}

		levelCompletionTimer -= Time.deltaTime;

		if (slowAnimCounter >= 5)
		{
			if (levelTimerBottom.fontSize >= 40 && levelTimerTop.fontSize >= 40)
				shrink = true;

			if (levelTimerBottom.fontSize <= 20 && levelTimerTop.fontSize <= 20)
				shrink = false;
			
			if (shrink)
			{
				levelTimerBottom.fontSize--;
				levelTimerTop.fontSize--;
			}
			else
			{
				levelTimerTop.fontSize++;
				levelTimerBottom.fontSize++;
			}
			slowAnimCounter = 0;
		}

		levelTimerTop.text = Math.Round(levelCompletionTimer, 1).ToString();
		levelTimerBottom.text = Math.Round(levelCompletionTimer, 1).ToString();

		slowAnimCounter++;

	}

}
