using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public GameObject explosionPrefab;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("PlayerMissile")) {
            GameObject explosionInstance = Instantiate(explosionPrefab, transform.parent.parent);
            explosionInstance.transform.position = transform.position;

            Destroy(explosionInstance, 1.5f);
            Destroy(gameObject);
            Destroy(collision.gameObject);

        }
    }
}
