using UnityEngine;

public enum Chapter
{
    none, c1, c2, c3, c4, c5, c6
}
public class HouseManager : MonoBehaviour
{
    static public HouseManager Instance { get; private set; } 

    [Header("챕터")]
    [SerializeField] Chapter chapter;

    [Header("지하")]
    [SerializeField] GameObject basementPrisionTimeline;//house 시작
    [SerializeField] GameObject basementKeyTimeline;//열쇠에 붙어있는 타임라인

    [Header("에일라 방")]
    [SerializeField] GameObject aylaRoomGreenPendant;//에일라 방 팬던트
    [SerializeField] GameObject aylaRoomAfterTimelineTrigger;//green 얻고 난 뒤에 alpha값 조정하는 trigger

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
