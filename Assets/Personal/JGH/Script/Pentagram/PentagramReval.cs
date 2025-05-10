using UnityEngine;

public class PentagramReveal : MonoBehaviour
{
    public Material mat;
    public float speed = 0.5f;
    private float progress = 0f;

    void Update()
    {
        if (progress < 1f)
        {
            progress += Time.deltaTime * speed;
            mat.SetFloat("_Progress", progress);
        }
    }
}
