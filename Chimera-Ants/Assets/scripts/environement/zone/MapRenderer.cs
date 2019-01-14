using UnityEngine;

public class MapRenderer : MonoBehaviour
{
    public Renderer texturerenderer;
    public MeshFilter meshfilter;
    public MeshRenderer meshrenderer;
    

    public Texture2D generateTexture(Color[] colormap, int width, int height)
    {
        Texture2D texture = new Texture2D(width, height);
        texture.SetPixels(colormap);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.Apply();
        return texture;
    }
    public void renderMesh(Mesh mesh, Texture2D texture)
    {
        meshfilter.sharedMesh = mesh;
        meshrenderer.sharedMaterial.mainTexture = texture;
        meshrenderer.material.mainTexture = texture;
        //meshrenderer.transform.localScale = new Vector3( texture.width,1, texture.height);

    }
}
