using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelObjectMap : MonoBehaviour {
	[System.Serializable]
	public class ObjectPair {
		public int objectID;
		public LevelObject obj;
	}

	public ObjectPair[] objectMap;
	private Dictionary<int, LevelObject> objectDict;

	public static LevelObjectMap instance;

	public void Awake() {
		instance = this;
		objectDict = new Dictionary<int, LevelObject>();

		for(int i = 0; i < objectMap.Length; i++) 
		{
			objectDict.Add(objectMap[i].objectID, objectMap[i].obj);
		}
	}

	public LevelObject GetPrefab(int objId)
	{
		if (!objectDict.ContainsKey(objId))
		{
			Debug.LogError("LevelObjectMap.GetPrefab Error: objectDict does not contain key: " + objId);
			return null;
		}
		
		return objectDict[objId];
	}
}
