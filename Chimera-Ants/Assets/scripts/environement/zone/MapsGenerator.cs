using System.Collections.Generic;
using UnityEngine;

public class MapsGenerator: MonoBehaviour
{
    /* Paramètre de génération du terrain*/
    public float lacunarity, persistence, scale, heightmeshscale, offsetx,offsety;
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

    public Transform speciesList;


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
                    heightvalue += (Mathf.PerlinNoise(xnoise+offsetx, ynoise+offsety) * 2 -1) * amplitude;

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
        int nbRocks = rng.Next() % 50;

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

        RaycastHit hit1;
        int index = rng.Next() % mesh.vertexCount;

        for (int i = 0; i < nbRocks; i++)
        {
            do
            {
                index = rng.Next() % mesh.vertexCount;

            } while (
                (heightmap[index / width, index % width] > biomes[plainIndex].height
                || heightmap[index / width, index % width] < biomes[plainIndex-1].height)
                ||
                treesLocationIndex.Contains(index)
                || rocksLocationIndex.Contains(index)
            );
            // save pos as an index in an array. Don't forget to check if pos is not already occupied
            rocksLocationIndex.Add(index);

            GameObject rock = Instantiate(rockModel, 20*mesh.vertices[index], Quaternion.FromToRotation(Vector3.up, mesh.normals[index]));
            if (Physics.Raycast(new Ray(rock.transform.position, new Vector3(0, -1000.0f, 0)), out hit1))
            {
                print("hit1");
                rock.transform.position = hit1.point;
            }

            float scalemultiply = (rng.Next() % 10.0f) * mesh.vertices[index].y;
            rock.transform.localScale = (scalemultiply!=0)?rock.transform.localScale * scalemultiply : rock.transform.localScale;
            rock.transform.parent = rockList;
        }


    }

    public void GenerateTreePatch(Vector3 pos, Quaternion rotation, int radius, float density)
    {
        System.Random rng = new System.Random();
        RaycastHit hit1;

        int nbTrees =  (int)(20.0 * density);
        for(int i=0; i< nbTrees; ++i)
        {
            GameObject tree = Instantiate(treeModel,   new Vector3(20.00f * (pos.x + rng.Next(-radius, radius)/1000.0f), 10 , 20.00f * (pos.z +rng.Next(-radius, radius) / 1000.0f) ) , rotation); // *50 pour correspondre au scale du terrain
            print(pos + " vs " + pos.y + " vs " + tree.transform.position.y);
            
            if (Physics.Raycast(new Ray(tree.transform.position, new Vector3(0,-1000.0f,0)), out hit1) )
            {
                print("hit1");
                tree.transform.position = hit1.point;
            }


            float scalemultiply =1;
            tree.transform.localScale = (scalemultiply != 0) ? tree.transform.localScale * scalemultiply : tree.transform.localScale;
            tree.transform.parent = treeList;
            
        }
        
    }
    public void GenerateVegetation(float[,] heightmap, Mesh mesh)
    {
        System.Random rng = new System.Random();
        int nbTrees = rng.Next() % 20;

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
                // adding + 0.01 prevent tree spawn too close to extreme slope part like water or mounts, whick looks weird
            } while (
                (heightmap[index / width, index % width] > biomes[plainIndex].height - 0.03f
                || heightmap[index / width, index % width] < biomes[plainIndex-1].height + 0.08f)
                ||
                treesLocationIndex.Contains(index)
                ||rocksLocationIndex.Contains(index)
            );
            // save pos as an index in an array. Don't forget to check if pos is not already occupied
            treesLocationIndex.Add(index);
            GenerateTreePatch(mesh.vertices[index], Quaternion.FromToRotation(Vector3.forward, mesh.normals[index]), 400, 2.0f);
        }


        
    }


    GameObject MakeCreature(Vector3 pos, int speciesId)
    {
        string source = "Prefabs/";
        switch (speciesId)
        {
            case 0:
                {
                    return (GameObject)Instantiate(Resources.Load(source + "AntQueen"), pos, new Quaternion());
                }
            case 1:
                {
                    return (GameObject)Instantiate(Resources.Load(source + "AntWorker"), pos, new Quaternion());
                }

            case 2:
                {
                    return (GameObject)Instantiate(Resources.Load(source + "AntSoldier"), pos, new Quaternion());
                }

            case 3:
                {
                    return (GameObject)Instantiate(Resources.Load(source + "Wolf"), pos, new Quaternion());
                }
            default:
                {
                    return (GameObject)Instantiate(Resources.Load(source + "Wolf"), pos, new Quaternion());
                }

        }
    }
    public void GenerateSpeciesPatch(Vector3 pos, int radius, float density)
    {
        int rng = Random.Range(0,5);
        bool isAnimal = false;
        bool isAnt = false;
        RaycastHit hit1;

        GameObject leader = MakeCreature(20.0f * (pos + new Vector3(Random.Range(-radius, radius) / 1000.0f, 0, Random.Range(-radius, radius) / 1000.0f)), rng);
        GameObject species;

        int speciesNb = (int)(10.0 * density);

        if (leader.GetComponent<Species>().GetType().IsSubclassOf(typeof(Animal)))
        {
            isAnimal = true;
            leader.GetComponent<Animal>().SetState(State.Leader);
            leader.GetComponent<Animal>().familyBoidId = Random.Range(0,100);
            if(leader.GetComponent<Species>().GetType() == typeof(ChimeraAnt))
            {
                isAnt = true;
                if(leader.GetComponent<ChimeraAnt>().status != ChimeraAntClass.King && leader.GetComponent<ChimeraAnt>().status != ChimeraAntClass.Queen)
                {
                    leader.GetComponent<Animal>().SetState(State.Follower);
                    speciesNb = 3;
                }
                else
                {
                    rng = 1; // one queen, rest are soldier/worker
                }
            }
        }
        
        
        for (int i = 0; i < speciesNb; ++i)
        {

            species = MakeCreature(20.0f * (pos+ new Vector3(Random.Range(-radius, radius) / 1000.0f, 0, Random.Range(-radius, radius) / 1000.0f)) , rng); 
            if (isAnimal)
            {
                species.GetComponent<Animal>().SetState(State.Follower);
                species.GetComponent<Animal>().familyBoidId = leader.GetComponent<Animal>().familyBoidId;
                if (isAnt)
                {
                    if(leader.GetComponent<ChimeraAnt>().status == ChimeraAntClass.Queen)
                    {
                        rng = Random.Range(1, 3);
                        species.GetComponent<ChimeraAnt>().queen = leader.GetComponent<ChimeraAnt>();
                    }
                    
                }
            }
            if (Physics.Raycast(new Ray(species.transform.position, new Vector3(0, -1000.0f, 0)), out hit1))
            {
                
                species.transform.position = hit1.point;
            }


            float scalemultiply = 1;
            species.transform.localScale = (scalemultiply != 0) ? species.transform.localScale * scalemultiply : species.transform.localScale;
            species.transform.parent = speciesList;
            species.GetComponent<SphereCollider>().enabled = false;
            species.GetComponent<SphereCollider>().enabled = true;
        }
        leader.GetComponent<SphereCollider>().enabled = false;
        leader.GetComponent<SphereCollider>().enabled = true;
        leader.transform.parent = speciesList;

    }
    public void GenerateRandomCreature(float[,] heightmap, Mesh mesh)
    {
        
        int creatureNb = Random.Range(5, 10);

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


        
        for (int i = 0; i < creatureNb; i++)
        {
            
            GenerateSpeciesPatch(mesh.vertices[(mesh.vertexCount / 2 + 17)] + new Vector3(Random.Range(-1.0f,1.0f),0, Random.Range(-1.0f, 1.0f)), 200, 2.0f);
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
    public void CreateVegetation()
    {
        GenerateVegetation(_heightMap, _mesh);
        
    }
    public void CreateRocks()
    {
        GenerateRocks(_heightMap, _mesh);

    }
    public void CreateCreature()
    {
        GenerateRandomCreature(_heightMap, _mesh);

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

