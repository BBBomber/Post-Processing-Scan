using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ScanAbility : MonoBehaviour
{
    public Material material; // The scan shader material
    public float maxRadius = 10f; // Maximum radius for the scan effect
    public float scanSpeed = 5f; // How quickly the scan expands
    public AudioClip hitSound; // Sound to play when the scan hits an entity

    private float currentRadius = 0f; // Current radius of the scan
    private bool isScanning = false; // Whether the scan is active
    private AudioSource audioSource; // Audio source for playing sounds

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        currentRadius = 0f;
        UpdateMaterial(); // Set initial radius to 0
    }

    void Update()
    {
        if (isScanning)
        {
            currentRadius += scanSpeed * Time.deltaTime; // Expand the radius
            UpdateMaterial(); // Update shader with current radius

            // Check for entities hit by the scan
            DetectEntitiesInRange();

            // Stop scanning if the max radius is reached
            if (currentRadius >= maxRadius)
            {
                isScanning = false;
                currentRadius = 0f; // Reset radius for the next scan
            }
        }

        // Listen for the "E" key to trigger the scan
        if (Input.GetKeyDown(KeyCode.E) && !isScanning)
        {
            StartScan();
        }
    }

    void StartScan()
    {
        currentRadius = 0f; // Reset radius at the start
        isScanning = true; // Start the scanning process
        UpdateMaterial(); // Update shader with initial radius
    }

    void UpdateMaterial()
    {
        if(isScanning)
        {
            material.SetFloat("_Radius", currentRadius); // Set the radius
        }
        else
        {
            material.SetFloat("_Radius", currentRadius); // Set the radius
            material.SetVector("_Position", transform.position); // Set scan position to player position
        }
        
        
    }

    void DetectEntitiesInRange()
    {
        // Detect entities within the current radius
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, currentRadius);
        foreach (var hitCollider in hitColliders)
        {
            // Assuming your entities have a specific tag, e.g., "Enemy"
            if (hitCollider.CompareTag("Enemy"))
            {
                // Play the sound if the entity is within the scan range
                audioSource.PlayOneShot(hitSound);
                Debug.Log("Found an enemy bro");
                // Additional logic here if you want to mark or reveal the entity
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = material.GetColor("_Colour");
        Gizmos.DrawWireSphere(transform.position, currentRadius);
    }
}
