using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
    private bool tileInRange = false;
    private bool hasLineOfSight = false;
    public bool canSeePlayer = false;
    private Color color;
    private GameObject playerBody;
    //help from https://www.youtube.com/watch?v=kkAjpQAM-jE
    // and https://www.youtube.com/watch?v=xDg2pxqJHq4&list=LL&index=1 
    // Start is called before the first frame update
    void Start()
    {
        playerBody = GameObject.FindGameObjectWithTag("PlayerBody");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init(bool isOffset) {
       if (isOffset) {
        _renderer.color = _offsetColor;
       } else {
        _renderer.color = _baseColor;
       }
       color = _renderer.color;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            tileInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
           tileInRange = false;
           _renderer.color = color;
           canSeePlayer = false;
        }
    }

    private void FixedUpdate()
    {
        if (tileInRange) {
            RaycastHit2D ray = Physics2D.Raycast(transform.position, playerBody.transform.position - transform.position);
            if (ray.collider != null)
            {
                hasLineOfSight = ray.collider.CompareTag("PlayerBody");
                if(hasLineOfSight)
                {
                    _renderer.color = Color.red;
                    canSeePlayer = true;
                    Debug.DrawRay(transform.position, playerBody.transform.position - transform.position, Color.green);
                } 
                else
                {
                    _renderer.color = color;
                    canSeePlayer = false;
                    Debug.DrawRay(transform.position, playerBody.transform.position - transform.position, Color.red);
                }
            }
        }
    }
}
