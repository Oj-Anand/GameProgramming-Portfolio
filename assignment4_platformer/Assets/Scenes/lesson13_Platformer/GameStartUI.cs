using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
public class GameStartUI : MonoBehaviour
{
    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        Button playBtn = root.Q<Button>("PlayButton");
        if (playBtn != null)
            playBtn.clicked += Play;
    }
    void Play()
    {
        GameManager.Instance.coinsCollected = 0; //clean start
        SceneManager.LoadScene("Platformer");
    }
}
