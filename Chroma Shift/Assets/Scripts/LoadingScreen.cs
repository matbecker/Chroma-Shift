using UnityEngine;
using System.Collections;
using DG.DemiLib;
using DG.Tweening;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour {

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
	[SerializeField] Image loadingOverlay;
	[SerializeField] Animator anim;

	public void DisplayLoadingScreen(bool atEnd)
	{
		loadingOverlay.DOColor(Color.white, 0.25f).OnComplete(() => 
		{
			anim.SetTrigger("startSpiral");
			loadingOverlay.DOColor(Color.black, 1.0f).OnComplete(() => {
				loadingOverlay.DOColor(Color.clear, 1.0f);

				if (atEnd)
					LevelManager.Instance.NextLevel();
				else
					LevelManager.Instance.restart = true;
			});


		});
	}
	// Use this for initialization
	void Start () 
	{
		//gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
