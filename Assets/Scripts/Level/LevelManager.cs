using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] int level;

    private void Start()
    {
        GameManager.Instance.SetLevel(level);
    }
}