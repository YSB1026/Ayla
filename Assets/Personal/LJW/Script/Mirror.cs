using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Collider2D))]
public class Mirror : MonoBehaviour
{
    [Header("�ɼ�")]
    [Tooltip("���� ������ �ʿ��� �� üũ (Ư���� �޽��� �ݶ��̴����� ������ ������ ��)")]
    public bool invertNormal = false;
}
