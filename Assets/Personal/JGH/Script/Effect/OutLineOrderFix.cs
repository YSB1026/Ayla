using UnityEngine;

public class OutlineOrderFix : MonoBehaviour
{
    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null && renderer.material != null)
        {
            renderer.material.renderQueue = 3001; // 배경보다 위
        }
    }
}
