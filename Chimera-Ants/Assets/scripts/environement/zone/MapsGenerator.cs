using UnityEngine;

public class MapsGenerator: MonoBehaviour
{
    public float lacunarity, persistence, scale, heightmeshscale;
    public int octaves, width, height;
    public Biome[] biomes;
    public AnimationCurve heightCorrectionCurve;
    
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

    public void CreateMap()
    {
        MapRenderer maprenderer = gameObject.GetComponent<MapRenderer>();
        float[,] heightmap = generateHeightMap();
        Color[] colormap = generateColorMap(heightmap);
        Texture2D texture = maprenderer.generateTexture(colormap, width, height);
        Mesh mesh = generateMesh(heightmap);

        maprenderer.renderMesh(mesh, texture);
        maprenderer.meshfilter.gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;
    }
    

}
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

