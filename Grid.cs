using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSetup : MonoBehaviour
{
    [Header("Art")]
    [SerializeField] private Material tileMaterial;


    [Header("Values")]
    [SerializeField] private GameObject[] targetBoxes;
    [SerializeField] private float tileSize = 1f;
    [SerializeField] private float zOffset = 1f; //Spawning a piece on the board just upon it or how much above it
      


    private GameObject[,] tiles;
    private Vector3 bounds; //how far the extent of our board goes
    private const int WIDTH = 10;  //TileCountX
    private const int HEIGHT = 12;  //TileCountY
   


    private void Awake()
    {
        GenerateAllTiles(tileSize, WIDTH, HEIGHT);
       
       
        
    }

    private void Start()
    {
        SpawnAllItems();

    }
    

    //Generate The Board
    private void GenerateAllTiles(float tileSize, int tileCountX, int tileCountY)
    {
        zOffset += transform.position.z;  //setting the zOffset to just above the board's depth

        //Below line is used if you want to set the board grid to a different board and align it properly
        //bounds = new Vector3((tileCountX / 2) * tileSize, (tileCountY / 2) * tileSize, 0f) + boardCenter;
        bounds = Vector3.zero; //since we dont need an offset for board here


        tiles = new GameObject[tileCountX, tileCountY];
        for(int x = 0; x < tileCountX; x++)
            for(int  y = 0; y < tileCountY; y++)
                tiles[x,y] = GenerateSingleTile(tileSize, x, y);
            
       
    }
    private GameObject GenerateSingleTile(float tileSize, int x, int y)
    {
        //naming convention
        GameObject tileObject = new GameObject(string.Format("X:{0}, Y:{0}", x, y));
        tileObject.transform.parent = transform;
        

        //Adding custom mesh and mesh references if not importing through a prefab
        Mesh mesh = new Mesh();  
        tileObject.AddComponent<MeshFilter>().mesh = mesh;
        tileObject.AddComponent<MeshRenderer>().material = tileMaterial;

        //vertices of a single tile
        Vector3[] vertices = new Vector3[4];
        vertices[0] = new Vector3(x * tileSize, y * tileSize, 0 ) - bounds;
        vertices[1] = new Vector3(x * tileSize, (y + 1) * tileSize, 0) - bounds;
        vertices[2] = new Vector3((x + 1) * tileSize, y * tileSize, 0 ) - bounds;
        vertices[3] = new Vector3((x + 1) * tileSize, (y + 1) * tileSize, 0) - bounds;

        //since a mesh is made up of 2 triangles (polygon rendering)
        int[] tris = new int[] { 0, 1, 2, 1, 3, 2 }; // 0 1 2 are indices of 1st triangle
                                                     // 1 3 2 are indices of 2nd triangle

        mesh.vertices = vertices;
        mesh.triangles = tris;

        //So the material appears directly facing the tile and behaves normally
        mesh.RecalculateNormals();

        //Adds a collider
        tileObject.AddComponent<BoxCollider>();

       

        return tileObject;
    }

    //Spawning Items
    private void SpawnAllItems()
    {
        for (int x = 0; x < WIDTH; x++)
        {
            for (int y = 0; y < HEIGHT; y++)
            {
                // Spawn an item at the grid position (x, y)
                
                SpawnSingleItem(GridItemType.Red, x, y);
            }
        }
    }

    private GridItem SpawnSingleItem(GridItemType type, int x, int y)
    {
        Vector3 position = new Vector3(x * tileSize, y * tileSize, 0);
        // Determine the correct prefab based on the GridItemType
        GameObject prefabToSpawn = GetPrefabForType(type);

        // Instantiate the prefab at a specific position
        
        GameObject newItemObject = Instantiate(prefabToSpawn, position, Quaternion.identity);

       
        return newItemObject.GetComponent<GridItem>();

    }

    private GameObject GetPrefabForType(GridItemType type)
    {
        switch (type)
        {
            case GridItemType.Red:
                return targetBoxes[0]; // First prefab in the array for Red
            case GridItemType.Pink:
                return targetBoxes[1]; // Second prefab in the array for Pink
            case GridItemType.Green:
                return targetBoxes[2]; // Third prefab in the array for Green
            case GridItemType.Yellow:
                return targetBoxes[3]; // Fourth prefab in the array for Yellow
            default:
                Debug.LogError("Unknown GridItemType");
                return null;
        }
    }

}




