using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapsGenerator))]
public class MapGeneratorEditor : Editor {

	public override void OnInspectorGUI()
    {
        MapsGenerator mapgen = (MapsGenerator)target;
        
        // for rendering upon changing any parameters. Can get annoying with heavy object generation
        if (DrawDefaultInspector())
        {
            mapgen.CreateMap();
        }
        if(GUILayout.Button("GenerateMap")){
            mapgen.CreateMap();
        }
        if (GUILayout.Button("GenerateTrees"))
        {
            mapgen.CreateVegetation();
        }
        if (GUILayout.Button("GenerateRocks"))
        {
            mapgen.CreateRocks();
        }
        if (GUILayout.Button("GenerateCreatures"))
        {
            mapgen.CreateCreature();
        }
    }
}
