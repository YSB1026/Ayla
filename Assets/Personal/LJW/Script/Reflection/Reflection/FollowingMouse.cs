using UnityEngine;

public class FollowingMouse : MonoBehaviour
{
    void Update()
    {
        // ���콺 ��ġ�� ���� ��ǥ�� ��ȯ
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // ���� ���� (���콺 ��ġ - ������Ʈ ��ġ)
        Vector2 direction = (mousePos - transform.position);

        // ���� ��� (���� �� ��)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // ������Ʈ ȸ�� (z�ุ)
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
