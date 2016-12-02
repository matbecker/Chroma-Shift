using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : Photon.MonoBehaviour {

	[SerializeField] Transform[] spawnPoints;
	[SerializeField] GameObject[] heroes;
	[SerializeField] Transform startPoint;
	public Transform currentSpawnPoint;
	private int spawnPointIndex;
	public int levelBottom;
	private bool isInit;

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
	}

}
