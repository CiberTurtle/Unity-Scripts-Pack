using UnityEngine;

public class Util
{
	public static Vector2 ToVector2(float value)
	{
		return new Vector2(value, value);
	}

	public static Vector3 ToVector3(float value)
	{
		return new Vector3(value, value, value);
	}

	public static Vector2 GetMousePos(Camera camera = null)
	{
		if (!camera)
		{
			camera = Camera.main;
		}

		return camera.ScreenToWorldPoint(Input.mousePosition);
	}

	public static Quaternion PointToRot(Vector2 targetPos, Vector2 pos)
	{
		Vector2 difference = targetPos - pos;
		return Quaternion.Euler(0f, 0f, Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg - 90);
	}

	public static Vector3 GetVectorDir(Vector3 start, Vector3 end, bool normalise = true)
	{
		if (normalise)
		{
			return (start - end).normalized;
		}

		return (start - end);
	}

	public static float VectorToAngle(Vector3 vector)
	{
		vector.Normalize();
		var ang = Mathf.Asin(vector.y) * Mathf.Rad2Deg;
		if (vector.x < 0)
		{
			ang = 180 - ang;
		}
		else
		if (vector.y < 0)
		{
			ang = 360 + ang;
		}
		return ang;
	}

	public static bool IsInRange(float value, float min, float max)
	{
		return value <= min && value >= max;
	}

	public static Vector2 RandomVector2(float min, float max, bool normalise = true)
	{
		if (normalise)
		{
			return new Vector2(Random.Range(min, max), Random.Range(min, max));
		}
		else
		{
			return new Vector2(Random.Range(min, max), Random.Range(min, max)).normalized;
		}
	}

	public static bool DistEqual(Vector2 pointA, Vector2 pointB, float amount)
	{
		return (pointA - pointB).sqrMagnitude == amount * amount;
	}

	public static bool DistGT(Vector2 pointA, Vector2 pointB, float amount)
	{
		return (pointA - pointB).sqrMagnitude > amount * amount;
	}

	public static bool DistLT(Vector2 pointA, Vector2 pointB, float amount)
	{
		return (pointA - pointB).sqrMagnitude < amount * amount;
	}

	public static int GetRandomItem(int length)
	{
		return Mathf.RoundToInt(Random.Range(0, length));
	}

	public static Color ChangeAlpha(Color color, float a)
	{
		return new Color(color.r, color.g, color.b, a);
	}

	public static Transform TryInstantiate(Transform objectTransform, Vector3 position, Quaternion rotation)
	{
		if (objectTransform)
			return MonoBehaviour.Instantiate(objectTransform, position, rotation);
		else
			return null;
	}

	public static GameObject TryInstantiate(GameObject gameObject, Vector3 position, Quaternion rotation)
	{
		if (gameObject)
			return MonoBehaviour.Instantiate(gameObject, position, rotation);
		else
			return null;
	}

	public static bool VectorGTF(Vector2 vector, float value, bool useABS)
	{
		if (useABS) return (Mathf.Abs(vector.x) > value) || (Mathf.Abs(vector.y) > value);
		return (vector.x > value) || (vector.y > value);
	}

	public static bool VectorLTF(Vector2 vector, float value, bool useABS)
	{
		if (useABS) return (Mathf.Abs(vector.x) < value) || (Mathf.Abs(vector.y) < value);
		return (vector.x < value) || (vector.y < value);
	}
}