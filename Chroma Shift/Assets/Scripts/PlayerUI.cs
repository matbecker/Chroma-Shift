using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerUI : Photon.MonoBehaviour {

	private static PlayerUI instance;
	public static PlayerUI Instance
	{
		get
		{
			if (!instance) 
				instance = GameObject.FindObjectOfType (typeof(PlayerUI)) as PlayerUI;

			return instance;
		}

	}

	[SerializeField] Hero hero;
	[SerializeField] Image healthBar;
	[SerializeField] Sprite[] heroImages;
	[SerializeField] GameObject[] colourShifts;
	[SerializeField] Image heroImage;
	[SerializeField] Text lifeTextTop;
	[SerializeField] Text lifeTextBottom;
	//[SerializeField] Hero.Type heroType;
	private float currentHealth;
	private float prevHealth;
	private bool isInit;

	void OnHeroSpawned(Hero hero) 
	{
		this.hero = hero;
		heroImage.sprite = heroImages[(int)hero.type];

		InputManager.Instance.SwitchColour += SwitchHealthBarColour;
		InputManager.Instance.SwitchShade += SwitchHealthBarShade;

		healthBar.color = hero.GetComponent<SpriteRenderer>().color;

		SetLifeText();

		for (int i = 0; i < hero.stats.colourShifts; i++)
		{
			colourShifts[i].SetActive(true);
		}

	}

	void Awake() 
	{
		LevelManager.Instance.OnHeroSpawned += OnHeroSpawned;
	}

	// Use this for initialization
	void Start () 
	{
		for (int i = 0; i < colourShifts.Length; i++)
		{
			colourShifts[i].SetActive(false);
		}
	}

	void OnDestroy()
	{
		//if (photonView.isMine || PhotonNetwork.offlineMode)
		//{
		if (InputManager.Instance)
		{
			InputManager.Instance.SwitchColour -= SwitchHealthBarColour;
			InputManager.Instance.SwitchShade -= SwitchHealthBarShade;
		}
			
		//}

	}

	// Update is called once per frame
	void Update () 
	{
		if(hero == null) 
			return;
		
		switch (hero.stats.colourShifts)
		{
		case 0:
			colourShifts[0].SetActive(false);
			break;
		case 1:
			colourShifts[0].SetActive(true);
			colourShifts[1].SetActive(false);
			break;
		case 2:
			colourShifts[1].SetActive(true);
			colourShifts[2].SetActive(false);
			break;
		case 3:
			colourShifts[2].SetActive(true);
			colourShifts[3].SetActive(false);
			break;
		case 4:
			colourShifts[3].SetActive(true);
			colourShifts[4].SetActive(false);
			break;
		case 5:
			colourShifts[4].SetActive(true);
			break;
		default:
			break;
		}
		//fix this shit
		healthBar.color = hero.GetComponent<SpriteRenderer>().color;

		healthBar.rectTransform.localScale = new Vector2((float)hero.stats.currentHealth / (float)hero.stats.maxHealth, 1.0f);
		//Mathf.Lerp(prevHealth, currentHealth, 0.5f);
	
	}
	private void SwitchHealthBarColour()
	{
		//if (photonView.isMine || PhotonNetwork.offlineMode)
		//if (hero.stats.colourShifts != 0)
//		for (int i = 0; i < hero.stats.colourShifts; i++)
//		{
//			colourShifts[i].SetActive(true);
//		}
		healthBar.color = hero.GetComponent<SpriteRenderer>().color;
	}
	private void SwitchHealthBarShade()
	{
		//if (photonView.isMine || PhotonNetwork.offlineMode)
		//if (hero.stats.colourShifts != 0)
		healthBar.color = hero.GetComponent<SpriteRenderer>().color;
	}
	public void SetLifeText()
	{
		lifeTextBottom.text = "x  " + hero.stats.lives.ToString();;
		lifeTextTop.text = "x  " + hero.stats.lives.ToString();
	}
}
