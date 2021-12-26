using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particles : MonoBehaviour
{

    ParticleSystem particleSys;

    // Start is called before the first frame update
    void Start()
    {
        particleSys = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        // update speed of particles based on game difficulty
        var vel = particleSys.velocityOverLifetime;
        vel.x = -GameManager.difficultyMultiplier;
    }
}
