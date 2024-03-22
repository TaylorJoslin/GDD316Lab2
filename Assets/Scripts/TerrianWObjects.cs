using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TerrainWObj : MonoBehaviour
{
    int xWidth;
    int zWidth;
    float xOrg;
    float zOrg;
    private Mesh mesh;
    private Vector3[] vertices;
    private MeshCollider meshCollider;

    float scale = 4.0F; //scale for the noise
    float heightScale = 5.0f; //scale for height

    private Texture2D noiseTex;
    private Color[] pix;
    private Renderer rend;
    private MeshFilter myMeshFilter;
    private float[,] heigthMap;

    float[] hTerrain = new float[] { 1.0f, 0.6f, 0.3f };//high to low
    Color[] cTerrain = new Color[] { Color.gray, Color.green, Color.blue };

    [SerializeField]
    private GameObject thingPrefab; //object to be placed on the terrain

    void Start()
    {
        xWidth = 40;
        zWidth = xWidth;
        xOrg = zOrg = 0;
        Generate(); //generate the tile using basic mesh and vertices

        rend = GetComponent<Renderer>();
        // Set up the height map to hold the y positions of the mesh
        heigthMap = new float[xWidth + 1, zWidth + 1];
        //Set up the color array to color the terriain
        pix = new Color[(xWidth + 1) * (zWidth + 1)];
        // Set up the texture map
        noiseTex = new Texture2D(xWidth + 1, zWidth + 1);
        rend.material.mainTexture = noiseTex;

        //Calculate the heightMap and colormap pix
        CalcNoise();

        //Update the tile mesh with the heightMap info       
        DeformMyGrid();

        noiseTex.SetPixels(pix);
        noiseTex.Apply();
        mesh.RecalculateTangents();

        //Add the preforms to the terrain
        AddThings();

    }

    void CalcNoise()
    {
        // For each pixel in the texture, sample the Perlin noise
        // Generate a color adn heigth map
        for (int i = 0, z = 0; z <= zWidth; z++)
        {
            for (int x = 0; x <= xWidth; x++, i++)
            {
                float xCoord = xOrg + (float)x / xWidth * scale;
                float zCoord = zOrg + (float)z / zWidth * scale;
                //use simple PerlinNoise, but can extend to multiple scale noise using MyTileGeneration script appraoch
                float sample = Mathf.PerlinNoise(xCoord, zCoord);
                heigthMap[x, z] = GetMyHeight(sample);
                pix[i] = GetMyColor(sample);

            }
        }
    }
    void AddThings()
    {
        //get the size of the thing
        Vector3 aThingSize = thingPrefab.GetComponent<MeshRenderer>().bounds.size;
        //float athingWidth = aThingSize.x;
        //float athingDepth = aThingSize.z;
        float athingHeight = aThingSize.y;

        //get the mesh
        vertices = new Vector3[(xWidth + 1) * (zWidth + 1)];
        vertices = mesh.vertices;

        //set up position for placing the thing
        Vector3 thisPos;
        //iterate through the mesh
        for (int i = 0, z = 0; z <= zWidth; z++)
        {
            for (int x = 0; x <= xWidth; x++, i++)
            {
                //use the color map to filter the terrain
                if (noiseTex.GetPixel(x, z) == Color.green) //only add things in the green territory
                {
                    if (Random.value < 0.5f) //add things randomly
                    {
                        thisPos = vertices[i]; //the current mesh position - you can shift randomly from here if you want
                        //instantiate a thing at thisPos
                        GameObject thing = Instantiate(thingPrefab, thisPos, Quaternion.identity);
                        thing.transform.localScale = 0.5f * Vector3.one;//rescale the prefab
                        thing.transform.position = vertices[i] + 0.45f * Vector3.up * athingHeight / 2;
                        //notice that when the terrain is steep, the base of their is a gap under the object on the down-hill side
                    }
                }
            }
        }
    }

    Color GetMyColor(float sample)
    {
        Color colorNow = new Color(0, 0, 0);
        int nTerrains = hTerrain.Length;
        for (int i = 0; i < nTerrains; i++)
        {
            if (sample < hTerrain[i])
            {
                colorNow = cTerrain[i];
            }

        }
        return colorNow;
    }

    float GetMyHeight(float sample)
    {
        float heightNow = 0;
        int nterrains = hTerrain.Length;
        for (int i = 0; i < nterrains - 1; i++)
        {
            if (sample < hTerrain[i])
            {
                heightNow = heightScale * sample;
            }
        }
        if (sample < hTerrain[nterrains - 1])
        {
            heightNow = heightScale * hTerrain[nterrains - 1];
        }
        return heightNow;
    }


    void DeformMyGrid()
    {
        vertices = mesh.vertices;
        float y = 0;
        for (int i = 0, z = 0; z <= zWidth; z++)
        {
            for (int x = 0; x <= xWidth; x++, i++)
            {
                vertices[i].y = 0;
                y = heigthMap[x, z];
                vertices[i] += new Vector3(0, y, 0);
            }
        }
        mesh.vertices = vertices;
        mesh.RecalculateNormals();
    }

    private void Generate()
    {
        int xSize = xWidth;
        int zSize = zWidth;
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Procedural Grid";

        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        Vector2[] uv = new Vector2[vertices.Length];
        Vector4[] tangents = new Vector4[vertices.Length];
        Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);
        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                vertices[i] = new Vector3(x, 0, z);
                uv[i] = new Vector2((float)x / xSize, (float)z / zSize);
                tangents[i] = tangent;
            }
        }
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.tangents = tangents;

        int[] triangles = new int[xSize * zSize * 6];
        for (int ti = 0, vi = 0, z = 0; z < zSize; z++, vi++)
        {
            for (int x = 0; x < xSize; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
                triangles[ti + 5] = vi + xSize + 2;
                mesh.triangles = triangles;
            }
        }
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}
