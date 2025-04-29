using UnityEngine;

public enum SurfaceType
{
    None,
    Forest, //1스테이지
    Wood, // 그 이후 나무 바닥들?
    Stair, // 계단도 혹시 따로할 수 있으면 
    Stone // 지하실?
}
public class Surface : MonoBehaviour
{
    public SurfaceType surfaceType;
}
