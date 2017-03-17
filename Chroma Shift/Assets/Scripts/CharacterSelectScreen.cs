using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using DG.DemiLib;

public class CharacterSelectScreen : Photon.MonoBehaviour {

	[System.Serializable]
	public class Layout
	{
		public Color[] colors;
	}
	public Layout[] layouts;

	public Transform heroPosition;
	//public static string currentLevelName;
	private List<GameObject> characters;
	[SerializeField] bool isFocusedScreen;
	[SerializeField] Text characterText;
	[SerializeField] Animator anim;
	[SerializeField] Image[] uiSprites;
	[SerializeField] Button[] horizontalButtons;
	private int hIndex;
	[SerializeField] Button[] verticalButtons;
	private int vIndex;
	[SerializeField] EventSystem es;
	private int currentHero;
	public static bool toGame;
	private float timer;

	// Use this for initialization
	void Start () 
	{
		if (isFocusedScreen)
		{
			characters = new List<GameObject>();
			foreach (var hero in HeroManager.Instance.heroes)
			{
				var go = Instantiate(hero.prefab, heroPosition) as GameObject;
				go.SetActive(false);
				var h = go.GetComponentInChildren<Hero>();
				h.enabled = false;
				h.SetupSprite();
				h.stats.colourShifts = 100;
				go.GetComponentInChildren<Rigidbody2D>().gravityScale = 0;
				go.transform.localPosition = hero.selectScreenPosition;
				go.transform.localScale = new Vector3(100.0f,100.0f,1.0f);
				go.GetComponentInChildren<Canvas>().enabled = false;
				characters.Add(go);
			}
			currentHero = 0;
			characters[0].SetActive(true);


		}
		for (int i = 0; i < uiSprites.Length; i++)
		{
			uiSprites[i].DOColor(layouts[0].colors[i], 0.0f);
		}
		if (InputManager.Instance)
		{
			InputManager.Instance.SwitchButton += SwitchButton;
		}
		if (LoadingScreen.Instance)
		{
			LoadingScreen.Instance.Begin += Begin;
		}


	}
	private void OnDestroy()
	{
		if (LoadingScreen.Instance)
		{
			LoadingScreen.Instance.Begin -= Begin;
		}
	}

	public void NextHero()
	{
		if (isFocusedScreen)
		{
			characters[currentHero].SetActive(false);
			currentHero++;
			if(currentHero >= characters.Count){
				currentHero = 0;
			}
			characters[currentHero].SetActive(true);
			HeroManager.Instance.heroIndex = currentHero;
			characterText.text = HeroManager.Instance.heroes[currentHero].type.ToString();
		}
		for (int i = 0; i < uiSprites.Length; i++)
		{
			uiSprites[i].DOColor(layouts[(int)characters[currentHero].GetComponent<Hero>().colour.currentColourType].colors[i], 1.0f);
		}

	}
	public void PreviousHero()
	{
		if (isFocusedScreen)
		{
			characters[currentHero].SetActive(false);
			currentHero--;
			if(currentHero < 0){
				currentHero = characters.Count - 1;
			}
			characters[currentHero].SetActive(true);
			HeroManager.Instance.heroIndex = currentHero;
			characterText.text = HeroManager.Instance.heroes[currentHero].type.ToString();
		}
		for (int i = 0; i < uiSprites.Length; i++)
		{
			uiSprites[i].DOColor(layouts[(int)characters[currentHero].GetComponent<Hero>().colour.currentColourType].colors[i], 1.0f);
		}
	}
	public void NextColour()
	{
		if (isFocusedScreen)
			characters[currentHero].GetComponentInChildren<Hero>().SwitchColour();

		for (int i = 0; i < uiSprites.Length; i++)
		{
			uiSprites[i].DOColor(layouts[(int)characters[currentHero].GetComponent<Hero>().colour.currentColourType].colors[i], 1.0f);
		}
	}
	public void NextShade()
	{
		if (isFocusedScreen)
			characters[currentHero].GetComponentInChildren<Hero>().SwitchShade();
	}
	public void StartGame()
	{
		if (isFocusedScreen)
		{
			var colour = characters[currentHero].GetComponentInChildren<ColourManager>();
			HeroManager.Instance.currentColorType = colour.currentColourType;
			HeroManager.Instance.currentShadeIndex = colour.shadeIndex;
			LoadingScreen.Instance.DisplayLoadingScreen(LoadingScreen.ScreenState.Menu);
			LevelLoader.Instance.LoadLevel(LevelLoader.Instance.currentLevelName);
		}
	}
	private void Begin()
	{
		//LevelLoader.Instance.LoadLevel(LevelLoader.Instance.currentLevelName);
	}
	public void PlayArrowAnimation()
	{
		
	}
	private void SwitchButton(float axis)
	{
		
	}
	private void Update()
	{
		timer += Time.deltaTime;
		var horizontalAxis = Input.GetAxis("SwitchButtonH");
		var verticalAxis = Input.GetAxis("SwitchButtonV");

		if (horizontalAxis != 0)
		{
			if (horizontalAxis < 0)
			{
				if (timer > 0.2f)
				{
					hIndex++;
					timer = 0.0f;
				}

			}
			else if (horizontalAxis > 0)
			{
				if (timer > 0.2f)
				{
					hIndex--;
					timer = 0.0f;
				}
			}
			if (hIndex < 0)
			{
				hIndex = horizontalButtons.Length;
			}
			if (hIndex > horizontalButtons.Length)
			{
				hIndex = 0;
			}
			es.SetSelectedGameObject(horizontalButtons[hIndex].gameObject);

		}
		if (verticalAxis != 0)
		{
			if (verticalAxis < 0)
			{
				if (timer > 0.2f)
				{
					vIndex++;
					timer = 0.0f;
				}

			}
			else if (verticalAxis > 0)
			{
				if (timer > 0.2f)
				{
					vIndex--;
					timer = 0.0f;
				}
			}
			if (vIndex < 0)
			{
				vIndex = horizontalButtons.Length;
			}
			if (vIndex > horizontalButtons.Length)
			{
				vIndex = 0;
			}
			es.SetSelectedGameObject(verticalButtons[vIndex].gameObject);
		}
	}
}
