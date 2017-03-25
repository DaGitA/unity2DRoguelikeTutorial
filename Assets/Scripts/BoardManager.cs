using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{

    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int minimum, int maximum)
        {
            this.minimum = minimum;
            this.maximum = maximum;
        }

    }

    public int rows = 8;
    public int columns = 8;
    public Count wallCount = new Count(5, 9);
    public Count foodCount = new Count(1, 5);
    public GameObject exit;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] ennemyTiles;
    public GameObject[] outerWallTiles;

    private Transform hierarchyBoardHolder;
    private List<Vector3> grid = new List<Vector3>();

    private void InitializeList()
    {
        grid.Clear();
        for (int x = 1; x < columns - 1; x++)
        {
            for (int y = 1; y < rows - 1; y++)
            {
                grid.Add(new Vector3(x, y, 0f));
            }
        }
    }

    private void BoardSetup()
    {
        hierarchyBoardHolder = new GameObject("Board").transform;

        for (int x = -1; x < columns + 1; x++)
        {
            for (int y = -1; y < rows + 1; y++)
            {
                GameObject toInstantiate;
                if (isOuterWallPosition(x, y))
                {
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                }
                else
                {
                    toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                }

                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                instance.transform.SetParent(hierarchyBoardHolder);
            }
        }
    }

    private bool isOuterWallPosition(int x, int y)
    {
        return x == -1 || x == columns || y == -1 || y == rows;
    }

    private Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, grid.Count);
        Vector3 randomPosition = grid[randomIndex];
        grid.RemoveAt(randomIndex);
        return randomPosition;
    }

    private void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        int objectCount = Random.Range(minimum, maximum + 1);

        for(int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }

    public void SetupScene(int level)
    {
        BoardSetup();
        InitializeList();
        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
        LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);
        int ennemyCount = (int)Mathf.Log(level, 2F);
        LayoutObjectAtRandom(ennemyTiles, ennemyCount, ennemyCount);
        Instantiate(exit, new Vector3(columns-1, rows-1, 0f), Quaternion.identity);
    }
}
