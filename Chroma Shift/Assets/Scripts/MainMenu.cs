using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	public InputField roomNameInput;
	[SerializeField] GameObject[] screens;
	int currentScreen;

	void Start () 
	{
		currentScreen = 0;
	}
	
	public void LoadCharacterSelect()
	{
		if (PhotonNetwork.offlineMode)
			SceneManager.LoadScene("CharacterSelectScreen");
		else
			PhotonNetwork.LoadLevel("CharacterSelectScreen");
	}

	public void SwitchScreen(int newScreenIndex)
	{
		screens[currentScreen].SetActive(false);
		screens[newScreenIndex].SetActive(true);
		currentScreen = newScreenIndex;
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
