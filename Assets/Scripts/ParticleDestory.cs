using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestory : MonoBehaviour
{
    private ParticleSystem _particle;

    // Start is called before the first frame update
    void Start()
    {
        _particle = GetComponent<ParticleSystem>();    
    }

    // Update is called once per frame
    void Update()
    {
        if (_particle.IsAlive() == false)
            Destroy(gameObject);
    }
}
