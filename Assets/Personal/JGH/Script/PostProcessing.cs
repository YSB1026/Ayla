using UnityEngine;

[ExecuteInEditMode]
public class WavyScreenEffect : MonoBehaviour
{
    public Material effectMaterial;

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (effectMaterial != null)
        {
            effectMaterial.SetFloat("_Time", Time.time);
            Graphics.Blit(src, dest, effectMaterial);
        }
        else
        {
            Graphics.Blit(src, dest); // fallback
        }
    }
}
