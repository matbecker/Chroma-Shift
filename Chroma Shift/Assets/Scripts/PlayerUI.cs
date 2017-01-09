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

	// Use this for initialization
	void Start () 
	{
		

		for (int i = 0; i < colourShifts.Length; i++)
		{
			colourShifts[i].SetActive(false);
		}

		isInit = true;
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
		if (isInit)
			hero = GameObject.FindGameObjectWithTag("Player").GetComponent<Hero>();
			
		if (hero.isInit)
		{
			switch (hero.type)
			{
			case Hero.Type.Archer:
				heroImage.sprite = heroImages[(int)hero.type];
				heroImage.rectTransform.localPosition = new Vector2(-468.0f, 214.0f);
				heroImage.rectTransform.sizeDelta = new Vector2(15.0f, 15.0f);
				break;
			case Hero.Type.Ninja:
				heroImage.sprite = heroImages[(int)hero.type];
				heroImage.rectTransform.localPosition = new Vector2(-470.0f, 208.0f);
				heroImage.rectTransform.sizeDelta = new Vector2(20.0f, 10.0f);
				break;
			case Hero.Type.Swordsmen:
				heroImage.sprite = heroImages[(int)hero.type];
				heroImage.rectTransform.localPosition = new Vector2(-470.0f, 208.0f);
				heroImage.rectTransform.sizeDelta = new Vector2(20.0f, 10.0f);
				break;
			case Hero.Type.Wizard:
				heroImage.sprite = heroImages[(int)hero.type];
				heroImage.rectTransform.localPosition = new Vector2(-468.0f, 208.0f);
				heroImage.rectTransform.sizeDelta = new Vector2(20.0f, 10.0f);
				break;
			default:
				break;
			}

			healthBar.color = hero.GetComponent<SpriteRenderer>().color;

			InputManager.Instance.SwitchColour += SwitchHealthBarColour;
			InputManager.Instance.SwitchShade += SwitchHealthBarShade;

			SetLifeText();

			for (int i = 0; i < hero.stats.colourShifts; i++)
			{
				colourShifts[i].SetActive(true);
			}

			hero.isInit = false;
			isInit = false;
		}
		 
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
