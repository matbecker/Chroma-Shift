using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class EnemySpawner : LevelObject {

	[SerializeField] int minEnemies;
	[SerializeField] int maxEnemies;
	public static List<GameObject> enemyWave;
	[SerializeField] GameObject[] barriers;
	public bool hasWaveStarted;
	private bool isWaveOver;
	private bool startLerp;
	private float startLerpTimer;
	private float endLerpTimer;
	[SerializeField] float lerpDuration;
	[SerializeField] Text enemyWaveCountBottomText;
	[SerializeField] Text enemyWaveCountTopText;
	[SerializeField] int spawnCircleWidth;
	[SerializeField] int spawnCircleHeight;
	[SerializeField] Enemy[] enemyTypes;
	[SerializeField] GameObject spawnerManager;

	// Use this for initialization
	void Awake()
	{
		if (!inEditor)
			spawnerManager = GameObject.FindGameObjectWithTag("EnemySpawnerManager");
	}
	void Start () 
	{
		hasWaveStarted = false;

		enemyWave = new List<GameObject>();

		for (int i = 0; i < barriers.Length; i++)
		{
			barriers[i].GetComponent<SpriteRenderer>().color = Color.clear;
			barriers[i].GetComponent<BoxCollider2D>().enabled = false;
		}

		enemyWaveCountBottomText.color = Color.clear;
		enemyWaveCountTopText.color = Color.clear;

		if (!inEditor)
			transform.parent = spawnerManager.transform;
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player") && !hasWaveStarted)
		{
			for (int i = 0; i < barriers.Length; i++)
			{
				barriers[i].GetComponent<BoxCollider2D>().enabled = true;
			}

			//get a random number of enemies to spawn based on the min and max provided
			int enemyCount = Random.Range(minEnemies, maxEnemies);

			//loop through as many times as the enemy count returned
			for (int i = 0; i <= enemyCount; i++)
			{
				int enemyType = Random.Range(0,enemyTypes.Length);

				//instantiate a random enemy type at the enemyspawner location
				GameObject enemyObj = Instantiate(enemyTypes[enemyType], transform.position + new Vector3(Random.insideUnitCircle.x * spawnCircleWidth, Random.insideUnitCircle.y * spawnCircleHeight, transform.position.z), Quaternion.identity) as GameObject;
				//add the enemy to the wave list
				enemyWave.Add(enemyObj);
			}
			hasWaveStarted = true;
		}
	}
	void Update()
	{
		if (hasWaveStarted)
		{
			for (int i = 0; i < barriers.Length; i++)
			{
				//HelperFunctions.ColourLerp(barriers[i], Color.clear, Color.white, 0.1f, startLerpTimer);
				barriers[i].GetComponent<SpriteRenderer>().color = Color.Lerp(Color.clear, Color.white, startLerpTimer);
			}

			enemyWaveCountBottomText.color = Color.Lerp(Color.clear,Color.black, startLerpTimer);
			enemyWaveCountTopText.color = Color.Lerp(Color.clear,Color.white, startLerpTimer);
			enemyWaveCountBottomText.text = enemyWave.Count.ToString();
			enemyWaveCountTopText.text = enemyWave.Count.ToString();
			startLerpTimer += Time.deltaTime / lerpDuration;
		}
		if (hasWaveStarted && enemyWave.Count == 0)
		{
			for (int i = 0; i < barriers.Length; i++)
			{
				//HelperFunctions.ColourLerp(barriers[i], Color.white, Color.clear, 0.1f, endLerpTimer);
				barriers[i].GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, Color.clear, endLerpTimer);
				barriers[i].GetComponent<BoxCollider2D>().enabled = false;
			}
			enemyWaveCountBottomText.color = Color.Lerp(Color.black,Color.clear, endLerpTimer);
			enemyWaveCountTopText.color = Color.Lerp(Color.white,Color.clear, endLerpTimer);

			endLerpTimer += Time.deltaTime / lerpDuration;

			if (endLerpTimer > lerpDuration)
			{
				float rand = Random.value;

				if (rand > 0.5f)
					ColourWheel.Instance.Shift();

				gameObject.SetActive(false);
			}
		}
	}
	public static void ClearEnemies()
	{
		if(enemyWave != null) 
		{
			foreach (GameObject enemy in enemyWave)
			{
				Destroy(enemy);
			}
			enemyWave.Clear();
		}
	}

	public override string GetSaveString ()
	{
		//int differentEnemyTypes = enemyTypes.Count;

		var saveString = string.Join(SPLIT_CHAR.ToString(), new []{objectID, transform.position.x, transform.position.y, transform.position.z, 
														barriers[0].transform.position.x, barriers[0].transform.position.y, barriers[0].transform.position.z,
														barriers[1].transform.position.x, barriers[1].transform.position.y, barriers[1].transform.position.z,
														minEnemies, maxEnemies, spawnCircleWidth, spawnCircleHeight, barriers[0].transform.localScale.y,
				/*enemyTypes[0]*/}.Select(s => s.ToString()).ToArray());

		if(enemyTypes == null || enemyTypes.Length == 0)
		{
			saveString += SPLIT_CHAR.ToString() + "0";

		} else {
			saveString += SPLIT_CHAR.ToString() + enemyTypes.Length;
			for(int i = 0; i < enemyTypes.Length; i++)
			{
				saveString += SPLIT_CHAR.ToString() + enemyTypes[i].objectID;
			}
		}
		return saveString;
	}

	public override void LoadSaveData (string input)
	{
		var data = input.Split(SPLIT_CHAR);

		transform.position = new Vector3(float.Parse(data[1]), float.Parse(data[2]), float.Parse(data[3]));
		barriers[0].transform.position = new Vector3(float.Parse(data[4]), float.Parse(data[5]), float.Parse(data[6]));
		barriers[1].transform.position = new Vector3(float.Parse(data[7]), float.Parse(data[8]), float.Parse(data[9]));
		minEnemies = int.Parse(data[10]);
		maxEnemies = int.Parse(data[11]);
		spawnCircleWidth = int.Parse(data[12]);
		spawnCircleHeight = int.Parse(data[13]);
		barriers[0].transform.localScale = new Vector3(barriers[0].transform.localScale.x, float.Parse(data[14]), barriers[0].transform.localScale.z);
		barriers[1].transform.localScale = new Vector3(barriers[1].transform.localScale.x, float.Parse(data[14]), barriers[1].transform.localScale.z);

		var enemys = new List<Enemy>();
		var num = int.Parse(data[15]);

		for(int i = 0; i < num; i++){
			var objectID = int.Parse(data[16 + i]);
			var enemyPrefab = (Enemy)LevelObjectMap.instance.GetPrefab(objectID);
			enemys.Add(enemyPrefab);
		}
		enemyTypes = enemys.ToArray();
		
	}
	public override Vector3 GetOffset ()
	{
		return Vector3.zero;
	}
}
