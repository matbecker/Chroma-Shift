using UnityEngine;
using System.Collections;
using System.Linq;

public class LevelObject : MonoBehaviour {
	
//	public const int PLAYER_SPAWNER = 100;
//	public const int PLAYER_SPAWNER = 101;
//	public const int PLAYER_SPAWNER = 102;

	public const int PLAYER_SPAWNER = 200;
	public const int ENEMY_SPAWNER = 201;

	public int objectID;
	public const char SPLIT_CHAR = '_';
	private LevelEditorSidebar sidebar;
	protected bool inEditor = false;
	public BoxCollider2D collider2D = null;

	public virtual string GetSaveString ()
	{
		return "";
	}
	public virtual void LoadSaveData (string input)
	{
		
	}
	public void Init(LevelEditorSidebar s)
	{
		sidebar = s;
		inEditor = true;
		enabled = false;
		collider2D = GetComponent<BoxCollider2D>();
		collider2D.size *= 0.99f;
	}
	void OnMouseDown()
	{
		if(inEditor)
			sidebar.OnMouseDown(this);
	}
	public virtual Vector3 GetOffset()
	{
		return gameObject.GetComponent<SpriteRenderer>().bounds.extents; 
	}
}
