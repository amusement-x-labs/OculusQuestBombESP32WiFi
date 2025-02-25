using Oculus.Interaction;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Core.Scripts
{
    public class ResultHandler : MonoBehaviour
    {
        [SerializeField] private List<ParticleSystem> _ps;
        [Space(20)]
        [SerializeField] private WebBasedController ctrl;
        private void Start()
        {
            ctrl.OnWinState += ActivateWinState;
            ctrl.OnLostState+= ActivateLostState;
        }

        private void OnDestroy()
        {
            ctrl.OnWinState -= ActivateWinState;
            ctrl.OnLostState -= ActivateLostState;
        }

        private void ActivateWinState()
        {
            
        }

        private void ActivateLostState()
        {
			for(int i = 0; i < _ps.Count; i++)
				_ps[i].Play();
        }
    }
}
