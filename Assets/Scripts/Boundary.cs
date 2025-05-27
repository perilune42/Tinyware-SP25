using UnityEngine;
using UnityEngine.UI;

public class Boundary : MonoBehaviour
{
    [SerializeField] float lethalChance = 0.2f;

    private void Start()
    {
        GridLayoutGroup glg = GetComponent<GridLayoutGroup>();
        LayoutRebuilder.ForceRebuildLayoutImmediate(glg.GetComponent<RectTransform>());
        glg.enabled = false;
        InitTiles();
    }

    private void InitTiles()
    {
        BoundaryTile[] foundTiles = GetComponentsInChildren<BoundaryTile>();
        for (int y = -1; y < GameGrid.gridHeight + 1; y++)
        {
            for (int x = -1; x < GameGrid.gridWidth + 1; x++)
            {
                BoundaryTile tile = foundTiles[(x+1) + (y+1) * (GameGrid.gridWidth + 2)];
                Vector2Int pos = new Vector2Int(x, y);
                // get rid of tiles within play area as they are not needed
                if (GameGrid.IsValidPos(pos))
                {
                    Destroy(tile.gameObject);
                }
                else
                {
                    tile.Init(pos);
                    if (Random.value  < lethalChance)
                    {
                        tile.SetLethal();
                    }
                    GameGrid.Instance.AddBoundaryTile(tile, pos);
                }
            }

        }
    }
}
