using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace GemTrader.Environment
{
    public class GridSystem : MonoBehaviour
    {
        [SerializeField] private int gridSizeX;
        [SerializeField] private int gridSizeY;
        [SerializeField] private float cellSize;
        [SerializeField] private BaseGem[] gemPrefabs;
    
        private GridCell[,] _grid; //grid matrix
    
        // A convenient struct to access every cells' properties in a grid 
        private struct GridCell
        {
            public bool HasGem;
            public BaseGem Gem;
        }
    
        private void Start()
        {
            CreateGrid();
        }

        private void CreateGrid()
        {
            _grid = new GridCell[gridSizeX, gridSizeY];
    
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    Vector3 cellPosition = new Vector3(x * cellSize, 0f, y * cellSize);
                    
                    GameObject cell = new GameObject("Cell")
                    {
                        //used object initializer to remove boilerplate code 
                        transform =
                        {
                            position = cellPosition,
                            localScale = new Vector3(cellSize, 0.1f, cellSize),
                            parent = transform
                        }
                    };

                    GameObject randomGemPrefab = gemPrefabs[Random.Range(0, gemPrefabs.Length)].gameObject;

                    GameObject gem = Instantiate(randomGemPrefab, cellPosition, Quaternion.identity);
                    gem.transform.parent = cell.transform;

                    GridCell gridCell;
                    gridCell.HasGem = true;
                    gridCell.Gem = gem.GetComponent<Gem>();
                    
                    _grid[x, y] = gridCell;
                }
            }
        }
    }
}

