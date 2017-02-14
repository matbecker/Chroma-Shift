using UnityEngine;
using System.Collections;

public class Dagger : MonoBehaviour {

	[SerializeField] Hero ninja;

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Enemy") && ninja.isAttacking)
		{
			//if the enemy is the same colour as me damage them critically 
			if (HelperFunctions.IsSameColour(ninja.colour.currentColourType, other.GetComponent<Enemy>().colour.currentColourType))
			{
				other.SendMessage("Damage", 10, SendMessageOptions.DontRequireReceiver);
				Debug.Log("++");
			}
			else
				other.SendMessage("Damage", ninja.stats.attackPower, SendMessageOptions.DontRequireReceiver);

			//Debug.Log("My dagger is" + ninja.colour.currentColourType);
		}
	}
}
