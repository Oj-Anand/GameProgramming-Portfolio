using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    private GameManager _gameManager;

    void Start()
    {
        _gameManager = GameObject.FindFirstObjectByType<GameManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player reached the goal!");
            if(GameManager.Instance.coinsCollected >= 3)
            {
                SceneManager.LoadScene("GameOver");
            }
            else
            {
                Debug.Log($"You Need to collect {3-GameManager.Instance.coinsCollected} coins more");
            }
        }
    }
}
