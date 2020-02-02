using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeModifier : MonoBehaviour
{
    [SerializeField] float maxDistanceFromCollisionPoint;

    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private GameObject DEBUG_collisionPointPrefab;
    [SerializeField] private bool DEBUG = false;

    [SerializeField] private float magnitudeScale = 1f;

    private Vector3[] vertices;
    private int vertexCount;

    private void Start()
    {
        vertices = meshFilter.mesh.vertices;
        vertexCount = meshFilter.mesh.vertexCount;
    }

    private void OnCollisionEnter(Collision pCollision)
    {
        var deformationVector = pCollision.relativeVelocity;
        var contactPoints = pCollision.contacts;

        ModifyMeshPoints(contactPoints, deformationVector);
    }

    public void ModifyMeshPoints(ContactPoint[] contactPoints, Vector3 deformationVector)
    {
        List<int> meshVerticesIndex = new List<int>();
        foreach (var contactPoint in contactPoints)
        {
            AddNearVertices(contactPoint.point, meshVerticesIndex);
        }

        foreach (var vertexIndex in meshVerticesIndex)
        {   
            if(DEBUG)
            {
                var debugPoint = Instantiate(DEBUG_collisionPointPrefab, transform.position+vertices[vertexIndex], Quaternion.identity);
                StartCoroutine(DestroyDebugPoint(debugPoint));
            } else
            {
                var v = vertices[vertexIndex];
                var vertexNormal = v.normalized;
                vertices[vertexIndex] -= vertexNormal * deformationVector.magnitude / magnitudeScale;
            }
        }

        meshFilter.mesh.vertices = vertices;
        meshFilter.mesh.MarkModified();
        meshFilter.mesh.UploadMeshData(false);
        //GetComponent<MeshCollider>().sharedMesh = null;
        GetComponent<MeshCollider>().sharedMesh = meshFilter.mesh;
    }

    private IEnumerator DestroyDebugPoint(GameObject debugPoint)
    {
        yield return new WaitForSeconds(1.5f);

        Destroy(debugPoint);
    }

    int meme = 0;

    private void AddNearVertices(Vector3 point, List<int> nearVertices)
    {
        
        for (int i = 0; i < vertexCount; i++)
        {
            meme++;
            var vertexWorldPos = transform.TransformPoint(vertices[i]);
            var distance = Vector3.Magnitude(point - vertexWorldPos);

            if (meme < 10)
            {
                Debug.Log("D: " + distance);
                Debug.Log("P: " + transform.position);
                Debug.Log("V: " + vertices[i]);
                Debug.Log("T: " + transform.TransformPoint(vertices[i]));
            }
            if (distance < maxDistanceFromCollisionPoint)
            {
                nearVertices.Add(i);
            }
        }
    }
}

