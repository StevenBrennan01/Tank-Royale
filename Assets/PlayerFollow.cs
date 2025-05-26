using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    private Transform playerPos;

    private void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        transform.position = playerPos.position;
    }
}
