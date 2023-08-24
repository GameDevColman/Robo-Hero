using CharacterState;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StateManagerScript : MonoBehaviour
{
    public int bulletsQuantity = 0;
    public int health = 100;
    public delegate void OnBulletsQuantityChanged();
    public OnBulletsQuantityChanged onBulletsQuantityChangedCallback;

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
            SceneManagerScript.DestroyAndLoadScene(4);
        }
    }
    
    public void AddLife(int quantity)
    {
        health += quantity;
        GameObject.Find("HealthBar").GetComponent<HealthBar>().SetHealth(health);
    }
}
