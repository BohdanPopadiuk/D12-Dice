using ObjectsPool;
using UnityEngine;

public class PortalSystem : MonoBehaviour
{
    enum PortalAxis
    {
        XAxisPortal,
        ZAxisPortal
    }
    
    [SerializeField] private Transform secondPortal;
    [SerializeField] private PortalAxis portalAxis;
    [SerializeField] private float portalOffset = 3.5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dice"))
        {
            Transform portalEffect1 = PortalPoolBase.Instance.PortalEffectPool.Get().transform;
            Transform portalEffect2 = PortalPoolBase.Instance.PortalEffectPool.Get().transform;
            
            Rigidbody diceRb = other.gameObject.GetComponent<Rigidbody>();
            
            portalEffect1.position = diceRb.transform.position;
            portalEffect1.rotation = Quaternion.LookRotation(diceRb.velocity.normalized);

            float posX = portalAxis == PortalAxis.XAxisPortal
                ? diceRb.position.x
                : (secondPortal.position + secondPortal.forward * (secondPortal.localScale.x * 0.5f + portalOffset)).x;
            float posZ = portalAxis == PortalAxis.ZAxisPortal
                ? diceRb.position.z
                : (secondPortal.position + secondPortal.forward * (secondPortal.localScale.z * 0.5f + portalOffset)).z;

            diceRb.transform.position = new Vector3(posX, diceRb.position.y, posZ);
            
            portalEffect2.position = diceRb.transform.position;
            portalEffect2.rotation = Quaternion.LookRotation(diceRb.velocity.normalized);
        }
    }
}
