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
    [SerializeField] private ParticleSystem[] staffParticles;
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

    void Update()
    {
        LookAtMouse();

        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }
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
            var marbleColor = marbleColorManager.GetRandomAvailableMarbleColor();
            staffColors.Add(marbleColor);
                       
            UpdateStaffColors();
        }   
    }

    private void UpdateStaffColors()
    {
        //for (int i = 0; i < staffParticles.Length; i++)
        //{
        //    if (staffColors.Count < i + 1)
        //    {
        //        staffParticles[i].gameObject.SetActive(false);
        //    }
        //    else
        //    {
        //        staffParticles[i].gameObject.SetActive(true);
        //        var main = staffParticles[i].GetComponent<ParticleSystem>().main;
        //        main.startColor = staffColors[i].GetRgbColor();                
        //    }            
        //}               
    }

    private void OnAvailableColorRemoved(MarbleColor color)
    {
        if(marbleColorManager.availableColors.Count == 0)
        {
            staffColors.Clear();
            UpdateStaffColors();
            return;
        }

        for (int i = 0; i < staffColors.Count; i++)
        {
            if (staffColors[i] == color)
            {
                staffColors[i] = marbleColorManager.GetRandomAvailableMarbleColor();
            }
        }
        UpdateStaffColors();
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
        UpdateStaffColors();

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
    }

  
}
