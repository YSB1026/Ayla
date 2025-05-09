using UnityEngine;

public enum Chapter
{
    none, c1, c2, c3, c4, c5, c6
}
public class HouseManager : MonoBehaviour
{
    static public HouseManager Instance { get; private set; } 

    [Header("é��")]
    [SerializeField] Chapter chapter;

    [Header("����")]
    [SerializeField] GameObject basementPrisionTimeline;//house ����
    [SerializeField] GameObject basementKeyTimeline;//���迡 �پ��ִ� Ÿ�Ӷ���

    [Header("���϶� ��")]
    [SerializeField] GameObject aylaRoomGreenPendant;//���϶� �� �Ҵ�Ʈ
    [SerializeField] GameObject aylaRoomAfterTimelineTrigger;//green ��� �� �ڿ� alpha�� �����ϴ� trigger

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if(chapter == Chapter.none) chapter = Chapter.c1;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void SetActiveTimeLine(GameObject go, bool active)
    {
        if(go == null || go.activeSelf == active) return;
        
        go.SetActive(active);
    }

    //void ActiveFalse(GameObject go)
    //{
    //    if (go == null || !go.activeSelf) return;
    //    go.SetActive(false);
    //}
}
