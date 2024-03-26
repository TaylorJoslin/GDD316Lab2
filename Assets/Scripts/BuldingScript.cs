using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuldingScript : MonoBehaviour
{
     public int width = 10;
    public int height = 10;
    public GameObject wallPrefab;
    public GameObject windowPrefab;

    void Start()
    {
        GenerateBuilding();
    }

    void GenerateBuilding()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    Instantiate(wallPrefab, new Vector3(x+22, 5, y+65), Quaternion.identity);
                }
                else
                {
                    Instantiate(windowPrefab, new Vector3(x+22, 7, y+65), Quaternion.identity);
                }
            }
        }
    }
}
