using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour {

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

	// Use this for initialization
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
				int enemyType = Random.Range(0,3);

				//instantiate a random enemy type at the enemyspawner location
				GameObject enemyObj = Instantiate(EnemyManager.Instance.enemyTypes[enemyType], transform.position + new Vector3(Random.insideUnitCircle.x * spawnCircleWidth, Random.insideUnitCircle.y * spawnCircleHeight, transform.position.z), Quaternion.identity) as GameObject;
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
				gameObject.SetActive(false);
				//return;


		}
	}
}
