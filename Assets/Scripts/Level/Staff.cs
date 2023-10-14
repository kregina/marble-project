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
    [SerializeField] private AudioSource fireSound;

    public ObservableCollection<MarbleColor> staffColors = new();

    private MarbleColorManager marbleColorManager;
    private Marble projectile;
    private Coroutine reloadColorsCoroutine;

    private void Start()
    {
        fireSound = GetComponent<AudioSource>();

        marbleColorManager = GameObject.FindWithTag("MarbleColorManager").GetComponent<MarbleColorManager>();
        marbleColorManager.OnAvailableColorAdded += OnFirstAvailableColor;
        marbleColorManager.OnAvailableColorRemoved += OnAvailableColorRemoved;
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
        reloadColorsCoroutine = StartCoroutine(ReloadColorsCoroutine());
        marbleColorManager.OnAvailableColorAdded -= OnFirstAvailableColor;
    }

    public IEnumerator ReloadColorsCoroutine()
    {
        while (staffColors.Count < maxColorCount)
        {
            yield return new WaitForSeconds(colorCooldown);
            if (marbleColorManager.availableColors.Count == 0)
            {
                Debug.Log("No colors available");
                yield break;
            }
            var marbleColor = marbleColorManager.GetRandomAvailableMarbleColor();
            staffColors.Add(marbleColor);
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

        StopCoroutine(reloadColorsCoroutine);
        reloadColorsCoroutine = StartCoroutine(ReloadColorsCoroutine());
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
