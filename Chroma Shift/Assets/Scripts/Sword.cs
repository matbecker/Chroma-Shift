using UnityEngine;
using System.Collections;

public class Sword : MonoBehaviour {

	[SerializeField] Hero swordsmen;

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Enemy"))
		{
			//if the enemy is the same colour as me damage them critically 
			if (HelperFunctions.IsSameColour(swordsmen.colour.currentColourType, other.GetComponent<Enemy>().colour.currentColourType))
			{
				other.SendMessage("Damage", 10, SendMessageOptions.DontRequireReceiver);
				Debug.Log("++");
			}
			else //if not the same colour do regular amount of damage
				other.SendMessage("Damage", swordsmen.stats.attackPower, SendMessageOptions.DontRequireReceiver);
		}
	}
}
