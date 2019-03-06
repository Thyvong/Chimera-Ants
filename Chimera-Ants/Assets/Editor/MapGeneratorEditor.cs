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
            
        }
        if(GUILayout.Button("Generate")){
            mapgen.CreateMap();
        }
    }
}
