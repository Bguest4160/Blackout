using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class potionEffectScript : MonoBehaviour
{
    [SerializeField] LayerMask players;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        Collider[] colliders = Physics.OverlapSphere(transform.position, 3f,players);

        foreach (Collider c in colliders)
        {
            if (c.GetComponent<MovementScript>())
            {
                c.GetComponent<MovementScript>().ChangeStats();
            }
        }
    }
}
