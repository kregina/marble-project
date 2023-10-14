using UnityEngine;

public class StaffParticles : MonoBehaviour
{
    public Particle[] particlePrefabs;

    [SerializeField] private GameObject primaryParticleSpawnPoint;
    [SerializeField] private GameObject secondaryParticleSpawnPoint;
    Particle[] primaryParticles;
    Particle[] secondaryParticles;

    private Staff staff;

    private void Start()
    {
        staff = GetComponent<Staff>();
        staff.staffColors.CollectionChanged += (_, _) => UpdateStaffColors();


        primaryParticles = new Particle[particlePrefabs.Length];
        secondaryParticles = new Particle[particlePrefabs.Length];

        for (int i = 0; i < particlePrefabs.Length; i++)
        {

            primaryParticles[i] = Instantiate(particlePrefabs[i], primaryParticleSpawnPoint.transform);
            secondaryParticles[i] = Instantiate(particlePrefabs[i], secondaryParticleSpawnPoint.transform);

            var ps1 = primaryParticles[i].GetComponent<ParticleSystem>().main;
            ps1.startSize = 5;
            var ps2 = secondaryParticles[i].GetComponent<ParticleSystem>().main;
            ps2.startSize = 3;
        }
    }


    private void UpdateStaffColors()
    {
        MarbleColor? primaryColor = staff.staffColors.Count > 0 ? staff.staffColors[0] : null;
        foreach (var particle in primaryParticles)
        {
            if (particle.color == primaryColor)
            {
                particle.ps.Play();
            }
            else
            {
                particle.ps.Stop();
            }
        }

        MarbleColor? secondaryColor = staff.staffColors.Count > 1 ? staff.staffColors[1] : null;
        foreach (var particle in secondaryParticles)
        {
            if (particle.color == secondaryColor)
            {
                particle.ps.Play();
            }
            else
            {
                particle.ps.Stop();
            }
        }
    }
}
