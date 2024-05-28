using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _particle = null;

    private void Start()
    {
        ////블록 머티리얼
        //var cubeMat = GetComponent<MeshRenderer>().sharedMaterial;

        ////파티클 렌더러
        //ParticleSystem.MainModule particleMain = _particle.main;
        //particleMain.startColor = new ParticleSystem.MinMaxGradient(cubeMat.color);
        //_cubeMat = GetComponent<MeshRenderer>().sharedMaterial;
        //_particleMain = _particle.main;

        //hack : 파티클의 시작 색을 여기서 처리하면 적용이 안되는 문제, 일단 객체 생성 바로 전에 색 처리하는 것으로 구현
    }

    public void Particle()
    {
        //블록 머티리얼
        var cubeMat = GetComponent<MeshRenderer>().sharedMaterial;

        //파티클 렌더러
        ParticleSystem.MainModule particleMain = _particle.main;
        particleMain.startColor = cubeMat.color;

        //월드 포지션에서 생성
        var particle = Instantiate(_particle, transform.position, Quaternion.identity);
    }
}
