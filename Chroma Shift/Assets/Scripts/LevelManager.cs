using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : Photon.MonoBehaviour {

	[SerializeField] Vector3 startPoint;
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
		
		GameObject go;

		if (!PhotonNetwork.offlineMode)
		{
			var hm = HeroManager.Instance;
			go = PhotonNetwork.Instantiate(HeroManager.Instance.CurrentHeroPrefab.name, startPoint, Quaternion.identity, 0, 
				new object[]{
				hm.currentColorType,
				hm.currentShadeIndex
			}) as GameObject;
		} 
		else
		{
			go = Instantiate(HeroManager.Instance.CurrentHeroPrefab, startPoint, Quaternion.identity) as GameObject;
		}
		var colour = go.GetComponent<ColourManager>();

		colour.currentColourType = HeroManager.Instance.currentColorType;
		colour.shadeIndex = HeroManager.Instance.currentShadeIndex;

	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

}
