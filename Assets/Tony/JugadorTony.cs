using System.Collections.Generic;
using UnityEngine;

public class JugadorTony : MonoBehaviour
{
    [SerializeField]
    GameObject GeneradorDePiezas = null;
    private GeneradorDePiezas GeneradorDePiezasComponente = null;
        
    [SerializeField]
    private float velocidad = 1;

    private float inputH;
    private float inputV;
    private Vector3 spawnPosition = Vector3.zero;
    private Rigidbody cuerpoRigido;
    
    GameObject PuertaActual = null;
    List<GameObject> PizasColocadas = new List<GameObject>();
    
    void Start()
    {
        GeneradorDePiezasComponente = GeneradorDePiezas.GetComponent<GeneradorDePiezas>();
        cuerpoRigido = GetComponent<Rigidbody>();
        cuerpoRigido.useGravity = false;
        cuerpoRigido.isKinematic = false;
        cuerpoRigido.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
        cuerpoRigido.collisionDetectionMode = CollisionDetectionMode.Continuous;
        cuerpoRigido.interpolation = RigidbodyInterpolation.Interpolate;
    }
    void Update()
    {
        inputH = Input.GetAxisRaw("Horizontal");
        inputV = Input.GetAxisRaw("Vertical");
        ManejarNuevaPieza();
    }

    void FixedUpdate()
    {
        Vector3 movimiento = new Vector3(inputV, 0, -inputH).normalized * velocidad * Time.fixedDeltaTime;
        cuerpoRigido.MovePosition(cuerpoRigido.position + movimiento);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coneccion"))
        {
            if (other.gameObject.GetComponent<ConeccionReferencia>().referencia.Usada)
                return;
            
            ActualizarPuertaActual(other.gameObject);
            
            if (GeneradorDePiezasComponente.piezaActual)
                return;
            
            GameObject NuevaPieza = GeneradorDePiezasComponente.Generar();
            Coneccion coneccionComponente = PuertaActual.GetComponent<ConeccionReferencia>().referencia;
            Transform gameObjectTransform = PuertaActual.transform.parent.gameObject.transform;
            float RotacionYActual = gameObjectTransform.eulerAngles.y;
            Vector3 posicionOffsetLocal = Vector3.zero;
            switch (coneccionComponente.EjeCardinalActual)
            {
                case EjeCardinal.Norte:
                    posicionOffsetLocal = new Vector3(2, 0, 0);
                    break;
                case EjeCardinal.Sur:
                    posicionOffsetLocal = new Vector3(-2, 0, 0);
                    break;
                case EjeCardinal.Este:
                    posicionOffsetLocal = new Vector3(0, 0, -2);
                    break;
                case EjeCardinal.Oeste:
                    posicionOffsetLocal = new Vector3(0, 0, 2);
                    break;
            }
            Vector3 posicionOffset = Quaternion.Euler(0f, RotacionYActual, 0f) * posicionOffsetLocal;
            spawnPosition = gameObjectTransform.position + posicionOffset;
            NuevaPieza.transform.position = spawnPosition;
        }
    }

    private void ActualizarPuertaActual(GameObject puertaActual)
    {
        if (puertaActual == PuertaActual)
            return;

        GameObject AntiguaPuerta = PuertaActual;

        PuertaActual = puertaActual;
        /*
        PuertaActual.GetComponent<MeshRenderer>().material.color = Color.yellow;
        
        if (AntiguaPuerta)
            AntiguaPuerta.GetComponent<MeshRenderer>().material.color = Color.green;
        */
    }
    
    private void ManejarNuevaPieza()
    {
        if (!GeneradorDePiezasComponente.piezaActual)
        {
            return;
        }
        
        // Check rotar a la izquierda
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GeneradorDePiezasComponente.piezaActual.transform.Rotate(Vector3.up, -90);
        }
        
        // Check rotar a la derecha
        if (Input.GetKeyDown(KeyCode.E))
        {
            GeneradorDePiezasComponente.piezaActual.transform.Rotate(Vector3.up, 90);
        }
        
        // Check spawn
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject PiezaNueva = GeneradorDePiezasComponente.EntregaPiezaActual();
           
            PuertaActual.GetComponent<ConeccionReferencia>().referencia.Usada = true;
            PizasColocadas.Add(PiezaNueva);

            MarcarConeccionesUsadas();
        }
    }

    private void MarcarConeccionesUsadas()
    {
        foreach (var pieza in PizasColocadas)
        {
            pieza.GetComponent<PiezaCuadrada>().MarcarConnecionesUsadas();
        }
    }
}
