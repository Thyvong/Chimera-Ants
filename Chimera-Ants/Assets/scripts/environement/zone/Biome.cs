using UnityEngine;
using UnityEditor;

public enum TerritoryType { NONE, FORET, PLAINE, STEPPES, MONTAGNES, TOUNDRA, DESERT, JUNGLE };
[System.Serializable]
public class Biome
{
    public TerritoryType zoneType;
    private int moisture;
    private float temperature;
    public float height;
    public Color color;
}