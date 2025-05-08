using UnityEngine;

public class Phase1_CrushingWall : MonoBehaviour
{
    private bool alreadyCrushed = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (alreadyCrushed) return;

        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("플레이어 눌림!");
            // 플레이어 죽이기
            Player player = collision.collider.GetComponent<Player>();
            if (player != null)
            {
                player.stateMachine.ChangeState(player.deadState);
            }

            // 천장 멈추기
            Phase1_Manager manager = FindObjectOfType<Phase1_Manager>();
            if (manager != null)
            {
                manager.StopCeiling();  // Stop 함수 필요
            }

            alreadyCrushed = true;
        }
    }
}
