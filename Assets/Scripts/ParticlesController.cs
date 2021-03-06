using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesController : MonoBehaviour
{
    public List<ParticleSystem> particles;
    
    private void OnEnable()
    {
        UIGameOverController.OnChangeScore += Play;
    }

    private void OnDisable()
    {
        UIGameOverController.OnChangeScore -= Play;
    }

    private void Play()
    {
        if (SliderController.currentResult != TResults.Bad && SliderController.currentResult != TResults.Poor)
        {
            foreach (var particle in particles)
            {
                particle.Play();
            }
        }
    }
}
