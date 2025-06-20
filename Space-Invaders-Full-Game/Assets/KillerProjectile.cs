using UnityEngine;
using UnityEngine.SceneManagement;

public class KillerProjectile : Projectile
{
    public int damage = 3; // Instant kill damage

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        GameState gameState = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();

        if (collider2D.gameObject.CompareTag("Player") && shooterTag != "Player")
        {
            if (gameState.PlayerHP > 0) // Prevent multiple calls giving error in level 2
            {
                gameState.PlayerHP -= damage; // Reduce HP by 3 (instant kill)
                Debug.Log("Player hit! HP remaining: " + gameState.PlayerHP);
            }

            if (gameState.PlayerHP <= 0)
            {
                gameState.UpdateHighScore();
                Debug.Log("Player has no HP left. Loading Start Menu...");
                SceneManager.LoadScene("GameOver"); // Load game over screen if player dies
            }

            Destroy(gameObject);
        }

        if (collider2D.gameObject.CompareTag("Enemy") && shooterTag != "Enemy")
        {
            Debug.Log("Enemy hit!");
            gameState.Score++;
            Destroy(collider2D.gameObject); // Destroy the enemy
            Destroy(gameObject);            // Destroy the projectile
        }

        if (collider2D.gameObject.CompareTag("Barrier"))
        {
            Debug.Log("Projectile destroyed by barrier!");
            Destroy(gameObject);
        }
    }
}
