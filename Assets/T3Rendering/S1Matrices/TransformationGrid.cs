using System.Collections.Generic;
using UnityEngine;

namespace T3Rendering.S1Matrices
{
    public class TransformationGrid : MonoBehaviour
    {
        [SerializeField] private Transform prefab = null;
        [SerializeField] private int gridResolution = 10;
        private Matrix4x4 transformation;
        private Transform[] grid;
        private List<Transformation> transformations;

        private void Awake()
        {
            transformations = new List<Transformation>();
            grid = new Transform[gridResolution * gridResolution * gridResolution];
            for (int i = 0, z = 0; z < gridResolution; z++)
            {
                for (var y = 0; y < gridResolution; y++)
                {
                    for (var x = 0; x < gridResolution; x++, i++)
                    {
                        grid[i] = CreateGridPoint(x, y, z);
                    }
                }
            }
        }

        private void Update()
        {
            UpdateTransformation();
            for (int i = 0, z = 0; z < gridResolution; z++)
            {
                for (var y = 0; y < gridResolution; y++)
                {
                    for (var x = 0; x < gridResolution; x++, i++)
                    {
                        grid[i].localPosition = TransformPoint(x, y, z);
                    }
                }
            }
        }

        private void UpdateTransformation()
        {
            GetComponents(transformations);
            if (transformations.Count > 0)
            {
                transformation = transformations[0].Matrix;
                for (var i = 1; i < transformations.Count; i++)
                {
                    transformation = transformations[i].Matrix * transformation;
                }
            }

            transformation = Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale) *
                             transformation;
        }

        private Vector3 TransformPoint(int x, int y, int z)
        {
            Vector3 coordinates = GetCoordinates(x, y, z);
            return transformation.MultiplyPoint(coordinates);
        }

        private Transform CreateGridPoint(int x, int y, int z)
        {
            Transform point = Instantiate(prefab);
            point.localPosition = GetCoordinates(x, y, z);
            point.GetComponent<Renderer>().material.color = new Color((float) x / gridResolution,
                (float) y / gridResolution, (float) z / gridResolution);
            return point;
        }

        private Vector3 GetCoordinates(int x, int y, int z)
        {
            return new Vector3(x - (gridResolution - 1) * 0.5f, y - (gridResolution - 1) * 0.5f,
                z - (gridResolution - 1) * 0.5f);
        }
    }
}