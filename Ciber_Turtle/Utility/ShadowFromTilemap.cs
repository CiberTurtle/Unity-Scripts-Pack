using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering;

[AddComponentMenu("Rendering/Shadow From Tilemap"), RequireComponent(typeof(TilemapCollider2D), typeof(ShadowCaster2D))]
public class ShadowFromTilemap : MonoBehaviour
{
  ShadowCaster2D shadow;
  TilemapRenderer tilemap;

  private void Awake()
  {

  }
}
