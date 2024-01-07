using System.Collections.Generic;
using UnityEngine;
public class DiceGenerator : MonoBehaviour
{
    #region Variables

    [SerializeField] private DiceSide facePrefab;
    [SerializeField] private int verticesPerFace = 5;
    [SerializeField] private List<int> faceNumbers;
    
    #endregion

    #region Public Methods

    public void GenerateDiceFaces()
    {
        #region Local Variables
        
        MeshCollider meshCollider = GetComponent<MeshCollider>();
        List<Vector3> meshVerticesPos = new List<Vector3>();
        List<List<Vector3>> faces = new List<List<Vector3>>();

        #endregion
        ////////////////////////////////////////////////////////////////////////
        #region ErrorLogs

        if (meshCollider == null)
        {
            Debug.LogError("MeshCollider was not found on this dice!");
            return;
        }
        if (meshCollider.sharedMesh == null)
        {
            Debug.LogError("Please add a mesh to your MeshCollider!");
            return;
        }
        if (facePrefab == null)
        {
            Debug.LogError("Please add facePrefab!");
            return;
        }

        #endregion
        ////////////////////////////////////////////////////////////////////////
        #region Cleaning

        //clean the faces during regeneration
        faceNumbers.Clear();
        while(transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        #endregion
        ////////////////////////////////////////////////////////////////////////
        #region Face vertices
        
        //Get the positions of all vertices
        foreach (Vector3 vertex in meshCollider.sharedMesh.vertices)
        {
            meshVerticesPos.Add(transform.TransformPoint(vertex));
        }
        if (meshVerticesPos.Count % verticesPerFace != 0)
        {
            Debug.LogError("The number of vertices in the mesh of the MeshCollider is not a multiple of [verticesPerFace]! " +
                           "Please enter the correct [verticesPerFace]");
            return;
        }
        //Create vertex lists for each side
        for (int i = 0; i < meshVerticesPos.Count; i += verticesPerFace)
        {
            List<Vector3> faceVertices = new List<Vector3>();
            for (int j = 0; j < verticesPerFace; j++)
                faceVertices.Add(meshVerticesPos[i + j]);
            
            faces.Add(faceVertices);
        }

        #endregion
        ////////////////////////////////////////////////////////////////////////
        #region Create dice faces
        
        for (int i = 0; i < faces.Count; i++)
        {
            faceNumbers.Add(i + 1);
            
            // Calculate the center of coordinates for a face
            Vector3 faceCenter = Vector3.zero;
            foreach (Vector3 vertex in faces[i])
            {
                faceCenter += vertex;
            }
            faceCenter /= faces[i].Count;
            
            // Find the normal to the face
            Vector3 faceNormal = Vector3.Cross(faces[i][1] - faces[i][0], faces[i][2] - faces[i][0]).normalized;

            // Spawn a face prefab and set its position and rotation
            DiceSide spawnedSide = Instantiate(facePrefab, faceCenter, Quaternion.identity, transform);
            spawnedSide.transform.rotation = Quaternion.LookRotation(faceNormal, transform.up);
        }

        #endregion
        UpdateFaces();
    }

    public void UpdateFaces()
    {
        if (transform.childCount != faceNumbers.Count)
        {
            Debug.LogError("Face count must be equal to the FaceNumbers count! " +
                           "Please manually resize the FaceNumbers list " +
                           "or click the [GENERATE DICE FACES] button to regenerate the dice");
            return;
        }
        
        //set face value, and set the value of the parallel face that will be at the top
        
        //ToDo переробити алгоритм :)
        int faceIndex = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (faceIndex >= 4) faceIndex = 0;
            faceIndex++;

            int faceNumber = faceNumbers[i];
            int topSideNumber = faceIndex <= 2 ? faceNumber + 2 : faceNumber - 2;

            DiceSide face = transform.GetChild(i).gameObject.GetComponent<DiceSide>();
            if (face == null) return;
            
            face.SetSideValue(faceNumber, topSideNumber);
            face.gameObject.name = faceNumber.ToString();
        }
    }

    #endregion
}