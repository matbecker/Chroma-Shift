using UnityEngine;
using System.Collections;

public class ColourComparer : MonoBehaviour {


	public ColourManager playerColour;
	public ColourManager enemyColour;
	[SerializeField] bool isPlayer;

	public static bool isSameColour;
	public static bool isContrastingColour;



	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	void OnCollisionEnter2D(Collision2D other)
	{
		
	}
//	private void CompareColour(ColourManager.ColourType type, bool isPlayer)
//	{
//		if (isPlayer)
//		{
//			switch (type)
//			{
//				case ColourManager.ColourType.Purple:
//				case ColourManager.ColourType.Blue:
//				case ColourManager.ColourType.Green:
//				case ColourManager.ColourType.Yellow:
//				case ColourManager.ColourType.Orange:
//				case ColourManager.ColourType.Red:
//					if (enemyColour.currentColourType == playerColour.currentColourType)
//						isSameColour = true;
//					else
//						isSameColour = false;
//				break;
//			default:
//				break;
//			}
//		}
//		else
//		{
//			switch (type)
//			{
//				case ColourManager.ColourType.Purple:
//			case ColourManager.ColourType.Blue:
//			case ColourManager.ColourType.Green:
//			case ColourManager.ColourType.Yellow:
//			case ColourManager.ColourType.Orange:
//			case ColourManager.ColourType.Red:
//					if (enemyColour.currentColourType == ColourManager.ColourType.Orange)
//					{
//
//					}
//					break;
//				default:
//					break;
//			}
//		}
//
//	}
}
