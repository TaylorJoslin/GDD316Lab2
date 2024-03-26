using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField] GameObject PlayerPreFab;
    [SerializeField] float X, Y, Z;
    [SerializeField] float rX, rY, rZ;


    // Start is called before the first frame update
    void Start()
    {
        Instantiate(PlayerPreFab, new Vector3(X, Y, Z), Quaternion.Euler(rX,rY,rZ));
    }

}
