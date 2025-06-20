using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class StartMenuHandler : MonoBehaviour
{
    
    private void OnEnable()
    {
        Debug.Log("UI works");
        // Get UI root
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        if (root == null)
        {
            Debug.LogError("UI Document root not found!");
            return;
        }

        
        Label title = root.Q<Label>();
        Label message = root.Q<Label>();
    
        // Get the Start Button
        Button startGameButton = root.Q<Button>("StartGameButton");

        Debug.Log($"Found Title Label: {title != null}");
        Debug.Log($"Found Message Label: {message != null}");
        Debug.Log($"Found Start Button: {startGameButton != null}");

        // Assign button click event
        if (startGameButton != null)
            startGameButton.clicked += StartGame;
    }

    private void StartGame()
    {
        Debug.Log("Starting Game...");
        SceneManager.LoadScene("SampleScene");  
    }
}
