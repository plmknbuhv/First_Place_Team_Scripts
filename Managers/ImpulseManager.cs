using Code.Core;
using Unity.Cinemachine;
using UnityEngine;

namespace Code.Managers
{
    public class ImpulseManager : MonoSingleton<ImpulseManager>
    {
        [SerializeField] private CinemachineImpulseSource source;

        public void Impulse(float force)
        {
            source.GenerateImpulse(force);
        }
    }
}