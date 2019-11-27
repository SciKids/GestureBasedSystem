using UnityEngine;
using UnityEngine.UI;

public class DrawColorFrame : MonoBehaviour
{
    Texture2D colorTexture = null;
    public Material ImageMaterial;
    public Image Background;

    int cols = 0;
    int rows = 0;
    byte[] outSegment;

    void Start()
    {
        NuitrackManager.onColorUpdate += DrawColor;

        nuitrack.OutputMode mode = NuitrackManager.ColorSensor.GetOutputMode();
        cols = mode.XRes;
        rows = mode.YRes;

        RecreateTextures();
    }

    void DrawColor(nuitrack.ColorFrame frame)
    {
        for (int i = 0; i < (cols * rows); i++)
        {
            int ptr = i * 3;
            outSegment[ptr] = frame[i].Red;
            outSegment[ptr + 1] = frame[i].Green;
            outSegment[ptr + 2] = frame[i].Blue;
        }
        
        colorTexture.LoadRawTextureData(outSegment);
        colorTexture.Apply(false);
    }

    void RecreateTextures()
    {
        outSegment = new byte[cols * rows * 4];

        if (colorTexture != null)
        {
            Destroy(colorTexture);
        }

        colorTexture = new Texture2D(cols, rows, TextureFormat.RGB24, false);
        colorTexture.filterMode = FilterMode.Bilinear;
        colorTexture.Apply();
        ImageMaterial.mainTexture = colorTexture;
        Background.material = ImageMaterial;
    }
}
