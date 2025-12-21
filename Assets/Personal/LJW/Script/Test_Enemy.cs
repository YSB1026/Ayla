using UnityEngine;

public class Test_Enemy : MonoBehaviour, ISlowable
{
    public float speed = 5f;
    private float slowFactor = 1f; // 1이면 정상 속도

    void Update()
    {
        // 묻지도 따지지도 않고 오른쪽으로만 갑니다.
        // Blue 능력을 맞으면 slowFactor가 작아져서 느려집니다.
        transform.Translate(Vector3.right * speed * slowFactor * Time.deltaTime);
    }

    // BlueLightReactiveObject가 이 함수를 호출해서 속도를 조절합니다.
    public void SetSlowFactor(float multiplier)
    {
        slowFactor = multiplier;
    }
}
