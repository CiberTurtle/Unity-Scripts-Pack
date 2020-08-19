using UnityEngine;
using TMPro;
using DG.Tweening;

namespace Ciber_Turtle.UI
{
  [AddComponentMenu("Game/Text Animations"), RequireComponent(typeof(TMP_Text)), DisallowMultipleComponent]
  public class UITextAnimations : MonoBehaviour
  {
    TMP_Text text;

    Color ogColor;
    DG.Tweening.Core.TweenerCore<Color, Color, DG.Tweening.Plugins.Options.ColorOptions> tweener;

    private void Awake()
    {
      text = GetComponent<TMP_Text>();
      ogColor = text.color;
    }

    public void PulseColor(Color color, float outTime)
    {
      if (tweener != null) tweener.Kill();
      text.color = color;
      tweener = text.DOColor(ogColor, outTime).SetEase(Ease.Linear).SetUpdate(true);
    }
  }
}