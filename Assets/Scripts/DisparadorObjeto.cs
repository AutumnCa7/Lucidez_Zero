using UnityEngine;

public class DisparadorObjeto : MonoBehaviour
{
    [Header("Configuración del Objeto (3D)")]
    [Tooltip("Arrastrá acá la silla de la Jerarquía.")]
    public Rigidbody objetoVolador; // Rigidbody 3D normal

    [Header("Fuerza del Lanzamiento")]
    [Tooltip("Dirección y fuerza en ejes X, Y, Z. Ejemplo: X = -50 (fuerza hacia un lado), Y = 15 (hacia arriba), Z = 0.")]
    public Vector3 direccionFuerza = new Vector3(-50f, 15f, 0f);

    private bool yaSeActivo = false;

    // Ojo acá: para 3D es OnTriggerEnter a secas, sin el "2D" al final
    private void OnTriggerEnter(Collider other)
    {
        // El trigger revisa si el que entró tiene el tag "Player"
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
            // Le quitamos el Kinematic por si estaba tildado, para que actúen las físicas
            objetoVolador.isKinematic = false;

            // Aplicamos la fuerza física 3D como un impulso instantáneo
            objetoVolador.AddForce(direccionFuerza, ForceMode.Impulse);
        }
    }
}