using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public float shootingInterval = 3f;
    public float shootingSpeed = 2f;

    public float maximumMovingInterval = 0.4f;
    public float minimumMovingInterval = 0.05f;
    public float movingDistance = 0.2f;
    public float horizontalLimit = 2.5f;

    public GameObject enemyMissilePrefab;
    public GameObject enemyContainer;
    public PlayerController player;

    private float shootingTimer;
    private float movingTimer;
    private float movingDirection = 1;
    private float movingInterval;
    private int enemyCount;

	// Use this for initialization
	void Start () {
        movingInterval = maximumMovingInterval;
        shootingTimer = shootingInterval;

        enemyCount = GetComponentsInChildren<EnemyController>().Length;
	}
	
	// Update is called once per frame
	void Update () {
        int currentEnemyCount = GetComponentsInChildren<EnemyController>().Length;

        //shooting
        shootingTimer -= Time.deltaTime;

        if (currentEnemyCount > 0 && shootingTimer <= 0f) {
            shootingTimer = shootingInterval;

            EnemyController[] enemies = GetComponentsInChildren<EnemyController>();
            EnemyController randomEnemy = enemies[Random.Range(0, enemies.Length)];

            GameObject missileInstance = Instantiate(enemyMissilePrefab, transform);
            missileInstance.transform.position = randomEnemy.transform.position;
            missileInstance.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -shootingSpeed);

            Destroy(missileInstance, 3f);
        }

        // movement
        movingTimer -= Time.deltaTime;

        if (movingTimer <= 0f) {
            float difficulty = 1f - (float) currentEnemyCount / enemyCount;
            movingInterval = maximumMovingInterval - (maximumMovingInterval - minimumMovingInterval) * difficulty;
            movingTimer = movingInterval;

            enemyContainer.transform.position = new Vector2(
                enemyContainer.transform.position.x + (movingDistance * movingDirection),
                enemyContainer.transform.position.y
            );

            if (movingDirection > 0) {
                float rightmostPosition = 0f;

                foreach (EnemyController enemy in GetComponentsInChildren<EnemyController>()) {
                    if (enemy.transform.position.x > rightmostPosition) {
                        rightmostPosition = enemy.transform.position.x;
                    }
                }

                if (rightmostPosition > horizontalLimit) {
                    movingDirection *= -1;

                    enemyContainer.transform.position = new Vector2(
                        enemyContainer.transform.position.x,
                        enemyContainer.transform.position.y - movingDistance
                    );
                }
            } else {
                float leftmostPosition = 0f;

                foreach (EnemyController enemy in GetComponentsInChildren<EnemyController>()) {
                    if (enemy.transform.position.x < leftmostPosition) {
                        leftmostPosition = enemy.transform.position.x;
                    }
                }

                if (leftmostPosition < -horizontalLimit) {
                    movingDirection *= -1;

                    enemyContainer.transform.position = new Vector2(
                        enemyContainer.transform.position.x,
                        enemyContainer.transform.position.y - movingDistance
                    );
                }
            }
        }

        if (currentEnemyCount == 0 || player == null) {
            SceneManager.LoadScene("Game");
        }
	}
}
