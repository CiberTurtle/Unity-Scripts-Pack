using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;
using TMPro;
using DG.Tweening;

namespace Ciber_Turtle.UI
{
  [AddComponentMenu("UI/Title"), DisallowMultipleComponent]
  public class UITitle : MonoBehaviour
  {
    public float titleInTime;
    public float titleOutTime;
    public float titleDelay;
    public float shadowInTime;
    [Space]
    public float subtitleInTime;
    public float subtitleOutTime;
    public float subtitleDelay;
    public float shadowOutTime;
    [Space]
    public float openTime;

    public string title { get => m_title; set { m_title = value; Refresh(); } }
    [Space]
    [SerializeField, FormerlySerializedAs("title")] string m_title;

    public string subtitle { get => m_subtitle; set { m_subtitle = value; Refresh(); } }
    [SerializeField, FormerlySerializedAs("title")] string m_subtitle;

    public TMP_Text titleText { get => m_titleText; set { m_titleText = value; Refresh(); } }
    [Space]
    [SerializeField, FormerlySerializedAs("titleText")] TMP_Text m_titleText;

    public TMP_Text subtitleText { get => m_subtitleText; set { m_subtitleText = value; Refresh(); } }
    [SerializeField, FormerlySerializedAs("titleText")] TMP_Text m_subtitleText;

    public Image shadow;

    bool isOpen;
    Color shadowColor;

    private void Awake()
    {
      shadowColor = shadow.color;

      Refresh();

      gameObject.SetActive(false);
    }

    public void SHOW()
    {
      if (!isOpen)
      {
        gameObject.SetActive(true);

        isOpen = true;

        m_titleText.color = Color.white;
        m_titleText.rectTransform.anchorMin = new Vector2(0, 0);
        m_titleText.rectTransform.anchorMax = new Vector2(1, 0);

        m_subtitleText.color = Color.white;
        m_subtitleText.rectTransform.anchorMin = new Vector2(0, 1);
        m_subtitleText.rectTransform.anchorMax = new Vector2(1, 1);

        shadow.color = Color.clear;

        shadow.DOColor(shadowColor, shadowInTime).SetEase(Ease.Linear).SetUpdate(true);

        m_titleText.rectTransform.DOAnchorMin(new Vector2(0, 1), titleInTime).SetUpdate(true);
        m_titleText.rectTransform.DOAnchorMax(new Vector2(1, 1), titleInTime).SetUpdate(true);

        m_subtitleText.rectTransform.DOAnchorMin(new Vector2(0, 0), subtitleInTime).SetDelay(subtitleDelay).SetUpdate(true);
        m_subtitleText.rectTransform.DOAnchorMax(new Vector2(1, 0), subtitleInTime).SetDelay(subtitleDelay).SetUpdate(true).OnComplete(() => StartCoroutine(Close()));
      }
    }

    IEnumerator Close()
    {
      yield return new WaitForSecondsRealtime(openTime);

      shadow.DOColor(shadowColor, shadowOutTime).SetEase(Ease.Linear).SetUpdate(true);

      m_titleText.DOColor(Color.clear, titleOutTime).SetDelay(titleDelay).SetUpdate(true).OnComplete(() => isOpen = false);

      m_subtitleText.DOColor(Color.clear, subtitleOutTime).SetUpdate(true).OnComplete(() => gameObject.SetActive(false));
    }

    public void Refresh()
    {
      m_titleText.text = m_title;
      m_subtitleText.text = m_subtitle;
    }
  }
}