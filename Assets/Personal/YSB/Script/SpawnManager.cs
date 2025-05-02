using UnityEngine;

public enum PlayerType
{
    FOREST,
    HOUSE
}

public enum EnemyType
{
    
}

public class SpawnManager : MonoBehaviour
{
    private static SpawnManager instance;

    public static SpawnManager Instance { get { return instance; } }

    [SerializeField] private GameObject forestPlayerPrefab;
    [SerializeField] private GameObject housePlayerPrefab;
    [SerializeField] private GameObject bossPrefab;

	private void Awake()
	{
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
	}

    public GameObject PlayerSpawn(Vector3 position , PlayerType playerType)
    {
        GameObject go = new GameObject();

		switch (playerType)
        {
            case PlayerType.FOREST:
                go = Instantiate(forestPlayerPrefab, position, Quaternion.identity);
                break;
            case PlayerType.HOUSE:
				go = Instantiate(housePlayerPrefab, position, Quaternion.identity);
                break;
        }

        return go;
    }

    public GameObject BossSpawn(Vector3 position)
    {
        GameObject go = Instantiate(bossPrefab, position, Quaternion.identity);

        return go;
    }

    public GameObject EnemySpawn(Vector3 position, EnemyType enemyType)
    {
        GameObject go = new GameObject();

        switch(enemyType)
        {    
            default:
                break;
        }

        return go;
    }
}
