using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed = 1.5f;
    public float horizontalLimit = 2.5f;
    public float firingSpeed = 3f;
    public float firingCooldownDuration = 1f;

    public GameObject missilePrefab;
    public GameObject explosionPrefab;

    private bool fired = false;
    private float cooldownTimer;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<Rigidbody2D>().velocity = new Vector2(
            Input.GetAxis("Horizontal") * speed,
            0
        );

        if (transform.position.x > horizontalLimit) {
            transform.position = new Vector3(
                horizontalLimit,
                transform.position.y,
                transform.position.z
            );

            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

        if (transform.position.x < -horizontalLimit) {
            transform.position = new Vector3(
                -horizontalLimit,
                transform.position.y,
                transform.position.z
            );

            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

        cooldownTimer -= Time.deltaTime;
        if (Input.GetAxis("Fire1") == 1f) {
            if (cooldownTimer <= 0 && fired == false) {
                fired = true;

                cooldownTimer = firingCooldownDuration;

                GameObject missileInstance = Instantiate(missilePrefab, transform.parent);
                missileInstance.transform.position = transform.position;
                missileInstance.GetComponent<Rigidbody2D>().velocity = new Vector2(0, firingSpeed);

                Destroy(missileInstance, 2f);
            }
        } else {
            fired = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("EnemyMissile") || collision.CompareTag("Enemy")) {
            GameObject explosionInstance = Instantiate(explosionPrefab, transform.parent);
            explosionInstance.transform.position = transform.position;

            Destroy(explosionInstance, 1.5f);
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }
}
