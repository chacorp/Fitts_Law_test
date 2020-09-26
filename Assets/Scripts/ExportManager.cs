using System.IO;
using UnityEngine;

public static class ExportManager
{
    static string reportdirectoryName = "Report";
    static string reportfileName = "FittsLaw_trial_result_4.csv";
    static string reportSeparator = ",";
    static string[] reportHeaders = new string[6]
    {
        "Width",
        "Distance",
        "MeanTCT",
        "success",
        "MousePosition",
        "TargetPosition"
    };
    static string timeStampHeader = "timeStamp";


#region Interactions
    public static void AppendToReport(string[] strings)
    {
        VerifyDirectory();
        VerifyFile();
        using (StreamWriter sw = File.AppendText(GetFilePath()))
        {
            string finalString = "";
            for (int i = 0; i < strings.Length; i++)
            {
                if (finalString != "")
                {
                    finalString += reportSeparator;
                }
                finalString += strings[i];
            }
            finalString += reportSeparator + GetTimeStamp();
            sw.WriteLine(finalString);
        }
    }

    public static void CreateReport()
    {
        VerifyDirectory();
        using (StreamWriter sw = File.CreateText(GetFilePath()))
        {
            string finalString = "";
            for (int i = 0; i < reportHeaders.Length; i++)
            {
                if (finalString != "")
                {
                    finalString += reportSeparator;
                }
                finalString += reportHeaders[i];
            }
            finalString += reportSeparator + timeStampHeader;
            sw.WriteLine(finalString);
        }
    }
#endregion


#region Operations
    static void VerifyDirectory()
    {
        string dir = GetDirPath();
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
    }

    static void VerifyFile()
    {
        string file = GetFilePath();
        if (!File.Exists(file))
        {
            CreateReport();
        }
    }
#endregion


#region Queires
    static string GetDirPath()
    {
        return Application.dataPath + "/" + reportdirectoryName;
    }

    static string GetFilePath()
    {
        return GetDirPath() + "/" + reportfileName;
    }

    static string GetTimeStamp()
    {
        return System.DateTime.UtcNow.ToString();
    }
#endregion
}
