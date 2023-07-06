using UnityEngine;

public class StateManagerScript : MonoBehaviour
{
    public int bulletsQuantity = 0;
    public delegate void OnBulletsQuantityChanged();
    public OnBulletsQuantityChanged onBulletsQuantityChangedCallback;

    public static StateManagerScript Instance { get; private set; } // static singleton

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    public void AddBullets(int quantity)
    {
        bulletsQuantity += quantity;
        onBulletsQuantityChangedCallback.Invoke();
    }
    
    public void SubBullets(int quantity)
    {
        bulletsQuantity -= quantity;
        onBulletsQuantityChangedCallback.Invoke();
    }
}
