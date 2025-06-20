using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
public class GameOverUI : MonoBehaviour
{
        public int coinsCollected = 0;


    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        Label coinLabel = root.Q<Label>("CoinLabel");
        if (coinLabel != null)
        {
            coinsCollected = GameManager.Instance.coinsCollected;
            coinLabel.text = $"Coins Collected: {coinsCollected}";
        }  

        Button playAgainBtn = root.Q<Button>("PlayAgainButton");
        if (playAgainBtn != null)
            playAgainBtn.clicked += OnPlayAgainClicked;
    }
    void OnPlayAgainClicked()
    {
        GameManager.Instance.ResetGame();//reset the game everytime your restart 
        SceneManager.LoadScene("GameStart");
    }

}
