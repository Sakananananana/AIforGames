using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meatHeal : MonoBehaviour
{

    public Rigidbody rb;
    PlayerMovements playerMVM;

    float yPos = 5;
    void Awake()
    {

        rb.AddForce(transform.up * 1);
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
         playerMVM = gameObject.GetComponent<PlayerMovements>();
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = new Vector3(0,yPos,0);
    }
    

     private void OnTriggerEnter(Collider other) 
     {
      //playerMVM.HP += 10;       
        Debug.Log("Health++");
        Destroy(gameObject);

      }
}
