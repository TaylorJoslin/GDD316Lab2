using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class win : MonoBehaviour
{
    [SerializeField] GameObject goal;

    private void Start()
    {
        goal.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            goal.SetActive(true);
        }
    }
}
