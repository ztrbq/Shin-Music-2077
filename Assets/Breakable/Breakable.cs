using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    Mesh mesh;
    MeshRenderer meshRenderer;

    public static Stack<GameObject> piecePool = new Stack<GameObject>();

    GameObject NewPiece()
    {
        if (piecePool.Count > 0)
        {
            GameObject piece = piecePool.Pop();
            piece.SetActive(true);
            return piece;
        }
        else
        {
            GameObject piece = new GameObject("Piece");
            MeshCollider co = piece.AddComponent<MeshCollider>();
            co.convex = true;
            MeshRenderer piece_render = piece.AddComponent<MeshRenderer>();
            piece.AddComponent<MeshFilter>();
            Rigidbody rig = piece.AddComponent<Rigidbody>();
            rig.useGravity = false;
            return piece;
        }
    }
    public void DestroyPiece()
    {
        gameObject.SetActive(false);
        piecePool.Push(gameObject);
    }
    public void DestroyPiece(float delay)
    {
        Invoke("DestroyPiece", delay);
    }

    [HideInInspector]
    public int recursive = 2;
    [HideInInspector]
    public Vector3 breakPoint = Vector3.zero;

    public void Initialize()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        meshRenderer = GetComponent<MeshRenderer>();
        if (recursive == 0) DestroyPiece(2);
        else if (recursive == 1) PieceUp();
    }

    private void Start()
    {
        Initialize();
    }

    static int[] Coordinates = {2,3,2,3,1,0,1,0,2,3,1,0,1,2,3,0,3,3,0,0,1,1,2,2};
    static int GetCoordinate(int i)
    {
        return Coordinates[i];
    }

    private Mesh[] GenerateMesh(Mesh mesh, Vector3 breakPoint)
    {
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;
        Vector3[] normals = mesh.normals;

        float[] randoms = new float[4];
        for (int i = 0; i < 4; i++)
        {
            randoms[i] = (float)Random.Range(10f, 90f) / 100f;
        }

        Mesh[] targets = new Mesh[4];
        for (int i = 0; i < 4; i++) targets[i] = new Mesh();
        foreach (Mesh target in targets)
        {
            target.vertices = vertices;
            target.normals = normals;
            target.triangles = triangles;
            target.uv = mesh.uv;
        }

        Vector3[] vectors0 = new Vector3[24];
        for (int i = 0; i < 24; i++)
        {
            Vector3 v = targets[0].vertices[i];
            if(GetCoordinate(i) == 2)
            {
                v.x = breakPoint.x;
                v.z = breakPoint.z;
            }
            else if(GetCoordinate(i) == 3)
            {
                v.z = Mathf.Lerp(vertices[5].z, vertices[1].z, randoms[3]);
                v.x = Mathf.Lerp(vertices[5].x, vertices[1].x, randoms[3]);
            }
            else if(GetCoordinate(i) == 1)
            {
                v.x = Mathf.Lerp(vertices[5].x, vertices[4].x, randoms[0]);
                v.z = Mathf.Lerp(vertices[5].z, vertices[4].z, randoms[0]);
            }
            vectors0[i] = v;
        }
        targets[0].vertices = vectors0;

        Vector3[] vectors1 = new Vector3[24];
        for (int i = 0; i < 24; i++)
        {
            Vector3 v = targets[1].vertices[i];
            if (GetCoordinate(i) == 3)
            {
                v.x = breakPoint.x;
                v.z = breakPoint.z;
            }
            else if (GetCoordinate(i) == 2)
            {
                v.x = Mathf.Lerp(vertices[4].x, vertices[0].x, randoms[1]);
                v.z = Mathf.Lerp(vertices[4].z, vertices[0].z, randoms[1]);
            }
            else if (GetCoordinate(i) == 0)
            {
                v.x = Mathf.Lerp(vertices[5].x, vertices[4].x, randoms[0]);
                v.z = Mathf.Lerp(vertices[5].z, vertices[4].z, randoms[0]);
            }
            vectors1[i] = v;
        }
        targets[1].vertices = vectors1;

        Vector3[] vectors2 = new Vector3[24];
        for (int i = 0; i < 24; i++)
        {
            Vector3 v = targets[2].vertices[i];
            if (GetCoordinate(i) == 0)
            {
                v.x = breakPoint.x;
                v.z = breakPoint.z;
            }
            else if (GetCoordinate(i) == 1)
            {
                v.x = Mathf.Lerp(vertices[4].x, vertices[0].x, randoms[1]);
                v.z = Mathf.Lerp(vertices[4].z, vertices[0].z, randoms[1]);
            }
            else if (GetCoordinate(i) == 3)
            {
                v.x = Mathf.Lerp(vertices[1].x, vertices[0].x, randoms[2]);
                v.z = Mathf.Lerp(vertices[1].z, vertices[0].z, randoms[2]);
            }
            vectors2[i] = v;
        }
        targets[2].vertices = vectors2;

        Vector3[] vectors3 = new Vector3[24];
        for (int i = 0; i < 24; i++)
        {
            Vector3 v = targets[3].vertices[i];
            if (GetCoordinate(i) == 1)
            {
                v.x = breakPoint.x;
                v.z = breakPoint.z;
            }
            else if (GetCoordinate(i) == 0)
            {
                v.z = Mathf.Lerp(vertices[5].z, vertices[1].z, randoms[3]);
                v.x = Mathf.Lerp(vertices[5].x, vertices[1].x, randoms[3]);
            }
            else if (GetCoordinate(i) == 2)
            {
                v.x = Mathf.Lerp(vertices[1].x, vertices[0].x, randoms[2]);
                v.z = Mathf.Lerp(vertices[1].z, vertices[0].z, randoms[2]);
            }
            vectors3[i] = v;
        }
        targets[3].vertices = vectors3;

        return targets;
    }

    private Rigidbody GeneratePiece(Mesh piece_mesh, MeshRenderer meshRenderer)
    {
        GameObject piece = NewPiece();
        MeshCollider co = piece.GetComponent<MeshCollider>();
        co.sharedMesh = piece_mesh;
        piece.transform.position = transform.position;
        piece.transform.localScale = transform.localScale;
        piece.transform.rotation = transform.rotation;
        piece.GetComponent<MeshFilter>().mesh = piece_mesh;
        MeshRenderer piece_render = piece.GetComponent<MeshRenderer>();
        piece_render.material = meshRenderer.material;
        return piece.GetComponent<Rigidbody>();
    }

    public void PieceUp(Vector3 breakPosition)
    {
        Mesh[] meshes = GenerateMesh(mesh, breakPosition);
        Rigidbody[] pieces = new Rigidbody[4];
        for (int i = 0; i < 4; i++) pieces[i] = GeneratePiece(meshes[i], meshRenderer);
        if(recursive == 1) DestroyPiece();
        for (int i = 0; i < 4; i++)
        {
            pieces[i].AddExplosionForce(500, transform.position - transform.up * 0.2f, 2);
            pieces[i].AddForce(Vector3.back * 750);
            Breakable b = pieces[i].gameObject.AddComponent<Breakable>();
            b.recursive = recursive - 1;
            switch (i)
            {
                case 0:
                    b.breakPoint = breakPosition + mesh.vertices[5] / 2;
                    break;
                case 1:
                    b.breakPoint = breakPosition + mesh.vertices[4] / 2;
                    break;
                case 2:
                    b.breakPoint = breakPosition + mesh.vertices[0] / 2;
                    break;
                case 3:
                    b.breakPoint = breakPosition + mesh.vertices[1] / 2;
                    break;
            }
            //Destroy(pieces[i].gameObject, 2);
        }
    }

    public void PieceUp()
    {
        PieceUp(breakPoint);
    }

    /*private void Update()
    {
        if (recursive == 2 && transform.position.z <= -0.3f)
        {
            PieceUp();
            QuadPool.Die(gameObject);
        }
    }*/
}
