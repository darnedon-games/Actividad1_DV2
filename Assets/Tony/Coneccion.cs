using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class Coneccion
{
    [SerializeField]
    public bool EstaBloqueado = true;

    private MeshRenderer _meshRendererRef;
    private BoxCollider _boxColliderRef;
    
    private GameObject prefab;
    
    public EjeCardinal EjeCardinalActual = EjeCardinal.Norte;
    
    public bool Usada = false;

    public void Inicializar(GameObject meshReference, EjeCardinal ejeCardinalActual)
    {
        prefab = meshReference;
        _meshRendererRef = prefab.GetComponent<MeshRenderer>();
        _boxColliderRef = prefab.GetComponent<BoxCollider>();
        prefab.GetComponent<ConeccionReferencia>().referencia = this;

        EjeCardinalActual = ejeCardinalActual;

        Update();
    }

    public void Update()
    {
        if (EstaBloqueado)
        {
            _meshRendererRef.material.color = Color.red;
            if (_boxColliderRef)
                _boxColliderRef.isTrigger = false;
        }
        else
        {
            _meshRendererRef.material.color = Color.green;
            if (_boxColliderRef)
                _boxColliderRef.isTrigger = true;
        }
    }

    public void Aleatorizar(float range)
    {
        EstaBloqueado = range < 0.75f;
        prefab.tag = EstaBloqueado ? "Pared" : "Coneccion";
        Update();
    }
}
