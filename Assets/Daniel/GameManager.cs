using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> board;
    [SerializeField] private Canvas selectionCanvas;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateCanvas()
    {
        if (selectionCanvas != null)
        {
            selectionCanvas.gameObject.SetActive(true);
        }
    }

    public void DeactivateCanvas()
    {
        if (selectionCanvas != null)
        {
            selectionCanvas.gameObject.SetActive(false);
        }
    }

    public void SetUpRoom()
    {
        Debug.Log("Nueva habitación colocada");
        DeactivateCanvas();
    }
}
