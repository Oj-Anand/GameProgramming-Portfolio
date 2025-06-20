using UnityEngine;
using UnityEngine.SceneManagement;
public class Alien : MonoBehaviour
{
    public GameObject projectilePrefab;  
    public int upperRandomRangeForFiring = 100;
    public GameObject explosionPrefab;   

    private void Update()
    {
        //Debug.Log("Alien is active and running Update.");
        // Randomly decide if this alien should fire
        int rando = Random.Range(1, upperRandomRangeForFiring);
        if (rando == 1)
        {
            Fire();
        }
    }

    public virtual void Fire()
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // Assign movement values from `Projectile.cs`
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            projectileScript.direction = Vector2.down;  // Move downward
            projectileScript.speed = 5f;               // Set speed
            projectileScript.shooterTag = "Enemy";     // set shooter tag to enemy so projectiles dont kill our alien friends      
        }

        Debug.Log("Alien fired a projectile!");
    }

    // Handle Wall Collisions (Reverse Direction)
    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        //Debug.Log($"Alien collided with: {collider2D.gameObject.name}");
        if (collider2D.CompareTag("WallLeft") || collider2D.CompareTag("WallRight"))
        {
            //Debug.Log("Reversing Enemy Direction!");
            Parent parentScript = transform.parent.GetComponent<Parent>();

            if (parentScript != null)
            {
                parentScript.direction.x *= -1; // Reverse movement direction
                parentScript.MoveDown();
            }
            else
            {
                Debug.LogError("Alien.cs: Parent.cs is missing from AlienManager!");
            } 

            // TODO: Move the entire squad down (to be implemented later)
        }
    }

    private void OnDestroy() 
    {        
        GameState gameState = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();

        // Reduce Enemy Count
        gameState.EnemyCount--;
        Debug.Log("Enemy destroyed! Remaining: " + gameState.EnemyCount);

        // If all enemies are destroyed, load Level 2
        if (gameState.EnemyCount <= 0)
        {
            Debug.Log("All enemies destroyed! Loading Level 2...");
            SceneManager.LoadScene("Level2");
        }
        
    }
}
