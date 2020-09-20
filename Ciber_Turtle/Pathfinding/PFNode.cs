using UnityEngine;

namespace Ciber_Turtle.Pathfinding
{
	[System.Serializable]
	public class PFNode
	{
		public bool walkable;
		public Vector2 worldPos;
		public Vector2Int gridPos;

		public int gCost;
		public int hCost;
		public int fCost { get => gCost + hCost; }

		public PFNode(bool _walkable, Vector3 _worldPos, Vector2Int _gridPos)
		{
			walkable = _walkable;
			worldPos = _worldPos;
			gridPos = _gridPos;
		}

		public int CompareTo(PFNode nodeToCompare)
		{
			int compare = fCost.CompareTo(nodeToCompare.fCost);
			if (compare == 0)
			{
				compare = hCost.CompareTo(nodeToCompare.hCost);
			}
			return -compare;
		}
	}
}