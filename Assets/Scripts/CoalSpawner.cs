using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CoalSpawner : MonoBehaviour
{
    [SerializeField] Rect area;
    [SerializeField] GameObject coal;
    [HideInInspector] public float discRadius;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector3(area.x + area.width / 2, transform.position.y, area.y + area.height / 2), new Vector3(area.width, 0, area.height));
    }

    public void GenerateCoal()
    {
        foreach(Transform t in transform)
        {
            DestroyImmediate(t.gameObject);
        }

        List<Vector2> points = PoissonDiscSampling.GeneratePoints(discRadius, area.size);
        points.ForEach(p =>
        {
            p += area.position;
            Instantiate(coal, new Vector3(p.x, transform.position.y, p.y), Quaternion.Euler(-90, 0, 0), transform).transform.localScale = Vector3.one * Random.Range(15f, 50f);
        });
    }
}

[CustomEditor(typeof(CoalSpawner))]
public class CoalSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var spawner = (CoalSpawner)target;

        base.OnInspectorGUI();
        EditorGUI.BeginChangeCheck();

        spawner.discRadius = EditorGUILayout.FloatField("Disc Radius", spawner.discRadius);

        if (EditorGUI.EndChangeCheck())
        {
            spawner.GenerateCoal();
        }
    }
}
