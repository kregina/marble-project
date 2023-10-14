using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Staff : MonoBehaviour
{
    public int maxColorCount = 2;

    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private float colorCooldown = 1f;

    public ObservableCollection<MarbleColor> staffColors = new();

    private AudioSource fireSound;
    private MarbleColorManager marbleColorManager;
    private Marble projectile;
    private Coroutine reloadColorsCoroutine;

    private void Start()
    {
        fireSound = GetComponent<AudioSource>();

        marbleColorManager = GameObject.FindWithTag("MarbleColorManager").GetComponent<MarbleColorManager>();
        marbleColorManager.OnFirstColorAvailable += OnFirstAvailableColor;
        marbleColorManager.OnAvailableColorRemoved += OnAvailableColorRemoved;
        marbleColorManager.OnNoColorsAvailable += OnNoColorsAvailable;

        reloadColorsCoroutine = StartCoroutine(ReloadColorsCoroutine());
    }

    void Update()
    {
        if(GameManager.Instance.GameIsPaused) return;

        LookAtMouse();

        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            SwapCollors();
        }
    }

    private void SwapCollors()
    {        
        if (staffColors.Count == 2)
        {
            var temp = staffColors[0];
            staffColors[0] = staffColors[1];
            staffColors[1] = temp;
        }
    }

    private void OnFirstAvailableColor(MarbleColor _)
    {
        Debug.Log("OnFirstAvailableColor");
        StopCoroutine(reloadColorsCoroutine);
        reloadColorsCoroutine = StartCoroutine(ReloadColorsCoroutine());
    }

    private void OnNoColorsAvailable()
    {
        Debug.Log("OnNoColorsAvailable");
        StopCoroutine(reloadColorsCoroutine);
        staffColors.Clear();
    }

    public IEnumerator ReloadColorsCoroutine()
    {
        Debug.Log("ReloadColorsCoroutine");
        while (staffColors.Count < maxColorCount)
        {
            yield return new WaitForSeconds(colorCooldown);
            Debug.Log("Loop ReloadColorsCoroutine availableColors: " + marbleColorManager.availableColors.Count);
            if (marbleColorManager.availableColors.Count == 0)
            {
                yield break;
            }
            staffColors.Add(marbleColorManager.GetRandomAvailableMarbleColor());

        }
    }

    private void OnAvailableColorRemoved(MarbleColor color)
    {
        if (marbleColorManager.availableColors.Count == 0)
        {
            staffColors.Clear();
            return;
        }

        for (int i = 0; i < staffColors.Count; i++)
        {
            if (staffColors[i] == color)
            {
                staffColors[i] = marbleColorManager.GetRandomAvailableMarbleColor();
            }
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
        fireSound.Play();

        Debug.Log("Fire ReloadColorsCoroutine");
        StopCoroutine(reloadColorsCoroutine);
        reloadColorsCoroutine = StartCoroutine(ReloadColorsCoroutine());
    }

    private void InstantiateAndSetupProjectile(Marble marblePrefab)
    {
        projectile = Instantiate(marblePrefab);
        projectile.transform.position = spawnPoint.transform.position;
        projectile.transform.rotation = spawnPoint.transform.rotation;

        projectile.AddComponent<Projectile>();
        projectile.GetComponent<PathFollower>().enabled = false;
    }
}
