using UnityEngine;
using System.Collections;

public class ChangeVolume : MonoBehaviour {

    AudioSource master;

	// Use this for initialization
	void Start () {
        master = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void setVolume(float value)
    {
        master.volume = value;
    }
}
