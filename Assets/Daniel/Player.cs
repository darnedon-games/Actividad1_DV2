using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    [SerializeField] private GameManager gameManager;

    private float inputH;
    private float inputV;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        inputH = Input.GetAxisRaw("Horizontal");
        inputV = Input.GetAxisRaw("Vertical");
        transform.Translate((new Vector3(inputH, 0, inputV)).normalized * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Door"))
        {
            Debug.Log("Door crossed");
            gameManager.ActivateCanvas();
        }
    }
}
