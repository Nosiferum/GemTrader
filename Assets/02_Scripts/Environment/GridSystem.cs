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
    
        private GameObject[,] _grid; //grid matrix
        
        private void Awake()
        {
            CreateGrid();
        }

        private void CreateGrid()
        {
            _grid = new GameObject[gridSizeX, gridSizeY];
    
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
                            parent = transform
                        }
                    };
                    
                    _grid[x, y] = cell;

                    CreateGem(x, y);
                }
            }
        }

        private void CreateGem(int x, int y)
        {
            GameObject randomGemPrefab = gemPrefabs[Random.Range(0, gemPrefabs.Length)].gameObject;

            GameObject gem = Instantiate(randomGemPrefab, _grid[x, y].transform.position, Quaternion.identity);
            gem.transform.parent = _grid[x, y].transform;

            BaseGem baseGem = gem.GetComponent<BaseGem>();

            baseGem.CellCoordinateX = x;
            baseGem.CellCoordinateY = y;

            gem.GetComponent<MeshFilter>().sharedMesh = baseGem.Model;
            //gem.transform.localScale = Vector3.zero;
            
        }

        
        public void RemoveAndRespawnGem(BaseGem gem, int x, int y)
        {
           
            Destroy(gem.gameObject);
            CreateGem(x, y);
        }
    }
}

