using UnityEngine;
using UnityEngine.SceneManagement;

public class Projectile : MonoBehaviour
{

    public float speed;
    public Vector2 direction;
    public string shooterTag;
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.Self);

        // Destroy the projectile if it moves too far off-screen
        if (transform.position.y < -10 || transform.position.y > 10)
        {
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D collider2D)
    {
        GameState gameState = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();

        if (collider2D.gameObject.CompareTag("Player") && shooterTag != "Player")
        {
            if (gameState.PlayerHP > 0) // Prevent multiple calls giving error in level 2
            {
                gameState.PlayerHP--;  // Reduce HP
                Debug.Log("Player hit! HP remaining: " + gameState.PlayerHP);
            }
            
            if (gameState.PlayerHP <= 0)
            {
                gameState.UpdateHighScore();
                Debug.Log("Player has no HP left. Loading Start Menu...");
                SceneManager.LoadScene("GameOver"); //load gameover screen if player deis 
            }

            Destroy(gameObject);
        }

        if (collider2D.gameObject.CompareTag("Enemy") && shooterTag != "Enemy")
        {
            Debug.Log("Enemy hit!");
            gameState.Score++;
            Destroy(collider2D.gameObject);  // Destroy the enemy
            Destroy(gameObject);             // Destroy the projectile
            
        }

        if (collider2D.gameObject.CompareTag("Barrier"))
        {
            Debug.Log("Projectile destroyed by barrier!");
            Destroy(gameObject);
        }
    }
}
