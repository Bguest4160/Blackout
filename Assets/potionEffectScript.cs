using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class potionEffectScript : MonoBehaviour
{
    [SerializeField] LayerMask players;
    [SerializeField] GameObject potion;
    public string potName;
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
            Collider[] colliders = Physics.OverlapSphere(transform.position, 3f, players);

            Instantiate(particle, transform.position, Quaternion.Euler(90,0,0));
            potion.SetActive(false);
            ParticleSystem parts = particle.GetComponent<ParticleSystem>();
            float totalDuration = parts.duration + parts.startLifetime;
            Destroy(particle, totalDuration);

            foreach (Collider c in colliders)
            {
                if (c.GetComponent<MovementScript>())
                {
                    if (potName.Equals("speed"))
                    {
                        c.GetComponent<MovementScript>().ChangeSpeedStats();
                    }
                    else if (potName.Equals("heal"))
                    {
                        c.GetComponent<MovementScript>().ChangeHealStats();
                    }

                    else if (potName.Equals("Jump"))
                    {
                        c.GetComponent<MovementScript>().ChangeJumpStats();
                    }

                    else if (potName.Equals("Big"))
                    {
                        c.GetComponent<MovementScript>().ChangeBigStats();
                    }
                    else if (potName.Equals("Small"))
                    {
                        c.GetComponent<MovementScript>().ChangeSmallStats();
                    }
                }
            }
        }
        objectGrabable.SetActivateCollider(false);
        
    }
}
