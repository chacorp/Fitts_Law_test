using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

public static class MyTools
{
    [MenuItem("My Tools/Add To Report %F1")]
    static void Dev_AppendToReport()
    {
        for (int i = 0; i < ExperimentManager.Instance.condition.Count; i++)
        {
            ExportManager.AppendToReport(
                new string[3]
                {
                ExperimentManager.Instance.condition[i].x.ToString(),
                ExperimentManager.Instance.condition[i].y.ToString(),
                ExperimentManager.Instance.completionTime[i].ToString()
                }
            );
        }
        EditorApplication.Beep();
    }

    [MenuItem("My Tools/Reset Report %F2")]
    static void Dev_ResetToReport()
    {
        ExportManager.CreateReport();
        EditorApplication.Beep();
    }
}
#else
#endif