using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallClip : MonoBehaviour
{

    public CapsuleCollider headTest;
    [SerializeField] LayerMask ground;
    public Transform character;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider ground)
    {
        //Debug.Log("yay");
    }
}
