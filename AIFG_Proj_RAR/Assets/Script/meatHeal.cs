using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meatHeal : MonoBehaviour
{

   // public PlayerMovements playerMVM;
    //public GameObject playerObject;

    float yPos = 5;
    void Awake()
    {

        
    }
    // Start is called before the first frame update
    void Start()
    {
      
         //playerMVM = gameObject.GetComponent<PlayerMovements>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //gameObject.transform.position = new Vector3(0,yPos,0);
    }
    

     public void OnTriggerEnter(Collider other) 
     {

        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerMovements>().HP += 10;

        //layerMVM.HP += 10;       
        Debug.Log("Health++");
        Destroy(gameObject);
        }
      }
}
