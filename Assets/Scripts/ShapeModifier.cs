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
            var v = vertices[vertexIndex];
            var vertexNormal = v.normalized;
            vertices[vertexIndex] -= vertexNormal * 0.005f;
            
            if(DEBUG)
            {
                var debugPoint = Instantiate(DEBUG_collisionPointPrefab, transform.position+vertices[vertexIndex], Quaternion.identity);
                StartCoroutine(DestroyDebugPoint(debugPoint));
            }
        }

        meshFilter.mesh.vertices = vertices;
        meshFilter.mesh.MarkModified();
        meshFilter.mesh.UploadMeshData(false);
    }

    private IEnumerator DestroyDebugPoint(GameObject debugPoint)
    {
        yield return new WaitForSeconds(1.5f);

        Destroy(debugPoint);
    }

    private void AddNearVertices(Vector3 point, List<int> nearVertices)
    {
        for (int i = 0; i < vertexCount; i++)
        {
            var vertexWorldPos = transform.position + vertices[i];
            var distance = Vector3.Magnitude(point - vertexWorldPos);

            if (distance < maxDistanceFromCollisionPoint)
            {
                nearVertices.Add(i);
            }
        }
    }
}

