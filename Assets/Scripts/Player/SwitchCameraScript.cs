using UnityEngine;

public class SwitchCameraScript : MonoBehaviour
{

    [SerializeField] private GameObject playerPrimaryCamera;
    [SerializeField] private GameObject playerSecondaryCamera;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            playerPrimaryCamera.SetActive(false);
            playerSecondaryCamera.SetActive(true);
        }

        if (Input.GetKeyUp(KeyCode.Mouse2))
        {
            playerPrimaryCamera.SetActive(true);
            playerSecondaryCamera.SetActive(false);
        }
    }

}
