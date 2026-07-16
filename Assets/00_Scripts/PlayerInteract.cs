using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header("Configuración del Raycast")]
    public Camera camaraPrincipal;
    public float distanciaInteraccion = 3.5f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            IntentarInteractuar();
        }
    }

    void IntentarInteractuar()
    {
        Ray rayo = camaraPrincipal.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit golpe;

        if (Physics.Raycast(rayo, out golpe, distanciaInteraccion))
        {
            if (golpe.collider.CompareTag("BotonTele"))
            {
                ControladorTele tele = golpe.collider.GetComponentInParent<ControladorTele>();

                if (tele != null)
                {
                    tele.AlternarTelevisor();
                }
            }
        }
    }
}