using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelEditorSidebarButton : MonoBehaviour {

	public LevelEditorSidebar.Tool tool;
	public int toolIndex;
	[SerializeField] Image image;
	public LevelObject createdObject;

	public void SetHighlight(Color color)
	{
		image.color = color;
	}
}
