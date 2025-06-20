using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    //public int totalCoins = 0;
    public int coinsCollected = 0;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    public void ResetGame()
    {
        Debug.Log("[GameManager] Resetting coin count");
        coinsCollected = 0;
        //totalCoins = 0;
    }
}
