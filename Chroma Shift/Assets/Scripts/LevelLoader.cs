using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using System;

public class LevelLoader : MonoBehaviour {

	private string currentLevelName;

	public static LevelLoader Instance;

	public event Action<Dictionary<int, List<LevelObject>>> OnLevelLoaded;
	private Dictionary<int, List<LevelObject>> objectLists;

	void Awake () {
		Instance = this;
		DontDestroyOnLoad(gameObject);

		SceneManager.sceneLoaded += OnSceneLoaded;

		objectLists = new Dictionary<int, List<LevelObject>>();
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
	{
		StartCoroutine(OnSceneLoadedCor(scene, sceneMode));
	}

	IEnumerator OnSceneLoadedCor(Scene scene, LoadSceneMode sceneMode) 
	{
		if(scene.name == "LevelLoader")
		{
			objectLists.Clear();
			var path = Application.streamingAssetsPath + "/Levels/" + currentLevelName + ".txt";;
			HelperFunctions.Load(path, id => {
				var prefab = LevelObjectMap.instance.GetPrefab(id);

				var obj = Instantiate(prefab);

				//insert elements into dictionary indexed by objectID
				if(objectLists.ContainsKey(obj.objectID) == false) 
				{
					objectLists.Add(obj.objectID, new List<LevelObject>());
				}
				//insert each object into its approriate list
				objectLists[obj.objectID].Add(obj);

				return obj;

			});

			yield return 1f;
			yield return 1f;

			if(OnLevelLoaded != null)
			{
				OnLevelLoaded(objectLists);
			}
		}
	}

	public void LoadLevel(string levelName)
	{
		currentLevelName = levelName;
		SceneManager.LoadScene("LevelLoader");
	}
}
