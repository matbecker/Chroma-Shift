using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	private static SoundManager instance;
	public static SoundManager Instance
	{
		get
		{
			if (!instance)
				instance = GameObject.FindObjectOfType(typeof(SoundManager)) as SoundManager;

			return instance;
		}
	}
	public Dictionary<string, AudioClip> soundEffects;
	public Dictionary<string, AudioClip> songs;
	[SerializeField] AudioSource musicSource;
	[SerializeField] AudioSource sfxSource;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
