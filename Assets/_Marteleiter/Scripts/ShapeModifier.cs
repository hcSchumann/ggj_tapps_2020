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
    [SerializeField] private int vertexCount;

    private void Start()
    {
        vertices = meshFilter.mesh.vertices;
        vertexCount = meshFilter.mesh.vertexCount;
    }

    private void OnCollisionEnter(Collision pCollision)
    {
        var hammerSwing = pCollision.rigidbody.GetComponentInParent<HammerSwing>();

        if (hammerSwing == null)
        {
            return;
        }

        hammerSwing.StopSwing();

        var contactPoints = pCollision.contacts;

        ModifyMeshPoints(contactPoints, hammerSwing.swingForce);
    }

    public void ModifyMeshPoints(ContactPoint[] contactPoints, float impactForce)
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
                vertices[vertexIndex] -= vertexNormal * impactForce / magnitudeScale;
            }
        }

        meshFilter.mesh.vertices = vertices;
        meshFilter.mesh.MarkModified();
        meshFilter.mesh.UploadMeshData(false);
        GetComponent<MeshCollider>().sharedMesh = meshFilter.mesh;
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
            var vertexWorldPos = transform.TransformPoint(vertices[i]);
            var distance = Vector3.Magnitude(point - vertexWorldPos);
            if (distance < maxDistanceFromCollisionPoint)
            {
                nearVertices.Add(i);
            }
        }
    }
}

