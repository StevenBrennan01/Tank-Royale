using System.Collections.Generic;
using UnityEngine;

public class EnemyPointer : MonoBehaviour
{
    private GameManager gameManager_SCR => FindObjectOfType<GameManager>();

    [Header("Enemy Pointer Settings")]
    [SerializeField] private GameObject pointerSprite;
    [SerializeField] private Transform anchorPoint;

    private List<GameObject> pointerSprites = new List<GameObject>();
    private List<GameObject> activeEnemies => gameManager_SCR.activeEnemies;

    [SerializeField] private float pointerDist;

    private void Awake()
    {
        if (gameManager_SCR == null)
        {
            Debug.LogError("GameManager not found in the scene.");
        }
    }
    
    private void Start()
    {
        UpdateEnemyPointers();
    }

    private void Update()
    {
        UpdateEnemyPointers();

        for (int i = 0; i < activeEnemies.Count; i++)
        {
            GameObject enemy = activeEnemies[i];
            GameObject pointer = pointerSprites[i];

            if (enemy is not null && pointer is not null)
            {
                Vector3 pointerDir = (enemy.transform.position - anchorPoint.position).normalized;

                // the trigonometry to calculate the angle
                float angle = Mathf.Atan2(pointerDir.y, pointerDir.x) * Mathf.Rad2Deg;

                pointer.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90f));
                pointer.transform.position = anchorPoint.position + pointerDir * pointerDist;
            }
            else if (pointer is null)
            {
                pointer.SetActive(false);
            }
        }
    }

    private void UpdateEnemyPointers()
    {
        int enemyCount = activeEnemies.Count;

        while (pointerSprites.Count < enemyCount)
        {
            GameObject newPointer = Instantiate(pointerSprite, anchorPoint.position, Quaternion.identity);
            pointerSprites.Add(newPointer);
        }

        while (pointerSprites.Count > enemyCount)
        {
            GameObject lastPointer = pointerSprites[pointerSprites.Count - 1];
            pointerSprites.RemoveAt(pointerSprites.Count - 1);
            Destroy(lastPointer);
        }
    }
}