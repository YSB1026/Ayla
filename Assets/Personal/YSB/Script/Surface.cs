using UnityEngine;

public enum SurfaceType
{
    None,
    Forest, //1��������
    Wood, // �� ���� ���� �ٴڵ�?
    Stair, // ��ܵ� Ȥ�� ������ �� ������ 
    Stone // ���Ͻ�?
}
public class Surface : MonoBehaviour
{
    public SurfaceType surfaceType;
}
