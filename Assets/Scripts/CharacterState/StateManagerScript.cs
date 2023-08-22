using CharacterState;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StateManagerScript : MonoBehaviour
{
    public int bulletsQuantity = 0;
    public int health = 100;
    public delegate void OnBulletsQuantityChanged();
    public OnBulletsQuantityChanged onBulletsQuantityChangedCallback;

    public static StateManagerScript Instance { get; private set; } // static singleton

    private void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        }
    }

    public void AddBullets(int quantity)
    {
        bulletsQuantity += quantity;
        if (onBulletsQuantityChangedCallback != null) onBulletsQuantityChangedCallback.Invoke();
    }
    
    public void SubBullets(int quantity)
    {
        bulletsQuantity -= quantity;
        if (onBulletsQuantityChangedCallback != null) onBulletsQuantityChangedCallback.Invoke();
    }

    public void TakeDamage()
    {
        health--;
        GameObject.Find("HealthBar").GetComponent<HealthBar>().SetHealth(health);

        if (health == 0)
        {
            SceneManagerScript.Instance.playerScript.KillPlayer();
            SceneManager.LoadScene(4);
        }
    }
}
