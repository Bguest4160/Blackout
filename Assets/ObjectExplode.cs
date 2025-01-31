using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class ObjectExplode : MonoBehaviour
{
    [SerializeField] GameObject object1;
    public GameObject particle;
    ObjectGrabable objectGrabable;


    // Start is called before the first frame update
    void Start()
    {
        objectGrabable = GetComponent<ObjectGrabable>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (objectGrabable.GetactivateCollier() == true)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 3f);

            Instantiate(particle, transform.position, Quaternion.Euler(90, 0, 0));
            object1.SetActive(false);
            ParticleSystem parts = particle.GetComponent<ParticleSystem>();
            float totalDuration = parts.duration + parts.startLifetime;
            Destroy(particle, totalDuration);

        }
        objectGrabable.SetActivateCollider(false);
    }
}
