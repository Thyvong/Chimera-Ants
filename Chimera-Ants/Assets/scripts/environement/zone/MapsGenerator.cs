using System.Collections.Generic;
using UnityEngine;

public class MapsGenerator: MonoBehaviour
{
    /* Paramètre de génération du terrain*/
    public float lacunarity, persistence, scale, heightmeshscale;
    public int octaves, width, height;
    public Biome[] biomes;
    public AnimationCurve heightCorrectionCurve;

    /* Données générés */
    float[,] _heightMap;
    Color[] _colorMap;
    Texture2D _texture;
    Mesh _mesh;
    

    /* Paramètre de génération du décor */
    public GameObject treeModel;
    public Transform treeList; // object parent de tous les trees à placer ici
    List<int> treesLocationIndex = new List<int>();
    public GameObject rockModel;
    public Transform rockList; // object parent de tous les cailloux à placer ici
    List<int> rocksLocationIndex = new List<int>();


    /* Methode */
    public float[,] generateHeightMap()
    {
        float[,] map = new float[width, height];

        float min = float.MaxValue;
        float max = float.MinValue;
        if (scale <= 0)
        {
            scale = 0.0001f;
        }
        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                float amplitude = 1;
                float frequency = 1;
                float heightvalue = 0;

                for (int oct=0; oct< octaves; oct ++)
                {
                    float xnoise =  x / scale * frequency ;
                    float ynoise =  y / scale * frequency;
                    heightvalue += (Mathf.PerlinNoise(xnoise, ynoise) * 2 -1) * amplitude;

                    amplitude *= persistence;
                    frequency *= lacunarity;
                }
                map[x, y] = heightvalue;
                if (heightvalue < min)
                {
                    min = heightvalue;
                }else if (heightvalue > max)
                {
                    max = heightvalue;
                }
            }
        }
        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                map[x, y] = Mathf.InverseLerp(min, max, map[x, y]);
                
            }
        }
        return map;
    }

    public Color[] generateColorMap(float[,] heightmap)
    {
        Color[] colormap = new Color[width * height];
        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                for(int i=0; i< biomes.Length; i++)
                {
                    if (heightmap[x, y] <= biomes[i].height )
                    {
                        colormap[y*width +x] = biomes[i].color;
                        break;
                    }
                }
            }
        }
        return colormap;
    }

    public Mesh generateMesh(float[,] heightmap)
    {
        int width = heightmap.GetLength(0);
        int height = heightmap.GetLength(1);
        Mesh terrain = new Mesh();
        MeshData data = new MeshData(width,height);

        int index = 0;
        for(int x = 0; x < width; x++)
        {
            for(int y=0; y< height; y++)
            {
                data.vertices[index] = new Vector3( x, heightCorrectionCurve.Evaluate(heightmap[x,y]) * heightmeshscale, y );
                data.uvs[index] = new Vector2(x / (float)width, y / (float)height);
                if( x < width-1 && y < height-1)
                {
                    data.addTriangles(index, index + width + 1, index + width);
                    data.addTriangles(index + width + 1, index, index + 1);
                }
                index ++;
            }
        }

        terrain.vertices = data.vertices;
        terrain.uv = data.uvs;
        terrain.triangles = data.triangles;
        terrain.RecalculateNormals();
        
        return terrain;
    }

    public void GenerateRocks(float[,] heightmap, Mesh mesh)
    {
        System.Random rng = new System.Random();
        int nbRocks = rng.Next();

        int plainIndex = 0;
        for (int it = 0; it < biomes.Length; it++)
        {
            if (biomes[it].zoneType == TerritoryType.PLAINE)
            {
                plainIndex = it;
                break;
            }
            else
            {
                plainIndex = -1;
            }
        }
        if (plainIndex == -1)
            return;
        
        
        int index = rng.Next() % mesh.vertexCount;

        for (int i = 0; i < nbRocks; i++)
        {
            do
            {
                index = rng.Next() % mesh.vertexCount;
            } while (
                (heightmap[index / width, index % width] > biomes[plainIndex].height
                || heightmap[index / width, index % width] < biomes[plainIndex--].height)
                ||
                treesLocationIndex.Contains(index)
                || rocksLocationIndex.Contains(index)
            );
            // save pos as an index in an array. Don't forget to check if pos is not already occupied
            rocksLocationIndex.Add(index);
            Instantiate(rockModel, mesh.vertices[index], Quaternion.FromToRotation(Vector3.up, mesh.tangents[index]), rockList);
        }


    }
    public void GenerateVegetation(float[,] heightmap, Mesh mesh)
    {
        System.Random rng = new System.Random();
        int nbTrees = rng.Next();

        int plainIndex=0;
        for(int it = 0; it < biomes.Length; it++)
        {
            if(biomes[it].zoneType == TerritoryType.PLAINE)
            {
                plainIndex = it;
                break;
            }
            else
            {
                plainIndex = -1;
            }
        }
        if (plainIndex == -1)
            return;

        
        int index = rng.Next()%mesh.vertexCount;
        
        for (int i = 0; i < nbTrees; i++)
        {
            do
            {
                index = rng.Next() % mesh.vertexCount;
            } while (
                (heightmap[index / width, index % width] > biomes[plainIndex].height
                || heightmap[index / width, index % width] < biomes[plainIndex--].height)
                ||
                treesLocationIndex.Contains(index)
                ||rocksLocationIndex.Contains(index)
            );
            // save pos as an index in an array. Don't forget to check if pos is not already occupied
            treesLocationIndex.Add(index);
            Instantiate(treeModel, mesh.vertices[index], Quaternion.FromToRotation(Vector3.up, mesh.tangents[index]), treeList);
        }
        
    }

    public void CreateMap()
    {
        MapRenderer maprenderer = gameObject.GetComponent<MapRenderer>();
        _heightMap = generateHeightMap();
        _colorMap = generateColorMap(_heightMap);
        _texture = maprenderer.generateTexture(_colorMap, width, height);
        _mesh = generateMesh(_heightMap);

        maprenderer.renderMesh(_mesh, _texture);
        maprenderer.meshfilter.gameObject.GetComponent<MeshCollider>().sharedMesh = _mesh;
    }
    

}
/* Classe support à la création de mesh */
public class MeshData
{
    public Vector3[] vertices;
    public Vector2[] uvs;
    public int[] triangles;
    public int currenttrianglesindex;

    public MeshData(int width, int height)
    {
        vertices = new Vector3[width * height];
        triangles = new int[(width - 1) * (height - 1) * 6];
        uvs = new Vector2[width * height];
        currenttrianglesindex = 0;
    }
    public void addTriangles(int a, int b ,int c)
    {
        triangles[currenttrianglesindex] = a;
        triangles[currenttrianglesindex+1] = b;
        triangles[currenttrianglesindex+2] = c;
        currenttrianglesindex += 3;
    }
}

