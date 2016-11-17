using UnityEngine;
using System.Collections;

public class HeroManager : MonoBehaviour {

	public enum HeroType { Swordsmen, Archer, Ninja, Wizard };

	[System.Serializable]
	public class Heroes
	{
		public HeroType type;
		public GameObject prefab;
		public Vector3 selectScreenPosition;
	}
	//container for Heroes class with their type and prefab
	public Heroes[] heroes;
	public GameObject CurrentHeroPrefab 
	{
		get 
		{
			return heroes[heroIndex].prefab;
		}
	}
	public int heroIndex;
	public ColourManager.ColourType currentColorType;
	public int currentShadeIndex;


	private static HeroManager instance;
	public static HeroManager Instance
	{
		get 
		{
			if (!instance)
				instance = GameObject.FindObjectOfType(typeof(HeroManager)) as HeroManager;

			return instance;
		}
	}
	void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}

}
