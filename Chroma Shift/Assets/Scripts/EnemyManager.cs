using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour {

	public enum EnemyType { Bouncer, Kamikazer, Buzzer };
	public EnemyType type;

	private static EnemyManager instance;
	public static EnemyManager Instance
	{
		get
		{
			if (!instance)
				instance = GameObject.FindObjectOfType(typeof(EnemyManager)) as EnemyManager;

			return instance;
		}
	}
	public GameObject[] enemyTypes;
	public int enemyCount;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
