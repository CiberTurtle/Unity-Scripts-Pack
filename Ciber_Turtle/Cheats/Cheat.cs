using UnityEngine;
using UnityEngine.Events;

namespace Ciber_Turtle.Cheats
{
  [CreateAssetMenu(fileName = "Cheat", menuName = "Ciber_Turtle/Cheat", order = 0)]
  public class Cheat : ScriptableObject
  {
    public string cheatName;
    public UnityEvent action;
  }
}