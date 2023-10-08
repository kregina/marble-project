using PathCreation.Examples;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Device;

public class Staff : MonoBehaviour
{
    [SerializeField] private Marble[] marblePrefabs;
    [SerializeField] private GameObject spawnPoint;

    public int maxColorCount = 2;

    MarbleColorManager marbleColorManager;
    private Marble projectile;

    private List<MarbleColor> staffColors = new();

    [SerializeField] private float colorCooldown = 1f;

    private void Start()
    {
        marbleColorManager = GameObject.FindWithTag("MarbleColorManager").GetComponent<MarbleColorManager>();

        marbleColorManager.OnAvailableColorAdded += OnFirstAvailableColor;
        marbleColorManager.OnAvailableColorRemoved += OnAvailableColorRemoved;
    }    

    private void OnFirstAvailableColor(MarbleColor _)
    {
        StartCoroutine(ReloadColorsCoroutine());
        marbleColorManager.OnAvailableColorAdded -= OnFirstAvailableColor;
    }

    public IEnumerator ReloadColorsCoroutine()
    {       
        while(staffColors.Count < maxColorCount)
        {
            yield return new WaitForSeconds(colorCooldown);
            if (marbleColorManager.availableColors.Count == 0)
            {
                Debug.Log("No colors available");
                yield break;
            }
            staffColors.Add(marbleColorManager.GetRandomMarbleColor());
        }   
    }

    private void OnAvailableColorRemoved(MarbleColor color)
    {
        if(marbleColorManager.availableColors.Count == 0)
        {
            staffColors.Clear();
            return;
        }

        for (int i = 0; i < staffColors.Count; i++)
        {
            if (staffColors[i] == color)
            {
                staffColors[i] = marbleColorManager.GetRandomMarbleColor();
            }
        }
    }

    void Update()
    {
        LookAtMouse();

        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }

    private void LookAtMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        var mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.transform.position.y));
        transform.LookAt(mouseWorldPosition);
    }

    private void Fire()
    {
        if (staffColors.Count == 0) return;
        
        var marblePrefab = marbleColorManager.GetMarblePrefab(staffColors[0]);
        staffColors.RemoveAt(0);        

        InstantiateAndSetupProjectile(marblePrefab);

        StartCoroutine(ReloadColorsCoroutine());
    }

    private void InstantiateAndSetupProjectile(Marble marblePrefab)
    {
        projectile = Instantiate(marblePrefab);
        projectile.transform.position = spawnPoint.transform.position;
        projectile.transform.rotation = spawnPoint.transform.rotation;

        projectile.AddComponent<Projectile>();
        projectile.tag = "Projectile";
        projectile.GetComponent<PathFollower>().enabled = false;

        var rb = projectile.GetComponent<Rigidbody>();
        rb.velocity = transform.forward * 50f;        
    }

  
}
