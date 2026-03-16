using UnityEngine;
using System.Runtime.CompilerServices;

public static class NullChecker
{
    public static bool IsNULL<T>(T obj, [CallerFilePath] string filePath = "", 
    [CallerLineNumber] int lineNumber = 0)
    {
        if (obj == null)
        {
            string fileName = System.IO.Path.GetFileName(filePath);

            Debug.LogError($"<b>[NULL CHECK]</b>: Object of type <b>{typeof(T).Name}</b> is NULL.\n" +
                       $"Source: <color=yellow>{fileName}</color> (Line: {lineNumber})");
            return true;
        }

        return false;
    }
}
