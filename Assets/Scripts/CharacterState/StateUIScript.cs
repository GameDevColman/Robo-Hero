using UnityEngine;
using UnityEngine.UI;

public class StateUIScript : MonoBehaviour
{
    private Text bulletsQuantity;
    private StateManagerScript m_stateManagerScript;

    void Start()
    {
        m_stateManagerScript = SceneManagerScript.Instance.stateManagerScript;
        bulletsQuantity = GameObject.Find("QuantityText").GetComponent<Text>();
        m_stateManagerScript.onBulletsQuantityChangedCallback += ChangeBulletsQuantity;
        ChangeBulletsQuantity();
    }

    void ChangeBulletsQuantity()
    {
        bulletsQuantity.text = (m_stateManagerScript.bulletsQuantity).ToString();
    }
}
