using UnityEngine;

public class UnloadScene : BaseTrigger //클래스 이름 바꾸기
{
    [SerializeField] bool isLobby = false;
    public void UnloadMemoryScene()
    {
        CustomSceneManager.Instance.UnloadAdditiveScene();
    }

    public void LoadLobbyScene()
    {
        CustomSceneManager.Instance.LoadScene("Lobby");
    }

    public void LoadEndingScene()
    {
        CustomSceneManager.Instance.LoadScene("Ending");
    }

    protected override void OnPlayerEnter()//얘가 왜 필요햇는지 기억이안나요..
    {
        UnloadMemoryScene();
    }
}
