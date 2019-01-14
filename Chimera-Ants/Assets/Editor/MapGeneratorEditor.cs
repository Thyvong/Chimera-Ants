using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapsGenerator))]
public class MapGeneratorEditor : Editor {

	public override void OnInspectorGUI()
    {
        MapsGenerator mapgen = (MapsGenerator)target;

        if (DrawDefaultInspector())
        {
            mapgen.CreateMap();
        }
        if(GUILayout.Button("Generate")){
            mapgen.CreateMap();
        }
    }
}
