using DG.Tweening;
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
            Rigidbody diceRb = other.gameObject.GetComponent<Rigidbody>();

            float posX = portalAxis == PortalAxis.XAxisPortal
                ? diceRb.position.x
                : (secondPortal.position + secondPortal.forward * (secondPortal.localScale.x * 0.5f + portalOffset)).x;
            float posZ = portalAxis == PortalAxis.ZAxisPortal
                ? diceRb.position.z
                : (secondPortal.position + secondPortal.forward * (secondPortal.localScale.z * 0.5f + portalOffset)).z;

            diceRb.transform.position = new Vector3(posX, diceRb.position.y, posZ);
        }
    }
}
