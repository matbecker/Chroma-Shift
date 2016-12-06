using UnityEngine;
using System.Collections;
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
		return Physics2D.OverlapCircle(col.bounds.center, 0.1f, collidableLayers);
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
	public static void ShrinkAndExpandText(Text text, int min, int max)
	{
		if (text.fontSize >= min && shrink)
			text.fontSize--;

		if (text.fontSize <= min)
			shrink = false;

		if (text.fontSize <= max && !shrink)
			text.fontSize++;

		if (text.fontSize >= max)
			shrink = true;
	}
}
