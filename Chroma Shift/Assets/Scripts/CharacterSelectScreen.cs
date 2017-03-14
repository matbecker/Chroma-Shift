using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;

public class CharacterSelectScreen : Photon.MonoBehaviour {

	public Transform heroPosition;
	public static string currentLevelName;
	private List<GameObject> characters;
	[SerializeField] bool isFocusedScreen;
	[SerializeField] Text characterText;
	private int currentHero;
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
	}
	public void NextColour()
	{
		if (isFocusedScreen)
			characters[currentHero].GetComponentInChildren<Hero>().SwitchColour();
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

			LevelLoader.Instance.LoadLevel(currentLevelName);
//			if (PhotonNetwork.offlineMode)
//				//SceneManager.LoadScene("Level1");
//			else
//			{
//				//PhotonNetwork.LoadLevel("Level1");
//				//PhotonNetwork.automaticallySyncScene = true;
//			}
			

		}
	}
}
