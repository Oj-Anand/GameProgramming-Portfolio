using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Parent : MonoBehaviour
{
    public float speed;
    public Vector2 direction;
    public GameObject alienPrefab;
    public GameObject killerAlienPrefab; // Prefab for KillerAlien
    public int rows = 3;
    public int columns = 6;
    public float spacing = 1.5f;
    public float minYPosition = -3.0f;
    private GameState gameState;
    void Start()
    {
        gameState = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();

        // Increase enemy speed in Level 2
        if (gameState.CurrentLevel == 2)
        {
            speed *= 1.5f; // Make enemies move 50% faster
        }
        
        SpawnAliens();
        gameState.EnemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
    }
    void Update()
    {
        transform.Translate(speed * direction * Time.deltaTime);
        if (gameState.PlayerHP <= 0)
        {
            SceneManager.LoadScene("GameOver");
            return;  // Stop execution to prevent Level 2 from loading
        }
        if (gameState.EnemyCount <= 0 && gameState.CurrentLevel == 1 && gameState.PlayerHP > 0)
        {
            gameState.CurrentLevel = 2;  // Update level
            //gameState.PlayerHP = 3; 
            SceneManager.LoadScene("Level2");
        }
        if (gameState.EnemyCount <= 0 && gameState.CurrentLevel == 2)
        {
            SceneManager.LoadScene("GameOver");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(transform.position, 0.3f);
    }

    void SpawnAliens()
    {
        bool isLevel2 = SceneManager.GetActiveScene().name == "Level2";
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                Vector3 spawnPosition = new Vector3(col * spacing, row * spacing, 0);

            if (isLevel2 && Random.value < 0.2f) 
            {
                Instantiate(killerAlienPrefab, spawnPosition, Quaternion.identity, transform);
                
            }
            else
            {
                Instantiate(alienPrefab, spawnPosition, Quaternion.identity, transform);
            }

                Debug.Log("Spawned Alien at: " + spawnPosition);

            }
        }
    }

    public void MoveDown()
    {
        float moveDistance = GetComponentInChildren<SpriteRenderer>().bounds.size.y; // Adjust dynamically based on enemy ship size
        float newYPosition = transform.position.y - moveDistance;

        // Only move down if we're above the minimum Y position
        if (newYPosition > minYPosition)
        {
            transform.position = new Vector3(transform.position.x, newYPosition, transform.position.z);
        }
        else
        {
            Debug.Log("Aliens reached the lowest point! No more downward movement.");
        }
    }
}
