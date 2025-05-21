using UnityEngine;

public class SpeedPickup : MonoBehaviour
{
    private PlayerController playerController_SCR;

    //private Coroutine reinstateSpeedPickup;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            playerController_SCR = collision.gameObject.GetComponent<PlayerController>();
            //reinstateSpeedPickup = StartCoroutine(ReinstateSpeedPickup());

            playerController_SCR.IncreaseSpeed();
            gameObject.SetActive(false);
        }
    }

    //private IEnumerator ReinstateSpeedPickup()
    //{
    //    gameObject.SetActive(false);
    //    yield return new WaitForSeconds(4f);
    //    gameObject.SetActive(true);
    //}
}