using UnityEngine;

public class Draw
{
  public static Color defaultColor = Color.red;

  public static void Line(Vector2 start, Vector2 end, Color color = new Color(), float time = 0)
  {
    if (color == Color.clear) color = defaultColor;
    Debug.DrawLine(start, end, color, time);
  }

  // public static void Arrow(Vector2 origin, Vector2 direction, Color color = new Color(), float time = 0)
  // {
  //   Vector2 end = origin + direction.normalized;
  //   if(color == Color.clear) color = defaultColor;
  //   Debug.DrawLine(origin, end, color, time);
  //   Debug.DrawLine(end, end + new Vector2(-0.5f, -0.5f) * end, color, time);
  // }

  public static void Square(Vector2 position, Vector2 size, Color color = new Color(), float time = 0)
  {
    Vector2 hSize = size * 0.5f;
    if (color == Color.clear) color = defaultColor;
    Debug.DrawLine(new Vector2(position.x - hSize.x, position.y - hSize.y), new Vector2(position.x - hSize.x, position.y + hSize.y), color, time);
    Debug.DrawLine(new Vector2(position.x + hSize.x, position.y - hSize.y), new Vector2(position.x + hSize.x, position.y + hSize.y), color, time);
    Debug.DrawLine(new Vector2(position.x - hSize.x, position.y - hSize.y), new Vector2(position.x + hSize.x, position.y - hSize.y), color, time);
    Debug.DrawLine(new Vector2(position.x - hSize.x, position.y + hSize.y), new Vector2(position.x + hSize.x, position.y + hSize.y), color, time);
  }
}
