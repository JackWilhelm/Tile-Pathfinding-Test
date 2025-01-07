using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//help from https://www.youtube.com/watch?v=xDg2pxqJHq4&list=LL&index=1 
public class ChasePlayer : MonoBehaviour
{
    [SerializeField] private float speed = 1.5f;
    private GameObject player;

    private bool hasLineOfSight = false;
    private List<GameObject> detectedTiles = new List<GameObject>();
    private bool playerInRange = false;
    private GameObject chaseTarget;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerBody");
    }

    void Update()
    {
        if(hasLineOfSight && playerInRange) {
            chaseTarget = player;
        } else {
            if (chaseTarget == player) {
                chaseTarget = null;
            }
            GameObject furthestObject = null;
            float maxDistance = 0f;

            foreach (GameObject obj in detectedTiles) {
                Tile tileScript = obj.GetComponent<Tile>();
                if (!tileScript || !tileScript.canSeePlayer) {
                    continue;
                }
                RaycastHit2D ray = Physics2D.Raycast(transform.position, obj.transform.position - transform.position);
                if (ray.collider != null) {
                    continue;
                }
                float distance = Vector2.Distance(transform.position, obj.transform.position);
                if (distance > maxDistance) {
                    maxDistance = distance;
                    furthestObject = obj;
                }
                Debug.Log("names" + obj.name);
            }
            if (furthestObject != null) {
                Debug.Log("furthest target = " + furthestObject.name);
                chaseTarget = furthestObject;
            }
        }
        if (chaseTarget != null) {
            Debug.Log(chaseTarget);
            transform.position = Vector2.MoveTowards(transform.position, chaseTarget.transform.position, speed * Time.deltaTime);
        }
        if (chaseTarget != player && chaseTarget != null && transform.position == chaseTarget.transform.position) {
            chaseTarget = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Tile")) {
            Debug.Log("see new tile");
            GameObject detectedObject = other.gameObject;

            SpriteRenderer spriteRenderer = detectedObject.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.color = Color.yellow;
            }
            if (!detectedTiles.Contains(other.gameObject)) {
                detectedTiles.Add(other.gameObject);
            }
        } else if (other.CompareTag("PlayerBody")) {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Tile")) {
            if (detectedTiles.Contains(other.gameObject)) {
                detectedTiles.Remove(other.gameObject);
            }
        } else if (other.CompareTag("PlayerBody")) {
            playerInRange = false;
            Debug.Log("player left");
        }
    }

    private void FixedUpdate()
    {
            RaycastHit2D playerRay = Physics2D.Raycast(transform.position, player.transform.position - transform.position);
            if (playerRay.collider != null) {
                hasLineOfSight = playerRay.collider.CompareTag("PlayerBody");
                if(hasLineOfSight) {
                    Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.green);
                } else {
                    Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.red);
                }
            }
    }
}
