using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    // uses a Box-Muller transform to approximate a normal dist.
    // https://mathworld.wolfram.com/Box-MullerTransformation.html
    public static float SampleFromNormalDist(float mean, float std) {
        float u1 = Random.Range(0f, 1f); //uniform(0,1] random doubles
        float u2 = Random.Range(0f, 1f);
        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2); //random normal(0,1)
        float randNormal = mean + std * randStdNormal; //random normal(mean,stdDev^2)
        return randNormal;
    }
}
