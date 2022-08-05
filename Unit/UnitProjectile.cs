using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class UnitProjectile : NetworkBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] int damageDeal = 50;
    [SerializeField] float destroyAfterSeconds = 5f;
    [SerializeField] float launchForce = 10f;

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.forward * launchForce;
    }

    public override void OnStartServer()
    {
        Invoke(nameof(DestroySelf), destroyAfterSeconds);
    }

    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<NetworkIdentity>(out NetworkIdentity networkidentity))
        {
            if(networkidentity.connectionToClient == connectionToClient) { return; }

        }
        if(other.TryGetComponent<Health>(out Health health))
        {
            health.DealDamage(damageDeal);
        }

        DestroySelf();
    }

    [Server]
    void DestroySelf()
    {
        NetworkServer.Destroy(gameObject);
    }
}
