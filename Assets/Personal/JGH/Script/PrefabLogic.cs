using System.Collections;
using UnityEngine;

public class PrefabLogic : MonoBehaviour
{
    [Header("씬에 존재하는 오브젝트를 드래그해서 넣기")]
    public GameObject sprite;             // Hierarchy에서 드래그해서 넣기
    public ParticleSystem particles;      // 파티클도 드래그

    private void Start()
    {
        StartCoroutine(waitForIt());
    }

    IEnumerator waitForIt()
    {
        yield return new WaitForSeconds(0.5f);
        play();
    }

    void play()
    {
        if (sprite != null)
            Destroy(sprite); // 오브젝트 제거

        if (particles != null)
            particles.Emit(9999); // 파티클 재생
    }
}
