using UnityEngine;

public class DropTarget : MonoBehaviour
{
    public int points = 50;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ball"))
        {
            // Add points
            GameState gameState = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();
            gameState.Score += points;

            // "Drop" the target — simple version
            gameObject.SetActive(false);
        }
    }
}
