
using UnityEngine;
using UnityEditor;

namespace SchedulingUtilities
{
	[CanEditMultipleObjects]
    [CustomEditor(typeof(SchedulingUtilities.TimeOffRequestReport))]
    public class TimeOffRequestReportEditor : Editor
    {
	    private TimeOffRequestReport _report;
		private SerializedProperty _timeOffRequests;
		private SerializedProperty _etmReportCsvFilePath;

        private void OnEnable()
        {
	        _report = target as TimeOffRequestReport;
			_timeOffRequests = serializedObject.FindProperty("timeOffRequests");
			_etmReportCsvFilePath = serializedObject.FindProperty("etmReportCsvFilePath");
        }
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
			EditorGUILayout.PropertyField(_timeOffRequests);

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PropertyField(_etmReportCsvFilePath);
			
			if (GUILayout.Button("Select File", GUILayout.Width(80)))
			{
				string path = EditorUtility.OpenFilePanel("Select ETM Report", "", "csv");
				
				if (path.Length != 0)
				{
					string relativePath = path.StartsWith(Application.dataPath) ?
						"Assets" + path.Substring(Application.dataPath.Length)
						: path;
					
					_report.etmReportCsvFilePath = relativePath;
				}
			}
			EditorGUILayout.EndHorizontal();
			
			if (GUILayout.Button("Create Report"))
			{
				Undo.RecordObject(_report, "Create Time Off Request Report");
				_report.CreateReport();
				serializedObject.ApplyModifiedProperties();
				AssetDatabase.Refresh();
			}
			
            serializedObject.ApplyModifiedProperties();
        }
    }
}