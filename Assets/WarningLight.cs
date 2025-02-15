using System.Collections;
using UnityEngine;

public class WarningLight : MonoBehaviour
{
    public Material warningMaterial;
    public Material safeMaterial;

    public GameObject warningLight;  
    public GameObject safeLight;    

    public float rotationSpeed = 100f;  

    private bool isActive = false;

    private void Update()
    {
        if (isActive && warningLight != null)
        {
            warningLight.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }
    }

    public void ActivateWarningLight()
    {
        isActive = true;

        if (warningLight != null)
        {
            warningLight.SetActive(true);
        }
        if (safeLight != null)
        {
            safeLight.SetActive(false);
        }

        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null && warningMaterial != null)
        {
            renderer.material = warningMaterial;
        }
    }

    public void DeactivateWarningLight()
    {
        isActive = false;

        if (warningLight != null)
        {
            warningLight.SetActive(false);
        }
        if (safeLight != null)
        {
            safeLight.SetActive(true);
        }

        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null && safeMaterial != null)
        {
            renderer.material = safeMaterial;
        }
    }
}

