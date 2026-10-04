using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float velocidadMovimiento = 5f;
    [SerializeField] private float gravedad = -9.81f;

    [Header("Look Settings")]
    [SerializeField] private float velocidadRotacion = 2f;
    [SerializeField] private float limiteInclinacionVertical = 90f;

    [Header("References")]
    [SerializeField] private CharacterController cc;
    [SerializeField] private Transform transformPJ;
    [SerializeField] private Camera camara;

    private float rotacionX;
    private float velocidadVertical;

    private void Awake()
    {
        if (cc == null) cc = GetComponent<CharacterController>();
        if (transformPJ == null) transformPJ = transform;
        if (camara == null) camara = GetComponentInChildren<Camera>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        MovimientoCamara();
        MovimientoPJ();
    }

    private void MovimientoPJ()
    {
        float movX = Input.GetAxisRaw("Horizontal");
        float movZ = Input.GetAxisRaw("Vertical");

        Vector3 direccionPlana = (transformPJ.forward * movZ + transformPJ.right * movX).normalized;
        Vector3 desplazamiento = direccionPlana * velocidadMovimiento;

        if (cc.isGrounded && velocidadVertical < 0f)
        {
            velocidadVertical = -2f;
        }
        else
        {
            velocidadVertical += gravedad * Time.deltaTime;
        }

        desplazamiento.y = velocidadVertical;
        cc.Move(desplazamiento * Time.deltaTime);
    }

    private void MovimientoCamara()
    {
        float ratonX = Input.GetAxisRaw("Mouse X");
        float ratonY = Input.GetAxisRaw("Mouse Y");

        rotacionX -= ratonY * velocidadRotacion;
        rotacionX = Mathf.Clamp(rotacionX, -limiteInclinacionVertical, limiteInclinacionVertical);

        if (camara != null)
        {
            camara.transform.localRotation = Quaternion.Euler(rotacionX, 0f, 0f);
        }

        transformPJ.Rotate(Vector3.up * (ratonX * velocidadRotacion));
    }
}
