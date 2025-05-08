using UnityEngine;
using UnityEngine.Playables;

public class ObjectTimelineTrigger : MonoBehaviour
{
    public PlayableDirector timelineDirector; // 타임라인 실행용
    public GameObject playerObject; // 플레이어 오브젝트 (애니메이터가 있는 오브젝트)

    // 충돌 시 타임라인 실행
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // 플레이어와 충돌했을 때
        {

            // 충돌체 (자기 자신)를 비활성화
            gameObject.SetActive(false);

            // 타임라인 실행
            timelineDirector.Play();

            // 타임라인 종료 후, 플레이어의 움직임을 복구하는 이벤트 설정
            timelineDirector.stopped += OnTimelineFinished;
        }
    }

    // 타임라인 종료 후 다시 플레이어 움직임 복구
    void OnTimelineFinished(PlayableDirector director)
    {
        // 이벤트 제거 (다시 호출되지 않도록)
        director.stopped -= OnTimelineFinished;
    }
}
