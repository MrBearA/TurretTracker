using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapTowerPlacement : MonoBehaviour
{
    public Tilemap towerPlacementTilemap; // Assign your Tilemap in Inspector
    public GameObject towerPrefab;
    private TowerManager towerManager;

    void Start()
    {
        towerManager = FindFirstObjectByType<TowerManager>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left Click
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPosition = towerPlacementTilemap.WorldToCell(mouseWorldPos);

            if (IsValidPlacement(cellPosition)) // Check if it's a valid tile
            {
                PlaceTower(cellPosition);
            }
            else
            {
                Debug.Log("Invalid placement: Outside tilemap or already occupied!");
            }
        }
    }

    bool IsValidPlacement(Vector3Int cellPosition)
    {
        TileBase tile = towerPlacementTilemap.GetTile(cellPosition);
        if (tile == null) return false; // No tile at this position

        Collider2D existingTower = Physics2D.OverlapPoint(towerPlacementTilemap.GetCellCenterWorld(cellPosition));
        return existingTower == null; // Return true only if no tower is there
    }

    void PlaceTower(Vector3Int cellPosition)
    {
        if (towerManager.SpendGold(100)) // Check if player has enough gold
        {
            Vector3 towerWorldPos = towerPlacementTilemap.GetCellCenterWorld(cellPosition);
            Instantiate(towerPrefab, towerWorldPos, Quaternion.identity);
            Debug.Log("Tower placed at: " + cellPosition);
        }
        else
        {
            Debug.Log("Not enough gold to place a tower!");
        }
    }
}
