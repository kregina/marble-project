using UnityEngine;

public class Marble : MonoBehaviour
{
    private MarbleColorManager marbleColorManager;
    public MarbleColor color;
    [HideInInspector] public PathFollower pathFollower;
    [HideInInspector] public Wave parentWave;


    void Start()
    {
        marbleColorManager = GameObject.FindWithTag("MarbleColorManager").GetComponent<MarbleColorManager>();
        marbleColorManager.AddColor(color);
    }

    void OnDestroy()
    {
        marbleColorManager.RemoveColor(color);
    }
}


