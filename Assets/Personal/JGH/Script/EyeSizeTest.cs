using UnityEngine;

public class EyeSizeTest : MonoBehaviour
{
    public Renderer targetRenderer;

    void Start()
    {
        Material mat = new Material(targetRenderer.sharedMaterial); // 인스턴스화
        targetRenderer.material = mat;

        // 테스트: 사이즈 0.3으로 설정
        mat.SetFloat("_Size", 0.3f);
    }
}
