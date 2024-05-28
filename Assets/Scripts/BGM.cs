using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    private static BGM _instance = null;

    public static BGM Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<BGM>();

                AudioSource audio = _instance.gameObject.GetComponent<AudioSource>();
                audio.Play();
            }

            return _instance;
        }
    }

    void Start()
    {
        DontDestroyOnLoad(transform.gameObject);
        BGM.Instance.Init();
    }

    private void Init()
    {
        //Do Nothing   
    }
}
