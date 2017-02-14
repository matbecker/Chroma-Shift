using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public static class HelperFunctions {

	public static LayerMask collidableLayers = 1 << LayerMask.NameToLayer("Collidable");
	private static bool shrink = true;

	public static IEnumerator TransitionTransparency(Image img, float duration)
	{
		while (true)
		{
			img.CrossFadeAlpha(0.0f, duration, false);
			yield return new WaitForSeconds(duration);
			img.CrossFadeAlpha(1.0f, duration, false);
			yield return new WaitForSeconds(duration);
		}
	}
	public static void ColourLerp(GameObject obj, Color start, Color end, float duration, float timer)
	{
		obj.GetComponent<SpriteRenderer>().color = Color.Lerp(start, end, duration);

		timer += Time.deltaTime / duration;
	}
	public static Vector3 ArcTowards(Transform start, Transform end, float angle)
	{
		var direction = end.position - start.position;
		var height = direction.y;
		direction.y = 0;
		var distance = direction.magnitude;
		var a = angle * Mathf.Deg2Rad;
		direction.y = distance * Mathf.Tan(a);
		distance += height / Mathf.Tan(a);
		var velocity = Mathf.Sqrt(distance * Physics.gravity.magnitude / Mathf.Sin(2 * a));

		return velocity * direction.normalized;
	}

	public static Vector2 Arc(Transform projectileLauncher)
	{
		Vector2 val; 

		val = new Vector2(Mathf.Cos(projectileLauncher.localRotation.z * 2), Mathf.Sin(projectileLauncher.localRotation.z * 2));

		return val;
	}

	public static bool GroundCheck(EdgeCollider2D col)
	{
		return Physics2D.OverlapCircle(col.bounds.center - col.bounds.extents, 0.1f, collidableLayers);
	}
	public static bool WallCheck(BoxCollider2D col, Transform transform, bool left)
	{
		if (left)
			return Physics2D.OverlapCircle(new Vector2(transform.position.x - col.bounds.extents.x, transform.position.y + col.bounds.extents.y), 0.1f, collidableLayers);
		else
			return Physics2D.OverlapCircle(new Vector2(transform.position.x + col.bounds.extents.x, transform.position.y + col.bounds.extents.y), 0.1f, collidableLayers);
	}

	public static Color ColorLerp(Color currentColor, Color desiredColor, float duration)
	{
		float startTime = Time.time;
		float endTime = startTime + duration;

		while (Time.time < endTime)
		{
			float elapsedTime = Time.time - startTime;
			float percentComplete = elapsedTime / (endTime - startTime);

			return Color.Lerp(currentColor, desiredColor, percentComplete);
		}
		return currentColor;
	}

	public static void FlipScaleX(GameObject obj, bool facingRight)
	{
		if (facingRight)
			obj.transform.localScale = new Vector3(1.0f, 1.0f,1.0f);
		else
			obj.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
	}

	public static bool CompareColour(ColourManager.ColourType playerColour, ColourManager.ColourType enemyColour, bool isPlayer)
	{
		bool isSame;
		bool isContrasting;

		if (isPlayer)
		{
			isSame = false;

			if (playerColour == enemyColour)
				isSame = true;
			else
				isSame = false;

			return isSame;
		}
		else
		{
			isContrasting = false;

			switch (enemyColour)
			{
			case ColourManager.ColourType.Purple:
				if (playerColour == ColourManager.ColourType.Yellow)
					isContrasting = true;
				break;
			case ColourManager.ColourType.Blue:
				if (playerColour == ColourManager.ColourType.Orange)
					isContrasting = true;
				break;
			case ColourManager.ColourType.Green:
				if (playerColour == ColourManager.ColourType.Red)
					isContrasting = true;
				break;
			case ColourManager.ColourType.Yellow:
				if (playerColour == ColourManager.ColourType.Purple)
					isContrasting = true;
				break;
			case ColourManager.ColourType.Orange:
				if (playerColour == ColourManager.ColourType.Blue)
					isContrasting = true;
				break;
			case ColourManager.ColourType.Red:
				if (playerColour == ColourManager.ColourType.Green)
					isContrasting = true;
				break;
				default:
				break;

				return isContrasting;
			}
				
		}
		return false;
	}
	public static bool IsSameColour(ColourManager.ColourType typeA, ColourManager.ColourType typeB)
	{
		if (typeA == typeB)
			return true;
		else
			return false;
	}
	public static bool IsContrastingColour(ColourManager.ColourType typeA, ColourManager.ColourType typeB)
	{
		switch (typeA)
		{
		case ColourManager.ColourType.Purple:
			if (typeB == ColourManager.ColourType.Yellow)
				return true;
			break;
		case ColourManager.ColourType.Blue:
			if (typeB == ColourManager.ColourType.Orange)
				return true;
			break;
		case ColourManager.ColourType.Green:
			if (typeB == ColourManager.ColourType.Red)
				return true;
			break;
		case ColourManager.ColourType.Yellow:
			if (typeB == ColourManager.ColourType.Purple)
				return true;
			break;
		case ColourManager.ColourType.Orange:
			if (typeB == ColourManager.ColourType.Blue)
				return true;
			break;
		case ColourManager.ColourType.Red:
			if (typeB == ColourManager.ColourType.Green)
				return true;
			break;
		default:
			return false;
			break;
		}
		return false;
	}

	public static void Save(string path, List<LevelObject> levelObjects)
	{
		//var path = EditorUtility.SaveFilePanel("Save Level", Application.streamingAssetsPath + "/Levels", "level.txt", "txt");
		if(path.Length != 0)
		{
			var sb = new System.Text.StringBuilder();
			for(int i = 0; i < levelObjects.Count; i++){
				sb.AppendLine(levelObjects[i].GetSaveString());
			}
			System.IO.File.WriteAllText(path, sb.ToString());
		}
	}

	public static void Load(string path, System.Func<int, LevelObject> creator)
	{
		//var path = 
		if (path.Length != 0)
		{
			var data = System.IO.File.ReadAllText(path);

			var lines = data.Split(new []{'\n'}, System.StringSplitOptions.RemoveEmptyEntries);

			for (int i = 0; i < lines.Length; i++)
			{
				var s = lines[i].Split(LevelObject.SPLIT_CHAR);
				var id = int.Parse(s[0]);

				var obj = creator(id);
				obj.LoadSaveData(lines[i]);
			}
		}
	}
}
