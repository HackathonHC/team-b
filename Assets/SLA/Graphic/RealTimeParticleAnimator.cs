using UnityEngine;
using System.Collections;

namespace SLA
{
    [RequireComponent(typeof(ParticleSystem))]
    public class RealTimeParticleAnimator : MonoBehaviour
    {
        ParticleSystem _particleSystem;
        ParticleSystem ParticleSystem
        {
            get
            {
                return _particleSystem ?? (_particleSystem = GetComponent<ParticleSystem>());
            }
        }

        IEnumerator Start()
        {
            while(true)
            {
                var lastTime = Time.realtimeSinceStartup;

                yield return null;

                var deltaTime = Time.realtimeSinceStartup - lastTime;
                ParticleSystem.Simulate(deltaTime, true, false);
            }
        }
        
    }
}
