using UnityEngine;

public class ParticleRotationByVelocity : MonoBehaviour
{
    private ParticleSystem ps;
    private ParticleSystem.Particle[] particles;

    void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        particles = new ParticleSystem.Particle[ps.main.maxParticles];
    }

    void LateUpdate()
    {
        int count = ps.GetParticles(particles);

        for (int i = 0; i < count; i++)
        {
            Vector3 velocity = particles[i].velocity;
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;

            // 이미지 방향에 따라 보정값 추가 필요할 수 있음
            particles[i].rotation = angle;
        }

        ps.SetParticles(particles, count);
    }
}
