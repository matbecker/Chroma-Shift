using UnityEngine;
using System.Collections;

public class Dagger : MonoBehaviour {

	[SerializeField] GameObject ninja;
	// Use this for initialization
	void Start () 
	{
		ninja = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Enemy") && ninja.GetComponent<Hero>().isAttacking)
		{
			other.gameObject.SendMessage("Damage", ninja.GetComponent<Hero>().stats.attackPower, SendMessageOptions.DontRequireReceiver);
		}
	}
}
