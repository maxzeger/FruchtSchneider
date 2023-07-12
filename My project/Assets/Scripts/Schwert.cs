using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using EzySlice;
using UnityEngine.XR.Interaction.Toolkit;

public class Schwert : MonoBehaviour
{
    public GameObject controller;
    public GameObject quitFruit;
    public Material bombFill;
    public Material fruitFill;
    public XRBaseController XRcontroller;
    // Start is called before the first frame update
    void Start()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        if(!other.gameObject.CompareTag("bomb")) XRcontroller.SendHapticImpulse(0.5f, 0.2f);
        if(other.gameObject.CompareTag("bomb")) XRcontroller.SendHapticImpulse(1.0f, 1.0f);


        if (other.gameObject.CompareTag("start")){
            controller.GetComponent<Spielverwaltung>().StartRound();
            other.gameObject.SetActive(false);
            quitFruit.SetActive(false);
        }else if(other.gameObject.CompareTag("quit")){
            Application.Quit();
        }
        else if(other.gameObject.CompareTag("fruit") || other.gameObject.CompareTag("bomb")){
            GameObject fruitToSlice = other.gameObject;
            

            MeshRenderer fruitRenderer = fruitToSlice.GetComponent<MeshRenderer>();
            MeshFilter fruitFilter = fruitToSlice.GetComponent<MeshFilter>();
            Mesh fruitMesh = fruitFilter.mesh;
            

            Vector3 cuttingPlanePosition = transform.position;
            Vector3 cuttingPlaneNormal = transform.right;

            SlicedHull result = fruitToSlice.Slice(cuttingPlanePosition, cuttingPlaneNormal, null);
            Material currentFill = fruitFill;
            if(other.gameObject.CompareTag("bomb")) currentFill = bombFill;

            GameObject upperHullGameObject = result.CreateUpperHull(fruitToSlice, currentFill);
            GameObject lowerHullGameObject = result.CreateLowerHull(fruitToSlice, currentFill);
            AddComponents(upperHullGameObject);
            AddComponents(lowerHullGameObject);
            Destroy(fruitToSlice);
            int score = 10;
            if(other.gameObject.CompareTag("bomb")) score = -50;
            controller.GetComponent<Spielverwaltung>().ChangeScore(score);

            
        }
    }

    public void AddComponents(GameObject objPart)
    {

        objPart.AddComponent<BoxCollider>();

        objPart.AddComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;
        objPart.GetComponent<Rigidbody>().AddExplosionForce(350, objPart.transform.position, 30);
        Destroy(objPart, 3.0f);
    }

    // Update is called once per frame
    void Update()
    {

    }
}