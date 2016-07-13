using UnityEngine;
using System.Collections;

public class IonBehavior : MonoBehaviour 
{

    public Color negativeChargeColor = Color.gray;
    public Color positiveChargeColor = Color.white;
    public Color bondedColor = Color.red;

    public float minimumStartingSpeed = 2f;
    public float maximumStartingSpeed = 20f;

    public bool flashWallOnCollide = false;

    public float meanSize = 1;
    public float stdSize = 0.2f;

    bool? charged = true;
    Rigidbody _rigidbody;
    Renderer renderer;

    void Start () 
    {
        _rigidbody = gameObject.GetComponent<Rigidbody>();

        Vector3 newVel = new Vector3(Random.Range(-1f, 1f), 
                                     Random.Range(-1f, 1f), 
                                     Random.Range(-1f, 1f));
        newVel.Normalize();
        newVel *= Random.Range(minimumStartingSpeed, maximumStartingSpeed);
        _rigidbody.velocity = newVel;
        _rigidbody.rotation = Quaternion.Euler(new Vector3(Random.Range(0, 360),
                                                          Random.Range(0, 360),  
                                                          Random.Range(0, 360)));

        renderer = gameObject.GetComponent<Renderer>();

        float randNormal = normal(meanSize, stdSize);

        transform.localScale = new Vector3(randNormal, randNormal, randNormal);
	}

    public static float normal(float mean, float std)
    {
        float u1 = Random.Range(0f, 1f);
        float u2 = Random.Range(0f, 1f);
        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) *
                              Mathf.Sin(2.0f * Mathf.PI * u2);

        return mean + std * randStdNormal;
    }

    public void SetCharge(bool? newCharge)
    {
        charged = newCharge;
        UpdateMaterial();
    }

	void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains("Ion"))
        {
            IonBehavior other = collision.gameObject.GetComponent<IonBehavior>();

            if (other.charged != null && charged != null)
            {
                if (other.charged != charged)
                {
                    FixedJoint fixedJoint = gameObject.AddComponent<FixedJoint>();
                    fixedJoint.connectedBody = collision.gameObject.GetComponent<Rigidbody>();
                    fixedJoint.enableCollision = false;
                    fixedJoint.breakForce = 110f;
                    fixedJoint.breakTorque = 110f;

                    SetCharge(null);
                    other.SetCharge(null);
                }
            }
        }
        else
        {
            if (charged != null)
            { 
                SetCharge(!charged);
                if (flashWallOnCollide)
                { 
                    MaterialFlashReset mflash = collision.gameObject.GetComponent<MaterialFlashReset>();
                    mflash.Flash();
                }
            }
        }
    }

    void OnJointBreak(float breakForce)
    {
        FixedJoint fixedJoint = gameObject.GetComponent<FixedJoint>();

        IonBehavior otherIon = fixedJoint.connectedBody.transform.GetComponent<IonBehavior>();
        otherIon.SetCharge(false);

        SetCharge(true);

        Vector3 direction = (_rigidbody.position - fixedJoint.connectedBody.position).normalized * (breakForce * 0.0000035f);
        _rigidbody.AddForce(direction);

        direction = (fixedJoint.connectedBody.position - _rigidbody.position).normalized * (breakForce * 0.0000035f);
        fixedJoint.connectedBody.AddForce(direction);
    }

    public void UpdateMaterial()
    {
        if (charged == null)
            renderer.material.color = bondedColor;
        else if (charged == true)
            renderer.material.color = positiveChargeColor;
        else
            renderer.material.color = negativeChargeColor;
    }
}
