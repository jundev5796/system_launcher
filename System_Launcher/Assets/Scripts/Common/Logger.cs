using System.Diagnostics;
using UnityEngine;

// Logging�� ���� utility class
// 1. �߰����� info ǥ�� (ex. timestamp)
// 2. ��ÿ� build ���� log ����
public static class Logger
{
    // �Ϲ� �α� �޽����� ���, DEV_VER ���Ǻ� �����Ͽ����� ����
    [Conditional("DEV_VER")]
    public static void Log(string msg)
    {
        UnityEngine.Debug.LogFormat("[{0}] {1}", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), msg);
    }

    // ��� �α� �޽����� ���. DEV_VER ���Ǻ� �����Ͽ����� ���� 
    [Conditional("DEV_VER")]
    public static void LogWarning(string msg)
    {
        UnityEngine.Debug.LogWarningFormat("[{0}] {1}", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), msg);
    }

    // ���� �α� �޽����� ���. �׻� ����
    public static void LogError(string msg)
    {
        UnityEngine.Debug.LogErrorFormat("[{0}] {1}", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), msg);
    }
}
