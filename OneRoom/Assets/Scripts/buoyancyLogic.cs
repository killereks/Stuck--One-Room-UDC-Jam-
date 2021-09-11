using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buoyancyLogic : MonoBehaviour {

    Transform water;
    Rigidbody rb;

    public float viscosity = 1f;
    public float force = 5f;

    public MeshFilter meshFilter;

    Matrix4x4 localToWorld;

    Vector3 defaultScale;

    private void Start() {
        rb = GetComponent<Rigidbody>();

        defaultScale = transform.localScale;
    }

    private void OnDrawGizmos() {
        /*Gizmos.color = Color.red;
        foreach (Vector3 vertex in meshFilter.sharedMesh.vertices) {
            Vector3 worldVertex = transform.TransformPoint(vertex);

            Gizmos.DrawRay(worldVertex, Vector3.up);
        }*/
    }

    private void FixedUpdate() {
        if (water != null) {
            if (transform.position.y < water.position.y) {
                rb.AddForce(Vector3.up * ( water.position.y - transform.position.y ) * force);
                rb.AddForce(rb.velocity * -viscosity);

                // per vertex
                /*foreach (Vector3 vertex in meshFilter.sharedMesh.vertices) {
                    Vector3 worldVertex = transform.TransformPoint(vertex);
                    // it is above water, ignore
                    if (worldVertex.y > water.position.y) {
                        continue;
                    }

                    rb.AddForceAtPosition(Vector3.up * (water.position.y - worldVertex.y) * force, worldVertex);
                }*/
                rb.AddForce(rb.velocity * -viscosity);
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Buoyancy")) {
            water = other.transform;

            /*transform.localScale = new Vector3(transform.localScale.x / water.localScale.x,
                                               transform.localScale.y / water.localScale.y,
                                               transform.localScale.z / water.localScale.z);
            transform.parent = water;*/

            Vector3 tempScale = transform.localScale;

            transform.SetParent(water, true);

            transform.localScale = tempScale;
            //transform.localScale = defaultScale;
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Buoyancy")) {
            water = null;
            transform.parent = null;
            transform.localScale = defaultScale;
        }
    }
}
