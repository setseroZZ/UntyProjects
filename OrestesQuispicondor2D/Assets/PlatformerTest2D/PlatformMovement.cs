﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour {
    public float gravity=8;
    public float horizontalSpeed = 2;
    public float verticalSpeed;
    public float jumpForce=11;
    public float jetpackForce = 10;
    public float totalGravity = 0;
    public float bottomMaxSpeed = -10;
    public float rayDetectionDistance = 0.1f;

    public float raydetectionDistance =0.1f;

    private Collider2D thisCollider;
    public Collider2D colliderToIgnore;


    Vector3 leftNode { get { return transform.position - new Vector3(0.5f, 1, 0); } }
    Vector3 rightNode { get { return transform.position + new Vector3(0.5f, -1, 0); } }

    bool isGrounded;

    SpriteRenderer spriteRenderer;



    // Use this for initialization
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        thisCollider=GetComponent<Collider2D>();
    }

    void RigidBodyUpdate(){
        RaycastHit2D downLeft = Physics2D.Raycast(leftNode, Vector3.down, rayDetectionDistance);
        RaycastHit2D downRight = Physics2D.Raycast(rightNode, Vector3.down, rayDetectionDistance);

        float horizontalDirection = Input.GetAxis("Horizontal");

        if(isGrounded){
            if (!downLeft && !downRight)
            {
                isGrounded = false;
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    Debug.Log("Jump Down!");
                    //Ignoring collision
                    Physics2D.IgnoreCollision(colliderToIgnore, thisCollider);
                    //Setting layer
                    colliderToIgnore.gameObject.layer = 2;
                    isGrounded = false;
                }
                else
                {
                    verticalSpeed = jumpForce;
                    isGrounded = false;
                }

            }
        }

    }


    // Update is called once per frame
    void Update()
    {

        RaycastHit2D downLeft = Physics2D.Raycast(leftNode, Vector3.down, rayDetectionDistance);
        RaycastHit2D downRight = Physics2D.Raycast(rightNode, Vector3.down, rayDetectionDistance);
        RaycastHit2D sideLeft = Physics2D.Raycast(leftNode + new Vector3(0, 0.1f, 0), Vector3.left, 0.1f);
        RaycastHit2D sideRight = Physics2D.Raycast(rightNode + new Vector3(0, 0.1f, 0), Vector3.right, 0.1f);

        /*if (downLeft || downRight) {
            CheckReposition (new RaycastHit2D[] { downLeft, downRight });
        } else { 
            isGrounded = false; 
        }*/
        float horizontalDirection = Input.GetAxis("Horizontal");
        float verticalDirection = Input.GetAxis("Vertical");
        if (horizontalDirection < 0)
        {
            if (!spriteRenderer.flipX) { spriteRenderer.flipX = true; }
            if (sideLeft)
            {
                horizontalDirection = 0;
            }
        }
        else if (horizontalDirection > 0)
        {
            if (spriteRenderer.flipX) { spriteRenderer.flipX = false; }
            if (sideRight)
            {
                horizontalDirection = 0;
            }
        }
        if (!isGrounded)
        {
            verticalSpeed -= gravity * Time.deltaTime;
            if (verticalSpeed < 0)
            {
                rayDetectionDistance = -verticalSpeed * Time.deltaTime;
                CheckVerticalClamp(new RaycastHit2D[] { downLeft, downRight });
            }
            else {
                if (rayDetectionDistance != 0.1f)
                {
                    rayDetectionDistance = 0.1f;
                }
            }
        }
        else {
            if (!downLeft && !downRight)
            {
                isGrounded = false;
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                if(Input.GetKey(KeyCode.DownArrow)){
                    Debug.Log("Jump Down!");
                    //Ignoring collision
                    Physics2D.IgnoreCollision(colliderToIgnore, thisCollider);
                    //Setting layer
                    colliderToIgnore.gameObject.layer = 2;
                    isGrounded = false;
                }else{
                    verticalSpeed = jumpForce;
                    isGrounded = false;
                }

            }
        }

        transform.Translate(horizontalDirection * horizontalSpeed * Time.deltaTime, verticalSpeed * Time.deltaTime, 0);
    }

    void CheckVerticalClamp(RaycastHit2D[] nodeRays)
    {
        foreach (RaycastHit2D ray in nodeRays)
        {
            if (ray && rayDetectionDistance > ray.distance)
            {
                if (ray.distance <= float.Epsilon)
                {
                    float difference = leftNode.y - ray.collider.bounds.ClosestPoint(leftNode).y;
                    transform.Translate(0, -difference, 0);
                }
                else {
                    transform.Translate(0, -ray.distance, 0);
                }
                Collider2D colliderInRay = ray.collider;
                if(!colliderToIgnore){
                    colliderToIgnore = colliderInRay;
                }
                else if(colliderToIgnore!=colliderInRay){
                    Debug.Log("Ignored no more!");
                    Physics2D.IgnoreCollision(colliderToIgnore, thisCollider,false);
                    colliderToIgnore.gameObject.layer = 0;
                    colliderToIgnore = colliderInRay;
                }
                isGrounded = true;
                verticalSpeed = 0;
                break;
            }
        }
    }

    void CheckReposition(RaycastHit2D[] nodeRays)
    {
        Debug.Log(verticalSpeed);
        foreach (RaycastHit2D ray in nodeRays)
        {
            if (ray && verticalSpeed <= 0)
            {
                float distance = ray.collider.transform.localScale.y * ray.collider.bounds.size.y / 2;
                float difference = (leftNode.y - ray.collider.transform.position.y) - distance;
                if (Mathf.Abs(difference) <= 0.5f)
                {
                    transform.Translate(0, -difference, 0);
                    isGrounded = true;
                    verticalSpeed = 0;
                }
                Debug.DrawLine(transform.position + Vector3.down, ray.collider.transform.position, Color.green);
                break;
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(leftNode, 0.2f);
        Gizmos.DrawSphere(rightNode, 0.2f);
        Gizmos.color = Color.white;
        Gizmos.DrawRay(leftNode, Vector3.down * rayDetectionDistance);
    }

    void SubstractSpeed(float speedToSub){
        float tempSpeed = verticalSpeed - speedToSub;
        Debug.Log(tempSpeed);
        if(tempSpeed<bottomMaxSpeed){
            //Debug.Log("Warning: MAX MIN SPEED!");
            tempSpeed = bottomMaxSpeed;
        }
        verticalSpeed = tempSpeed;
    }

}
