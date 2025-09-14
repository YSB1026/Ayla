using UnityEngine;

public class FollowingMouse : MonoBehaviour
{
    void Update()
    {
        // 마우스 위치를 월드 좌표로 변환
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // 방향 벡터 (마우스 위치 - 오브젝트 위치)
        Vector2 direction = (mousePos - transform.position);

        // 각도 계산 (라디안 → 도)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 오브젝트 회전 (z축만)
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
