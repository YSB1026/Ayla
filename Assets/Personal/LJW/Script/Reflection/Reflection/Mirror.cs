using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Collider2D))]
public class Mirror : MonoBehaviour
{
    [Header("옵션")]
    [Tooltip("법선 반전이 필요할 때 체크 (특수한 메쉬나 콜라이더에서 방향이 뒤집힐 때)")]
    public bool invertNormal = false;
}
