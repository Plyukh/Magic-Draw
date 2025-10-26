using UnityEngine;

public class CurrencyEffect : Move
{
    private ParticleSystem system;

    public float seconds;
    public float startSeconds;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    private static ParticleSystem.Particle[] particles = new ParticleSystem.Particle[1000];

    [System.Obsolete]
    void Update()
    {
        if (system == null) system = GetComponent<ParticleSystem>();

        var count = system.GetParticles(particles);
        seconds += Time.deltaTime;

        if(seconds > startSeconds)
        {
            for (int i = 0; i < count; i++)
            {
                var particle = particles[i];

                particle.velocity = new Vector3(0,0,0);

                particle.position = Vector3.Lerp(particle.position, target.GetComponent<Collider2D>().bounds.center, moveSpeed * Time.deltaTime);

                particles[i] = particle;
                if (Vector3.Distance(target.GetComponent<Collider2D>().bounds.center, particle.position) <= 1)
                {
                    particle.startLifetime = 0;
                    particles[i] = particle;
                }
            }

            system.SetParticles(particles, count);
        }
    }
}
