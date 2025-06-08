using UnityEngine;

public class SpeedPickup : MonoBehaviour
{
    private PlayerController playerController_SCR;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            playerController_SCR = collision.gameObject.GetComponent<PlayerController>();

            playerController_SCR.IncreaseSpeed();
            UIManager.Instance.SpeedBarDepletion();
            gameObject.SetActive(false);
        }
    }
}