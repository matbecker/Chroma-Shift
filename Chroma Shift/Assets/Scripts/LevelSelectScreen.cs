using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectScreen : MonoBehaviour {

	[System.Serializable]
	public class LevelButton
	{
		public int id;
		public Button button;
		public Text levelName;
		public Text bestTime;
		public Image currentStarRating;
		public Image levelPicture;
		public string levelNameString;
		public bool isLocked;
	}
	public LevelButton[] levelButtons;
	public Dictionary<string,float> levelTimeDict;
	[SerializeField] Image lockImage;
	[SerializeField] Color[] starColours;
	[SerializeField] Sprite[] levelPictures;


	private static LevelSelectScreen instance;
	public static LevelSelectScreen Instance
	{
		get
		{
			if (!instance)
				instance = GameObject.FindObjectOfType(typeof(LevelSelectScreen)) as LevelSelectScreen;

			return instance;
		}
	}

	// Use this for initialization
	void Start () 
	{
		levelTimeDict = new Dictionary<string, float>();

		foreach (LevelButton button in levelButtons)
		{
			button.levelName.text = button.levelNameString;
			button.currentStarRating.color = starColours[0];

			if (button.isLocked)
			{
				Locked(button.id);
			}
			else
			{
				button.levelPicture.sprite = levelPictures[button.id];
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void SelectLevel(int index)
	{
		if (levelButtons[index].isLocked)
		{
			return;
		}
		if (levelButtons[index].id == index && !levelButtons[index].isLocked)
		{
			CharacterSelectScreen.currentLevelName = levelButtons[index].levelNameString;
			LevelLoader.Instance.currentLevelId = index;
			SceneManager.LoadScene("CharacterSelectScreen");
		}
		else
		{
			Debug.LogError("Error in LevelSelectScreen class. The Level ID does not match the level index");
		}
	}

	private void Locked(int index)
	{
		var levelButton = levelButtons[index];
		levelButton.button.animator.SetBool("Locked", true);
		levelButton.bestTime.text = "--:--";
		levelButton.currentStarRating.color = Color.clear;
		levelButton.levelName.text = "???";
		levelButton.levelPicture.sprite = lockImage.sprite;
	}
	public void Load()
	{
		var path = Application.streamingAssetsPath + "/LevelTimes/levelTimes.txt";
		//var path = 
		if (path.Length != 0)
		{
			var data = System.IO.File.ReadAllText(path);

			var lines = data.Split(new []{'\n'}, System.StringSplitOptions.RemoveEmptyEntries);

			for (int i = 0; i < lines.Length; i++)
			{
				var s = lines[i].Split(LevelObject.SPLIT_CHAR);
				var id = int.Parse(s[0]);
				var name = s[0];
				var time = s[1];

				//var obj = creator(id);
				//obj.LoadSaveData(lines[i]);
			}
		}
	}
}
