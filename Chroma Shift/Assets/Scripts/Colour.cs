using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Colour : MonoBehaviour {

	//enum for each different colour type
	public enum ColourType { Purple, Blue, Green, Yellow, Orange, Red };
	public ColourType currentColourType = ColourType.Purple;

	//micro class that holds colourType information and all the colours associated with each type
	[System.Serializable]
	public class ColorContainer{
		public ColourType type;
		public Color[] colors;
	}


	//Container that has all the colors in it
	public ColorContainer[] colors;
	//a dictionary that will have each colour type and each shade of each type
	public Dictionary<ColourType, Color[]> colorDict;

	// Use this for initialization
	void Start () 
	{
		LoadColours();
	}
	public ColourType NextColour()
	{
		return currentColourType++;
	}
	// Update is called once per frame
	void Update () 
	{

	
	}

	private void LoadColours()
	{
		colorDict = new Dictionary<ColourType, Color[]>();

		foreach(var container in colors)
			colorDict.Add(container.type, container.colors);
	}
}
