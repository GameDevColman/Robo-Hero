using Inventory.Items;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class StateUIScript : MonoBehaviour
{
    public Text bulletsQuantity;
    private StateManagerScript m_stateManagerScript;

    void Start()
    {
        m_stateManagerScript = SceneManagerScript.Instance.stateManagerScript != null ?
            SceneManagerScript.Instance.stateManagerScript : StateManagerScript.Instance;
        m_stateManagerScript.onBulletsQuantityChangedCallback += ChangeBulletsQuantity;
        ChangeBulletsQuantity();
    }

    void ChangeBulletsQuantity()
    {
        bulletsQuantity.text = (m_stateManagerScript.bulletsQuantity).ToString();
    }
}
