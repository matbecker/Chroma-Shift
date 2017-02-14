using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LevelEditorSidebar : MonoBehaviour {

	public enum Tool { None, Move, Create, Delete };
	private LevelEditorSidebarButton currentButton;
	private Tool currentTool { get { return currentButton.tool; } }
	private int currentIndex { get { return currentButton.toolIndex; } }

	public List<LevelEditorSidebarButton> buttons;
	public LevelObject currentHeldObject;
	public Vector2 gridSize;
	public bool bounds;

	public List<LevelObject> levelObjects = new List<LevelObject>();

	public void Start()
	{
		bounds = true;
	}

	public void OnButtonClicked(LevelEditorSidebarButton button)
	{
		if(currentButton)
			currentButton.SetHighlight(Color.white);

		if(currentHeldObject) 
			Destroy(currentHeldObject.gameObject);

		currentButton = button;

		currentButton.SetHighlight(Color.gray);

		if (currentTool == Tool.Create)
		{
			var worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			worldPos.z = 0;
			currentHeldObject = Instantiate(currentButton.createdObject);
			currentHeldObject.Init(this);
			currentHeldObject.transform.position = worldPos;
			currentHeldObject.enabled = false;
			currentHeldObject.GetComponentInChildren<SpriteRenderer>().color = Color.red;
		}
	}

	void Update()
	{
		if(!currentButton)
			return;

		var worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		worldPos.z = 0;

		bool overlap = false;
		if(currentHeldObject)
		{
			var halfWidth = currentHeldObject.GetOffset();
			//halfWidth.x *= currentHeldObject.transform.localScale.x;
			//halfWidth.y *= currentHeldObject.transform.localScale.y;
			worldPos.x -= worldPos.x % gridSize.x;
			worldPos.y -= worldPos.y % gridSize.y;
			worldPos += halfWidth;

			currentHeldObject.transform.position = worldPos;

			if (Input.GetMouseButtonUp(0) && currentTool == Tool.Move)
			{
				currentHeldObject = null;
			}

			for(int i = 0; i < levelObjects.Count; i++) 
			{
				var c2D = currentHeldObject.collider2D;
				if(c2D.bounds.Intersects(levelObjects[i].collider2D.bounds)) 
				{
					overlap = true;
					break;
				}
			}

//			if (currentHeldObject.transform.position.x < Grid.instance.gridSize.x && currentHeldObject.transform.position.y < Grid.instance.gridSize.y)
//			{
//				bounds = true;
//			}
//			else
//			{
//				bounds = false;
//			}


			var objColor = Color.green;
			if (overlap || !bounds)
				objColor = Color.red;
			
			var sr = new []{currentHeldObject.GetComponent<SpriteRenderer>()};
			if(sr[0] == null)
				sr = currentHeldObject.GetComponentsInChildren<SpriteRenderer>();
			
			for(int i = 0; i < sr.Length; i++)
				sr[i].color = objColor;

		}

		if (!overlap && currentTool == Tool.Create && !EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(0))
		{
			var o = Instantiate(currentButton.createdObject);
			o.Init(this);
			o.transform.position = worldPos;
			levelObjects.Add(o);
		}

	}
		
	public void Save()
	{
		var path = EditorUtility.SaveFilePanel("Save Level", Application.streamingAssetsPath + "/Levels", "level.txt", "txt");
		HelperFunctions.Save(path, levelObjects);
	}

	public void Load()
	{
		var path = EditorUtility.OpenFilePanel("Load Level", Application.streamingAssetsPath + "/Levels", "txt");
		for(int i= 0; i < levelObjects.Count; i++) 
		{
			Destroy(levelObjects[i].gameObject);
		}
		levelObjects.Clear();
			
		HelperFunctions.Load(path, Creator);				
	}

	LevelObject Creator(int id){
		var prefab = LevelObjectMap.instance.GetPrefab(id);
		var obj = Instantiate(prefab);
		obj.Init(this);
		levelObjects.Add(obj);

		return obj;
	}

	public void OnMouseDown(LevelObject obj)
	{
		if (currentTool == Tool.Delete)
		{
			levelObjects.Remove(obj);
			Destroy(obj.gameObject);
		}
		if (currentTool == Tool.Move)
		{
			currentHeldObject = obj;
		}
	}
}