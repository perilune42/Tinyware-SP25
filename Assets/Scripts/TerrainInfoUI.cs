using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TerrainInfoUI : MonoBehaviour
{
    [SerializeField] TMP_Text tileTypeText;
    [SerializeField] TMP_Text descText;
    [SerializeField] Image tileImage;

    GameObject container;

    public static TerrainInfoUI Instance;

    private void Awake()
    {
        Instance = this;
        container = transform.GetChild(0).gameObject;
        container.SetActive(false);
    }

    public void ShowTile(GridTile tile)
    {
        container.SetActive(true);
        tileImage.color = Color.white;
        tileImage.sprite = tile.TileSprite;
        switch (tile.Type)
        {
            case TileType.Normal:
                tileTypeText.text = "FIELD";
                descText.text = "No special effect.";
                break;
            case TileType.Chasm:
                tileTypeText.text = "CHASM";
                descText.text = "Ground units are destroyed upon entering this tile.";
                break;
            case TileType.Mountain:
                tileTypeText.text = "MOUNTAINS";
                descText.text = "Units cannot be pushed into this tile.";
                break;
        }
    }

    public void ShowBoundaryTile(BoundaryTile bTile)
    {
        container.SetActive(true);
        tileImage.color = bTile.SpriteColor;
        tileImage.sprite = bTile.TileSprite;
        if (bTile.IsLethal)
        {
            tileTypeText.text = "BORDER (LETHAL)";
            descText.text = "All units are destroyed upon entering this tile.";
        }
        else
        {
            tileTypeText.text = "BORDER";
            descText.text = "Units cannot be pushed into this tile.";
        }

    }

    public void Hide()
    {
        container.SetActive(false);
    }
}
