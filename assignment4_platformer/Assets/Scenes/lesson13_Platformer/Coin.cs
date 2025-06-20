using UnityEngine;

public class Coin : MonoBehaviour
{
    private SoundHub _soundHub;
    private bool _collected = false; //to prevent multiple triggers in one frame
    void Awake()
    {
        _soundHub = GameObject.Find("SoundHub").GetComponent<SoundHub>();
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (_collected == false && collider.CompareTag("Player"))
        {
            Debug.Log($"[COIN] {gameObject.name} triggered by {collider.name}");
            _collected = true;
            //if we play sound file here, it destroys itself before we can hear anything
            if(_soundHub == null)
            {
                Debug.LogWarning("[COIN] Missing SoundHub reference!");
            }
            _soundHub.PlayCoinSound();
            GameManager.Instance.coinsCollected++;

            Debug.Log($"[COIN] Coins Collected: {GameManager.Instance.coinsCollected}");
            Destroy(this.gameObject);
        }
        else
        {
            Debug.Log($"[COIN] Hit by something else");
        }
       
    }
}
