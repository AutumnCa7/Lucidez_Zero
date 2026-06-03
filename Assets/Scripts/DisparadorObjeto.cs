using UnityEngine;

public class DisparadorObjeto : MonoBehaviour
{
    [Header("Configuración del Objeto (3D)")]
    public Rigidbody objetoVolador;

    [Header("Fuerza del Lanzamiento")]
    public Vector3 direccionFuerza = new Vector3(-1f, 0.3f, 0f);
    public float multiplicadorFuerza = 800f;

    private bool yaSeActivo = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !yaSeActivo)
        {
            Debug.Log("¡Trigger detectado");
            yaSeActivo = true;
            LanzarObjeto();
        }
    }

    private void LanzarObjeto()
    {
        if (objetoVolador != null)
        {
            objetoVolador.isKinematic = false;
            Vector3 fuerzaFinal = direccionFuerza.normalized * multiplicadorFuerza;
            objetoVolador.AddForce(fuerzaFinal, ForceMode.Impulse);
        }
    }
}