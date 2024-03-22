using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    [SerializeField] GameObject PlayerPreFab;


    // Start is called before the first frame update
    void Start()
    {
        Instantiate(PlayerPreFab, new Vector3(57.769f, 5.413f, 2.424f), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
