using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VortexManager : MonoBehaviour
{
    public bool Clockwise;
    public float maxForce = 1;
    public float maxTorque = 0.01f;
    
    public float buoncyForce = -0.1f;

    public GameObject particlePrefab;
    public bool showDebugRays;

    float radius;
    const float halfPI = Mathf.PI / 2;


    List<ConstantForce> particles = new List<ConstantForce>();

    void Start()
    {
        if (transform.localScale.x != transform.localScale.z)
            throw new System.Exception("X Scale must equal Y Scale");

        radius = transform.localScale.x;


        particles.Add(particlePrefab.GetComponent<ConstantForce>());
    }

    Vector3 CalculateForce(Vector3 worldPos)
    {
        var localPos = transform.InverseTransformPoint(worldPos);
        var distance = Mathf.Sqrt(Mathf.Pow(localPos.x, 2) + Mathf.Pow(localPos.z, 2));
        var angle = halfPI + Mathf.Atan2(localPos.z, localPos.x);
        var localForce = maxForce * (radius - distance) / radius * 
                         new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
        var worldForce = transform.TransformVector(localForce);

        if (Clockwise)
            worldForce = -worldForce;

        worldForce += new Vector3(0, buoncyForce, 0);

        if (float.IsNaN(worldForce.x) || float.IsNaN(worldForce.y) || float.IsNaN(worldForce.z))
            return Vector3.zero;

        return worldForce;
    }

    Vector3 CalculateTorque(Vector3 worldPos)
    {
        var localPos = transform.InverseTransformPoint(worldPos);
        var distance = Mathf.Sqrt(Mathf.Pow(localPos.x, 2) + Mathf.Pow(localPos.z, 2));
        var angle = halfPI + Mathf.Atan2(localPos.z, localPos.x);
        var localTorque = maxTorque * (distance / radius) *
                          new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
        var worldTorque = transform.TransformVector(localTorque);

        if (Clockwise)
            worldTorque = -worldTorque;

        if (float.IsNaN(worldTorque.x) || float.IsNaN(worldTorque.y) || float.IsNaN(worldTorque.z))
            return Vector3.zero;

        return worldTorque;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
            SpawnParticle();

        if (Input.GetKey(KeyCode.R))
            Clockwise = !Clockwise;
    }

    void FixedUpdate()
    {

        for (int i = 0; i< particles.Count; i++)
        {
            var cf = particles[i];
            cf.force = CalculateForce(cf.transform.position);
//            cf.torque = CalculateTorque(cf.transform.position);
            if (showDebugRays)
                Debug.DrawRay(cf.transform.position, cf.force);
        }
    }

    void SpawnParticle()
    {
        var g = Instantiate(particlePrefab) as GameObject;
        g.transform.position = transform.position;
        g.transform.position += new Vector3(Random.Range(-0.3f, 0.3f),
                                            Random.Range(-0.3f, 0.3f),
                                            Random.Range(-0.3f, 0.3f));
        g.transform.rotation = Quaternion.Euler(Random.Range(0, 360), 
                                                Random.Range(0, 360), 
                                                Random.Range(0, 360));
        particles.Add(g.GetComponent<ConstantForce>());
    }
}
