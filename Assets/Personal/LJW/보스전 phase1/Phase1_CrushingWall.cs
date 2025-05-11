// using UnityEngine;

// public class Phase1_CrushingWall : MonoBehaviour
// {
//     private bool alreadyCrushed = false;

//     private void OnCollisionEnter2D(Collision2D collision)
//     {
//         if (alreadyCrushed) return;

//         if (collision.collider.CompareTag("Player"))
//         {
//             Debug.Log("�÷��̾� ����!");
//             // �÷��̾� ���̱�
//             Player player = collision.collider.GetComponent<Player>();
//             if (player != null)
//             {
//                 player.stateMachine.ChangeState(player.deadState);
//             }

//             // õ�� ���߱�
//             Phase1_Manager manager = FindObjectOfType<Phase1_Manager>();
//             if (manager != null)
//             {
//                 manager.StopCeiling();  // Stop �Լ� �ʿ�
//             }

//             alreadyCrushed = true;
//         }
//     }
// }
