/*
	This script is placed in public domain. The author takes no responsibility for any possible harm.
	Contributed by Jonathan Czeck
*/
using UnityEngine;
using System.Collections;
using static UnityEngine.ParticleSystem;

public class LightningBolt : MonoBehaviour
{
	public Transform target;
	public int zigs = 100;
	public float speed = 1f;
	public float scale = 1f;
	public Light startLight;
	public Light endLight;
	
	Perlin noise;
	float oneOverZigs;

    private ParticleSystem ps;
    private Particle[] particles;
	
	void Start()
	{
		oneOverZigs = 1f / (float)zigs;
		ps = GetComponent<ParticleSystem>();
        particles = new Particle[zigs];
        var em = ps.emission;
		em.enabled = false;
		ps.Emit(zigs);
        ps.GetParticles(particles);
	}
	
	void Update ()
	{
		if (noise == null)
			noise = new Perlin();
			
		float timex = Time.time * speed * 0.1365143f;
		float timey = Time.time * speed * 1.21688f;
		float timez = Time.time * speed * 2.5564f;

		for (int i = 0; i < particles.Length; i++)
		{
			Vector3 position = Vector3.Lerp(transform.position, target.position, oneOverZigs * (float)i);
			Vector3 offset = new Vector3(noise.Noise(timex + position.x, timex + position.y, timex + position.z),
										noise.Noise(timey + position.x, timey + position.y, timey + position.z),
										noise.Noise(timez + position.x, timez + position.y, timez + position.z));
			position += (offset * scale * ((float)i * oneOverZigs));

			particles[i].position = position;
			particles[i].startColor = Color.white;
			//particles[i].energy = 1f;
		}

		ps.SetParticles(particles);

		if (ps.particleCount >= 2)
		{
			if (startLight)
				startLight.transform.position = particles[0].position;
			if (endLight)
				endLight.transform.position = particles[particles.Length - 1].position;
		}
	}	
}