using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileLaser : MonoBehaviour
{
    private RaycastHit2D hit;
    private LineRenderer laser;
    private ParticleSystem particles;
    private bool isClockwise = false;
    private float initialZRotation;

    public LayerMask targetLayers;
    public bool ableToRotate = false;
    public float rotationMaxAngle = 10;
    public float rotationSpeed = 0.5f;


    private void Awake()
    {
        initialZRotation = transform.eulerAngles.z;
        laser = GetComponent<LineRenderer>();
        particles = GetComponentInChildren<ParticleSystem>();
    }

    private void Start()
    {
        laser.SetPosition(0, transform.position);
        
        if(AudioController.instance != null)
        {
            AudioController.instance.PlayLaserTile();
        }

        InvokeRepeating("ShootLaser", 0, Time.deltaTime * 2f);
    }

    private void FixedUpdate()
    {
        // ROTATES THE TILE IF IT IS MARKED TO DO SO
        if(ableToRotate)
            RotateLaser();
    }

    private void ShootLaser()
    {
        // ASSIGN THE RAYCAST
        hit = Physics2D.Raycast(transform.position, transform.up, 100, targetLayers);

        if (hit.collider != null)
        {
            // DRAWS THE LINE
            laser.SetPosition(1, hit.point);
            particles.gameObject.transform.position = new Vector3(hit.point.x, hit.point.y, particles.gameObject.transform.position.z);
            particles.Emit(1);


            // HITS THE PLAYER
            if (hit.collider.GetComponent<PlayerController>() != null)
            {
                hit.collider.GetComponent<PlayerController>().Die();
            }

            // HITS AN ENEMY
            if (hit.collider.GetComponent<EnemyController>() != null)
            {
                hit.collider.GetComponent<EnemyController>().Die();
            }
        }
    }

    private void RotateLaser()
    {
        // TO ROTATE CLOCKWISE, IT HAS TO DECREASE THE Z ROTATION VALUE
        // TO ROTATE COUNTERCLOCKWISE, IT HAS TO INCREASE THE Z ROTATION VALUE
        if(isClockwise)
        {
            transform.Rotate(new Vector3(0, 0, -1 * rotationSpeed));
            
            if (transform.eulerAngles.z < (initialZRotation - rotationMaxAngle))
            {
                isClockwise = false;                
            }                
        }
        else
        {
            transform.Rotate(new Vector3(0, 0, 1 * rotationSpeed));

            if (transform.eulerAngles.z > (initialZRotation + rotationMaxAngle))
            {
                isClockwise = true;
            }
        }        
    }
}



