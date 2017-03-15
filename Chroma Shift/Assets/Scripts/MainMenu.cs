﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	private static MainMenu instance;
	public static MainMenu Instance
	{
		get
		{
			if (!instance)
				instance = GameObject.FindObjectOfType(typeof(MainMenu)) as MainMenu;

			return instance;
		}
	}

	public InputField roomNameInput;
	[SerializeField] GameObject[] screens;
	int currentScreen;
	[SerializeField] Image[] backgroundImages;
	[SerializeField] Animator[] animators;
	[SerializeField] Animator titleAnim;
	private float animTimer;
	private bool delay;
	private Coroutine delayCor;
	public bool toCharacterSelect;
	public bool toLevelEditor;



	void Start () 
	{
		currentScreen = 0;
		animTimer = 0.0f;
		delayCor = null;
		toLevelEditor = false;
	}

	public void LoadLevelEditor()
	{
		toLevelEditor = true;
		LoadingScreen.Instance.DisplayLoadingScreen(LoadingScreen.ScreenState.Menu);
	}

	public void SwitchScreen(int newScreenIndex)
	{
		switch (newScreenIndex)
		{
		case 0:
			if (currentScreen == 1)
			{
				animators[currentScreen].SetBool("toMultiplayerMenu", false);	
				animators[newScreenIndex].SetBool("toMainMenu", true);
			}
			else if (currentScreen == 2)
			{
				animators[currentScreen].SetBool("toSettingsMenu",false);
				animators[newScreenIndex].SetBool("toMainMenu", true);
			}
			else if (currentScreen == 3)
			{
				delayCor = StartCoroutine(DelayAnimation(1.0f,animators[newScreenIndex], "toMainMenu", true));
				animators[currentScreen].SetBool("activated", false);
				//animators[newScreenIndex].SetBool("toMainMenu", true);
			}
			break;
		case 1:
			animators[currentScreen].SetBool("toMainMenu", false);
			animators[newScreenIndex].SetBool("toMultiplayerMenu", true);
			break;
		case 2:
			animators[currentScreen].SetBool("toMainMenu", false);
			animators[newScreenIndex].SetBool("toSettingsMenu", true);
			break;
		case 3:
			animators[currentScreen].SetBool("toMainMenu", false);
			animators[newScreenIndex].SetBool("activated", true);
			if (delayCor != null)
			{
				StopCoroutine(delayCor);
				delayCor = null;
			}
			break;
		default:
			break;
		}
		currentScreen = newScreenIndex;
	}
//	public void ShowTitle()
//	{
//		titleAnim.SetTrigger("appear");
//	}
//	public void HideTitle()
//	{
//		titleAnim.SetTrigger("dissappear");
//	}
	public IEnumerator DelayAnimation(float delayAmount, Animator anim, string animName, bool playAnim)
	{
		yield return new WaitForSeconds(delayAmount);
		anim.SetBool(animName, playAnim);
	}
	public void ConnectToServer()
	{
		NetworkManager.Instance.Connect();
		PhotonNetwork.offlineMode = false;
	}

	public void DisconnectFromServer()
	{
		NetworkManager.Instance.Disconnect();
		PhotonNetwork.offlineMode = true;
	}

	public void Quit()
	{
		Application.Quit();
	}

	public void SinglePlayer() 
	{ 
		PhotonNetwork.offlineMode = true;
	}

	public void CreateServer() 
	{
		NetworkManager.Instance.StartServer(roomNameInput.text);
	}

	public void JoinServer() 
	{
		NetworkManager.Instance.JoinServer(roomNameInput.text);
	}
}
