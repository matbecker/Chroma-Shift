using UnityEngine;
using System.Collections;

public class Sword : MonoBehaviour {

	[SerializeField] GameObject swordsmen;

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Enemy"))
		{
			other.SendMessage("Damage", swordsmen.GetComponent<Hero>().stats.attackPower, SendMessageOptions.DontRequireReceiver);
		}
	}
}
