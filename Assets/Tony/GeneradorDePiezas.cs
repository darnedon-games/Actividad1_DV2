using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;

public class GeneradorDePiezas: MonoBehaviour
{
    [SerializeField]
    GameObject PiezaBase;

    [SerializeField]
    bool Desactivar = false;
    
    private List<GameObject> piezas = new List<GameObject>();

    private int counterX = 0;
    private int counterY = 0;
    private int counterZ = 0;

    public GameObject piezaActual = null;
    
    void Start()
    {
                
    }
    
    void Update()
    {
        
    }
    
    public GameObject Generar()
    {
        Vector3 spawnPosition = new Vector3(1000, 0, 0);
        Vector3 spawnRotation = Vector3.zero;
    
        piezaActual = Instantiate(
            PiezaBase, 
            spawnPosition, 
            Quaternion.Euler(spawnRotation)
        );
        PiezaCuadrada piezaCuadradaComponente = piezaActual.GetComponent<PiezaCuadrada>();
        piezaCuadradaComponente.Aleatorizar90();
        
        HacerTransparentePieza(piezaActual);

        return piezaActual;
    }

    void GenerarTablero()
    {
        if (Desactivar || piezas.Count > 1000 || PiezaBase is null)
            return;
        
        Vector3 spawnPosition = new Vector3(counterX, counterY, counterZ);
        Vector3 spawnRotation = Vector3.zero;
    
        GameObject newObject = Instantiate(
            PiezaBase, 
            spawnPosition, 
            Quaternion.Euler(spawnRotation)
        );
        PiezaCuadrada piezaCuadradaComponente = newObject.GetComponent<PiezaCuadrada>();
        piezaCuadradaComponente.Aleatorizar();

        piezas.Add(newObject);
        
        counterX += 5;
        if (counterX <= 100) 
            return;
        
        counterX = 0;
        counterZ += 5;
    }

    public GameObject EntregaPiezaActual()
    {
        GameObject newObject = piezaActual;
        QuitarTranparenciaPieza(newObject);
        piezaActual = null;
        
        return newObject;
    }

    private void HacerTransparentePieza(GameObject pieza)
    {
        AplicarTransparencia(pieza, 0.1f, true);
    }

    private void QuitarTranparenciaPieza(GameObject pieza)
    {
        AplicarTransparencia(pieza, 1f, false);
    }

    private void AplicarTransparencia(GameObject pieza, float alpha, bool transparente)
    {
        MeshRenderer[] renderers = pieza.GetComponentsInChildren<MeshRenderer>(true);
        for (int i = 0; i < renderers.Length; i++)
        {
            Material material = renderers[i].material;
            ConfigurarSuperficie(material, transparente);
            Color color = material.HasProperty("_BaseColor")
                ? material.GetColor("_BaseColor")
                : material.color;
            color.a = alpha;
            if (material.HasProperty("_BaseColor"))
                material.SetColor("_BaseColor", color);
            material.color = color;
        }
    }

    private static void ConfigurarSuperficie(Material material, bool transparente)
    {
        if (transparente)
        {
            material.SetFloat("_Surface", 1f);
            material.SetOverrideTag("RenderType", "Transparent");
            material.SetFloat("_Blend", 0f);
            material.SetFloat("_SrcBlend", (float)BlendMode.SrcAlpha);
            material.SetFloat("_DstBlend", (float)BlendMode.OneMinusSrcAlpha);
            material.SetFloat("_SrcBlendAlpha", (float)BlendMode.One);
            material.SetFloat("_DstBlendAlpha", (float)BlendMode.OneMinusSrcAlpha);
            material.SetFloat("_ZWrite", 0f);
            material.SetFloat("_BlendModePreserveSpecular", 0f);
            material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            material.DisableKeyword("_ALPHATEST_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.DisableKeyword("_ALPHAMODULATE_ON");
            material.renderQueue = (int)RenderQueue.Transparent;
        }
        else
        {
            material.SetFloat("_Surface", 0f);
            material.SetOverrideTag("RenderType", "Opaque");
            material.SetFloat("_SrcBlend", (float)BlendMode.One);
            material.SetFloat("_DstBlend", (float)BlendMode.Zero);
            material.SetFloat("_SrcBlendAlpha", (float)BlendMode.One);
            material.SetFloat("_DstBlendAlpha", (float)BlendMode.Zero);
            material.SetFloat("_ZWrite", 1f);
            material.DisableKeyword("_SURFACE_TYPE_TRANSPARENT");
            material.DisableKeyword("_ALPHATEST_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.DisableKeyword("_ALPHAMODULATE_ON");
            material.renderQueue = (int)RenderQueue.Geometry;
        }
    }
    
}
