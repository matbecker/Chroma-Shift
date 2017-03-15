using UnityEngine;
using System.Collections;
using DG.DemiLib;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour {

	public enum ScreenState { Menu, Restart, Next };
	private static LoadingScreen instance;
	public static LoadingScreen Instance
	{
		get
		{
			if (!instance)
				instance = GameObject.FindObjectOfType(typeof(LoadingScreen)) as LoadingScreen;

			return instance;
		}
	}
	public delegate void StartEvent();
	public event StartEvent Begin;

	[SerializeField] Image loadingOverlay;
	[SerializeField] Animator anim;
	[SerializeField] GameObject renderTarget;
	[SerializeField] GameObject originalTarget;
	public bool startGame;

	public void DisplayLoadingScreen(ScreenState screenState)
	{
		transform.parent = renderTarget.transform;

		loadingOverlay.DOColor(Color.white, 0.25f).OnComplete(() => 
		{
			anim.SetTrigger("startSpiral");
			loadingOverlay.DOColor(Color.black, 1.0f).OnComplete(() => {
				

				switch (screenState)
				{
				case ScreenState.Menu:
					if (MainMenu.Instance.toCharacterSelect)
					{
						SceneManager.LoadScene("CharacterSelectScreen");
					}
					if (MainMenu.Instance.toLevelEditor)
					{
						SceneManager.LoadScene("LevelEditor");
					}
					if (Begin != null)
					{
						Begin();
					}
					transform.parent = originalTarget.transform;
					break;
				case ScreenState.Next:
					LevelManager.Instance.NextLevel();
					transform.parent = originalTarget.transform;
					break;
				case ScreenState.Restart:
					LevelManager.Instance.restart = true;
					transform.parent = originalTarget.transform;
					break;
				}
				loadingOverlay.DOColor(Color.clear, 1.0f);
			});


		});
	}
	// Use this for initialization
	void Start () 
	{
		DontDestroyOnLoad(gameObject);

		if (FindObjectsOfType(GetType()).Length > 1)
		{
			Destroy(gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
