using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Building))]
public class BuildingEditor : Editor
{
	public override void OnInspectorGUI()
	{
		Building building = (Building)target;

        if (Application.isPlaying)
        {
            building.roomsToSpawn = EditorGUILayout.IntField("Rooms To Spawn:", building.roomsToSpawn);

            if (GUILayout.Button("Spawn Rooms"))
            {
                building.SpawnRooms();
            }

            EditorGUILayout.LabelField("Rooms:", building.existingRooms.ToString());

            string state;
            if (building.spawned)
            {
                state = "Spawned!";
            }
            else
            {
                state = "Not spawned";
            }
            EditorGUILayout.LabelField("State:", state);
        }

        DrawDefaultInspector();
	}
}
