using UnityEngine;

public class DisparadorObjeto : MonoBehaviour
{
    [Header("Configuración del Objeto (3D)")]
    [Tooltip("Arrastrá acá la silla de la Jerarquía.")]
    public Rigidbody objetoVolador; 

    [Header("Fuerza del Lanzamiento")]
    [Tooltip("Dirección y fuerza en ejes X, Y, Z. Ejemplo: X = -50 (fuerza hacia un lado), Y = 15 (hacia arriba), Z = 0.")]
    public Vector3 direccionFuerza = new Vector3(-50f, 15f, 0f);

    private bool yaSeActivo = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !yaSeActivo)
        {
            Debug.Log("¡El jugador entró al trigger 3D! Lanzando objeto...");
            yaSeActivo = true;
            LanzarObjeto();
        }
    }

    private void LanzarObjeto()
    {
        if (objetoVolador != null)
        {
            objetoVolador.isKinematic = false;

            objetoVolador.AddForce(direccionFuerza, ForceMode.Impulse);
        }
    }
}