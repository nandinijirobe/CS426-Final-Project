using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SmartRoad : MonoBehaviour
{

    [SerializeField]
    public Queue<CarAI> trafficQueue = new Queue<CarAI>();
    public CarAI currentCar;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            var car = other.GetComponent<CarAI>();
            if(car != null && car != currentCar && car.IsThisLastPathIndex() == false)
            {
                // check if current car in intersection is going to turn
                // if (currentCar != null && Vector3.Dot(car.transform.forward.normalized, (currentCar.transform.position - car.transform.position).normalized) > 0)  {
                    car.Stop = true;
                // }
                trafficQueue.Enqueue(car);
            }
        }
    }

    private void Update()
    {
        if((currentCar == null) && (trafficQueue.Count > 0))
        {
            currentCar = trafficQueue.Dequeue();
            currentCar.Stop = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            var car = other.GetComponent<CarAI>();
            if(car != null)
            {
                RemoveCar(car);
            }
        }
    }

    private void RemoveCar(CarAI car)
    {
        if(car == currentCar)
        {
            currentCar = null;
        }
    }
}