using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> particleList;
    public static ParticleManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public enum ParticleCallers
    {
        Impact = 0,
        Blood = 1,
    }

    public void PlayParticle(ParticleCallers particle, Vector3 position)
    {
        particleList[(int)particle].transform.position = position;
        particleList[(int)particle].Play();
    }

}
