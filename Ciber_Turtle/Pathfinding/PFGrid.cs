#pragma warning disable 649
using System.Collections.Generic;
using UnityEngine;

namespace Ciber_Turtle.Pathfinding
{
	using Ciber_Turtle.IO;

	[AddComponentMenu("Ciber_Turtle/Pathfinding/Grid"), DisallowMultipleComponent]
	public class PFGrid : MonoBehaviour
	{
		[System.Serializable]
		struct Debug
		{
			public bool enableGizmos;
			[Min(0)] public float nodeSpacing;
			public Color gridBorderColor;
			public bool enableWalkableNodes;
			public Color nodeWalkableColor;
			public Color nodeUnwalkableColor;
			public Color nodePathColor;
			public Color referanceColor;
			public bool enableReferanceGuides;
		}

		[SerializeField] Vector2 gridWorldSize;
		[SerializeField, Min(0)] float nodeRadius;
		[SerializeField, Min(0)] float checkRadius;
		[SerializeField] LayerMask unwalkableMask;
		[Space]
		public string savePath;

		[SerializeField] Debug debug;

		public PFNode[,] grid;
		float nodeDiameter;
		Vector2Int gridSize;

		private void Awake()
		{
			CreateGrid();
		}

		[ContextMenu("Create Grid")]
		public void CreateGrid()
		{
			nodeDiameter = nodeRadius * 2;
			gridSize = new Vector2Int(Mathf.RoundToInt(gridWorldSize.x / nodeDiameter), Mathf.RoundToInt(gridWorldSize.y / nodeDiameter));

			grid = new PFNode[gridSize.x, gridSize.y];
			Vector2 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;

			for (int x = 0; x < gridSize.x; x++)
			{
				for (int y = 0; y < gridSize.y; y++)
				{
					Vector3 worldPoint = worldBottomLeft + Vector2.right * (x * nodeDiameter + nodeRadius) + Vector2.up * (y * nodeDiameter + nodeRadius);
					bool walkable = !(Physics2D.OverlapCircle(worldPoint, checkRadius, unwalkableMask));
					grid[x, y] = new PFNode(walkable, worldPoint, new Vector2Int(x, y));
				}
			}
		}

		[ContextMenu("Clear Grid")]
		public void ClearGrid()
		{
			grid = null;
			gridSize = Vector2Int.zero;
			nodeDiameter = 0;
		}

		[ContextMenu("Save")]
		public void Save()
		{
			IO.WriteJson(savePath, grid, false);
		}

		[ContextMenu("Load")]
		public void Load()
		{
			grid = IO.ReadJson<PFNode[,]>(savePath, false);
		}

		public List<PFNode> GetNeighbours(PFNode node)
		{
			List<PFNode> neighbours = new List<PFNode>();

			for (int x = -1; x <= 1; x++)
			{
				for (int y = -1; y <= 1; y++)
				{
					if (x == 0 && y == 0)
						continue;

					Vector2Int check = new Vector2Int(node.gridPos.x + x, node.gridPos.y + y);

					if (check.x >= 0 && check.x < gridSize.x && check.y >= 0 && check.y < gridSize.y)
					{
						neighbours.Add(grid[check.x, check.y]);
					}
				}
			}

			return neighbours;
		}

		public PFNode NodeFromWorldPoint(Vector2 worldPosition)
		{
			Vector2 percent = new Vector2((worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x, (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y);
			percent.x = Mathf.Clamp01(percent.x);
			percent.y = Mathf.Clamp01(percent.y);

			return grid[Mathf.RoundToInt((gridSize.x - 1) * percent.x), Mathf.RoundToInt((gridSize.y - 1) * percent.y)];
		}

		public List<PFNode> path;
		void OnDrawGizmos()
		{
			if (debug.enableGizmos)
			{
				Gizmos.color = debug.gridBorderColor;
				Gizmos.DrawWireCube(transform.position, gridWorldSize);

				if (grid != null)
				{
					foreach (PFNode n in grid)
					{
						if (!n.walkable)
						{
							Gizmos.color = debug.nodeUnwalkableColor;
							Gizmos.DrawCube(n.worldPos, Vector3.one * (nodeDiameter - debug.nodeSpacing / 2));
						}
						else if (path != null)
						{
							if (path.Contains(n))
							{
								Gizmos.color = debug.nodePathColor;
								Gizmos.DrawCube(n.worldPos, Vector3.one * (nodeDiameter - debug.nodeSpacing / 2));
							}
						}
						else if (debug.enableWalkableNodes)
						{
							Gizmos.color = debug.nodeWalkableColor;
							Gizmos.DrawCube(n.worldPos, Vector3.one * (nodeDiameter - debug.nodeSpacing / 2));
						}
					}
				}
			}
		}

		private void OnDrawGizmosSelected()
		{
			if (debug.enableGizmos)
			{
				if (debug.enableReferanceGuides)
				{
					Gizmos.color = debug.referanceColor;
					Gizmos.DrawWireCube(transform.position, Util.ToVector2(nodeRadius * 2));
					Gizmos.DrawWireSphere(transform.position, checkRadius);
				}
			}
		}
	}
}