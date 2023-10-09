using System.Collections;
using UnityEngine;

public class Particle : MonoBehaviour
{
    public MarbleColor color;

    public ParticleSystem ps;

    private void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }
}
