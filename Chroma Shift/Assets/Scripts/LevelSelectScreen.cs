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
	[SerializeField] Image lockImage;

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
		foreach (LevelButton button in levelButtons)
		{
			button.levelName.text = button.levelNameString;
			if (button.isLocked)
			{
				Locked(button.id);
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
}
