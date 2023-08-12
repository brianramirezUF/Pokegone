using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAfter : MonoBehaviour
{
    public GameObject box;
    // Start is called before the first frame update
    public void dis() { 
        box.SetActive(false);
    }
}