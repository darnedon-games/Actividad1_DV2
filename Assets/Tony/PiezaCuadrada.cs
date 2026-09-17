using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class PiezaCuadrada : Pieza
{
    // Meshes
    [SerializeField]
    GameObject MeshNorte;
    [SerializeField]
    GameObject MeshNorEsteNorte;
    [SerializeField]
    GameObject MeshNorOesteNorte;
    
    [SerializeField]
    GameObject MeshSur;
    [SerializeField]
    GameObject MeshSurEsteSur;
    [SerializeField]
    GameObject MeshSurOesteSur;
    
    [SerializeField]
    GameObject MeshEste;
    [SerializeField]
    GameObject MeshEsteEsteNorte;
    [SerializeField]
    GameObject MeshEsteEsteSur;
    
    [SerializeField]
    GameObject MeshOeste;
    [SerializeField]
    GameObject MeshOesteOesteNorte;
    [SerializeField]
    GameObject MeshOesteOesteSur;
    
    // Conneciones
    [SerializeField]
    Coneccion Norte;
    [SerializeField]
    Coneccion NorEsteNorte;
    [SerializeField]
    Coneccion NorOesteNorte;
    
    [SerializeField]
    Coneccion Sur;
    [SerializeField]
    Coneccion SurEsteSur;
    [SerializeField]
    Coneccion SurOesteSur;
    
    [SerializeField]
    Coneccion Este;
    [SerializeField]
    Coneccion EsteEsteNorte;
    [SerializeField]
    Coneccion EsteEsteSur;
    
    [SerializeField]
    Coneccion Oeste;
    [SerializeField]
    Coneccion OesteOesteNorte;
    [SerializeField]
    Coneccion OesteOesteSur;
    
    private List<Coneccion> conecciones = new List<Coneccion>();
    
    private EjeCardinal ejeCardinalActual = EjeCardinal.Norte;
    
    void Awake()
    {
        Norte.Inicializar(MeshNorte, EjeCardinal.Norte);
        NorEsteNorte.Inicializar(MeshNorEsteNorte, EjeCardinal.Norte);
        NorOesteNorte.Inicializar(MeshNorOesteNorte, EjeCardinal.Norte);
        
        Sur.Inicializar(MeshSur, EjeCardinal.Sur);
        SurEsteSur.Inicializar(MeshSurEsteSur, EjeCardinal.Sur);
        SurOesteSur.Inicializar(MeshSurOesteSur, EjeCardinal.Sur);
        
        Este.Inicializar(MeshEste, EjeCardinal.Este);
        EsteEsteNorte.Inicializar(MeshEsteEsteNorte, EjeCardinal.Este);
        EsteEsteSur.Inicializar(MeshEsteEsteSur, EjeCardinal.Este);
        
        Oeste.Inicializar(MeshOeste, EjeCardinal.Oeste);
        OesteOesteNorte.Inicializar(MeshOesteOesteNorte, EjeCardinal.Oeste);
        OesteOesteSur.Inicializar(MeshOesteOesteSur, EjeCardinal.Oeste);
        
        conecciones.Add(Norte);
        conecciones.Add(NorEsteNorte);
        conecciones.Add(NorOesteNorte);
        
        conecciones.Add(Sur);
        conecciones.Add(SurEsteSur);
        conecciones.Add(SurOesteSur);
        
        conecciones.Add(Este);
        conecciones.Add(EsteEsteNorte);
        conecciones.Add(EsteEsteSur);
        
        conecciones.Add(Oeste);
        conecciones.Add(OesteOesteNorte);
        conecciones.Add(OesteOesteSur);
    }

    void Update()
    {
        Debug.DrawLine(MeshNorte.transform.position + new Vector3(0, 0.3f, 0), MeshNorte.transform.position + new Vector3(1f, 0.3f, 0), Color.magenta);
    }
    
    public void Aleatorizar90()
    {
        Random.InitState((int)DateTime.Now.Ticks);
        
        int i = 0;
        while (i < conecciones.Count)
        {
            conecciones[i].Aleatorizar(Random.Range(0f, 1f));
            conecciones[i+1].Aleatorizar(0);
            conecciones[i+2].Aleatorizar(0);
            i += 3;
        }
        
        if (!AlMenosDosEjesDesbloqueados())
            Aleatorizar90();
    }

    public void Aleatorizar()
    {
        Random.InitState((int)DateTime.Now.Ticks);
        
        for (int i = 0; i < conecciones.Count; i++)
        {
            conecciones[i].Aleatorizar(Random.Range(0f, 1f));
        }
        
        if (!AlMenosDosEjesDesbloqueados())
            Aleatorizar();
    }
    
    public int VerificarEje(int EjesDesbloqueadosActuales, EjeCardinal EjeCardinalActual)
    {
        int EjesDesbloqueados = EjesDesbloqueadosActuales;

        Coneccion coneccionActual = null;
        for (int i = 0; i < conecciones.Count; i++)
        {
            coneccionActual = conecciones[i];
            if (!coneccionActual.EstaBloqueado && coneccionActual.EjeCardinalActual == EjeCardinalActual)
            {
                EjesDesbloqueados++;
                break;
            }
        }

        return EjesDesbloqueados;
    }

    public bool AlMenosDosEjesDesbloqueados()
    {
        int ejesDesbloqueados = VerificarEje(0, EjeCardinal.Norte);
        ejesDesbloqueados = VerificarEje(ejesDesbloqueados, EjeCardinal.Sur);
        ejesDesbloqueados = VerificarEje(ejesDesbloqueados, EjeCardinal.Este);
        ejesDesbloqueados = VerificarEje(ejesDesbloqueados, EjeCardinal.Oeste);

        return ejesDesbloqueados > 1;
    }
    
    public void MarcarConnecionesUsadas()
    {
        AnalizarPuertaYMarcarPuertaContraria(MeshNorte);
        AnalizarPuertaYMarcarPuertaContraria(MeshSur);
        AnalizarPuertaYMarcarPuertaContraria(MeshEste);
        AnalizarPuertaYMarcarPuertaContraria(MeshOeste);
    }

    private void AnalizarPuertaYMarcarPuertaContraria(GameObject meshConeccion)
    {
        if (meshConeccion.GetComponent<ConeccionReferencia>().referencia.EstaBloqueado)
            return;
        
        GameObject gameObjectConeccion = ConsigueConeccionParalela(meshConeccion);
        if (!gameObjectConeccion)
        {
            return;
        }

        ConeccionReferencia coneccionReferencia = gameObjectConeccion.GetComponent<ConeccionReferencia>();
        if (!coneccionReferencia)
        {
            return;
        }
        
        meshConeccion.GetComponent<ConeccionReferencia>().referencia.Usada = true;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private Vector3 CalcularDireccion(ConeccionReferencia coneccionReferencia, float rotacionY)
    {
        const float distancia = 0.5f;
        Vector3 direccionLocal = Vector3.zero;
        switch (coneccionReferencia.referencia.EjeCardinalActual)
        {
            case EjeCardinal.Norte:
                direccionLocal = Vector3.right;
                break;
            case EjeCardinal.Sur:
                direccionLocal = Vector3.left;
                break;
            case EjeCardinal.Este:
                direccionLocal = Vector3.back;
                break;
            case EjeCardinal.Oeste:
                direccionLocal = Vector3.forward;
                break;
        }

        return Quaternion.Euler(0f, rotacionY, 0f) * direccionLocal * distancia;
    }
    
    private GameObject ConsigueConeccionParalela(GameObject coneccion)
    {
        ConeccionReferencia coneccionReferencia = coneccion.GetComponent<ConeccionReferencia>();
        if (!coneccionReferencia)
        {
            return null;
        }
        
        Vector3 direccion = CalcularDireccion(coneccionReferencia, transform.eulerAngles.y);
        Vector3 posicion = coneccion.transform.position;
        LayerMask collisionLayers = Physics.DefaultRaycastLayers;
        Vector3 offsetInY = new Vector3(0, 0.3f, 0);
        if (!Physics.Linecast(posicion + offsetInY, posicion + offsetInY + direccion, 
                out RaycastHit hit, collisionLayers))
        {
            return null;
        }
        
        BoxCollider meshCol = hit.collider as BoxCollider;
        if (!meshCol)
        {
            return null;
        }
        
        return hit.collider.gameObject;
    }
}
