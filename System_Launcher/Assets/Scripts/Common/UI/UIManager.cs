using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingletonBehaviour<UIManager> // �̱��� ������ ��ӹ޴� UI ������ Ŭ����
{
    public Transform UICanvasTrs; // UI ĵ���� Transform
    public Transform ClosedUITrs; // ���� UI�� ������ Transform



    public Image m_Fade;



    private BaseUI m_FrontUI; // ���� ���� �տ� �ִ� UI
    private Dictionary<System.Type, GameObject> m_OpenUIPool = new Dictionary<System.Type, GameObject>(); // ���� UI���� �����ϴ� ��ųʸ�
    private Dictionary<System.Type, GameObject> m_ClosedUIPool = new Dictionary<System.Type, GameObject>(); // ���� UI���� �����ϴ� ��ųʸ�


    // ��ȭ UI ������Ʈ
    private GoodsUI m_GoodsUI;

    // �ʱ�ȭ �޼���
    protected override void Init()
    {
        // �θ� Ŭ������ Init ȣ��
        base.Init();

        m_Fade.transform.localScale = Vector3.zero;

        // ������ GoodsUI ������Ʈ ã��
        m_GoodsUI = FindObjectOfType<GoodsUI>();
        // GoodsUI�� ã�� ������ ��
        if (!m_GoodsUI)
        {
            // �α� ���
            Logger.Log("No stats ui component found.");
        }
    }


    private BaseUI GetUI<T>(out bool isAlreadyOpen) // ���׸� Ÿ������ UI�� �������� �޼���
    {
        System.Type uiType = typeof(T); // ���׸� Ÿ���� System.Type���� ��ȯ

        BaseUI ui = null; // UI ������Ʈ ���� �ʱ�ȭ
        isAlreadyOpen = false; // �̹� �����ִ��� ���� �ʱ�ȭ

        if (m_OpenUIPool.ContainsKey(uiType)) // ���� UI Ǯ�� �ش� Ÿ���� �ִ��� Ȯ��
        {
            ui = m_OpenUIPool[uiType].GetComponent<BaseUI>(); // ���� UI Ǯ���� UI ������Ʈ ��������
            isAlreadyOpen = true; // �̹� ������������ ����
        }
        else if (m_ClosedUIPool.ContainsKey(uiType)) // ���� UI Ǯ�� �ش� Ÿ���� �ִ��� Ȯ��
        {
            ui = m_ClosedUIPool[uiType].GetComponent<BaseUI>(); // ���� UI Ǯ���� UI ������Ʈ ��������
            m_ClosedUIPool.Remove(uiType); // ���� UI Ǯ���� �ش� Ÿ�� ����
        }
        else // ��� Ǯ���� ���� ���
        {
            var uiObj = Instantiate(Resources.Load($"UI/{uiType}", typeof(GameObject))) as GameObject; // ���ҽ����� UI ������ �ε� �� �ν��Ͻ�ȭ
            ui = uiObj.GetComponent<BaseUI>(); // ������ ������Ʈ���� BaseUI ������Ʈ ��������
        }

        return ui; // UI ������Ʈ ��ȯ
    }


    public void OpenUI<T>(BaseUIData uiData) // ���׸� Ÿ������ UI�� ���� �޼���
    {
        System.Type uiType = typeof(T); // ���׸� Ÿ���� System.Type���� ��ȯ

        Logger.Log($"{GetType()}::OpenUI({uiType})"); // UI ���� �α� ���

        bool isAlreadyOpen = false; // �̹� �����ִ��� ���� ����
        var ui = GetUI<T>(out isAlreadyOpen); // UI ��������

        if (!ui) // UI�� ���� ���
        {
            Logger.LogError($"{uiType} does not exist."); // ���� �α� ���
            return; // �޼��� ����
        }

        if (isAlreadyOpen) // �̹� �����ִ� ���
        {
            Logger.LogError($"{uiType} is already open."); // ���� �α� ���
            return; // �޼��� ����
        }

        // ĵ������ �ڽ� �������� 1�� �� �ε��� ���
        var siblingIdx = UICanvasTrs.childCount - 1;
        ui.Init(UICanvasTrs); // UI �ʱ�ȭ
        ui.transform.SetSiblingIndex(siblingIdx); // UI�� ���� �ε��� ����
        ui.gameObject.SetActive(true); // UI ���ӿ�����Ʈ Ȱ��ȭ
        ui.SetInfo(uiData); // UI ���� ����
        ui.ShowUI(); // UI ǥ��

        m_FrontUI = ui; // ���� UI�� ���� ���� UI�� ����
        m_OpenUIPool[uiType] = ui.gameObject; // ���� UI Ǯ�� UI �߰�
    }

    public void CloseUI(BaseUI ui) // UI�� �ݴ� �޼���
    {
        System.Type uiType = ui.GetType(); // UI�� Ÿ�� ��������

        Logger.Log($"CloseUI UI:{uiType}"); // UI �ݱ� �α� ���

        ui.gameObject.SetActive(false); // UI ���ӿ�����Ʈ ��Ȱ��ȭ
        m_OpenUIPool.Remove(uiType); // ���� UI Ǯ���� �ش� Ÿ�� ����
        m_ClosedUIPool[uiType] = ui.gameObject; // ���� UI Ǯ�� UI �߰�
        ui.transform.SetParent(ClosedUITrs); // UI�� �θ� ���� UI Transform���� ����

        m_FrontUI = null; // ���� ���� UI �ʱ�ȭ
        var lastChild = UICanvasTrs.GetChild(UICanvasTrs.childCount - 1); // UI ĵ������ ������ �ڽ� ��������
        if (lastChild) // ������ �ڽ��� �ִ� ���
        {
            m_FrontUI = lastChild.gameObject.GetComponent<BaseUI>(); // ������ �ڽ��� ���� ���� UI�� ����
        }
    }

    public BaseUI GetActiveUI<T>() // ���׸� Ÿ������ Ȱ�� UI�� �������� �޼���
    {
        var uiType = typeof(T); // ���׸� Ÿ���� System.Type���� ��ȯ
        return m_OpenUIPool.ContainsKey(uiType) ? m_OpenUIPool[uiType].GetComponent<BaseUI>() : null; 
        // ���� UI Ǯ�� ������ UI ������Ʈ ��ȯ, ������ null ��ȯ
    }

    public bool ExistsOpenUI() // ���� UI�� �����ϴ��� Ȯ���ϴ� �޼���
    {
        return m_FrontUI != null; // ���� ���� UI�� null�� �ƴϸ� true ��ȯ
    }

    public BaseUI GetCurrentFrontUI() // ���� ���� ���� UI�� �������� �޼���
    {
        return m_FrontUI; // ���� ���� UI ��ȯ
    }

    public void CloseCurrFrontUI() // ���� ���� ���� UI�� �ݴ� �޼���
    {
        m_FrontUI.CloseUI(); // ���� ���� UI �ݱ�
    }

    public void CloseAllOpenUI() // ��� ���� UI�� �ݴ� �޼���
    {
        while (m_FrontUI) // ���� ���� UI�� �ִ� ���� �ݺ�
        {
            m_FrontUI.CloseUI(true); // ���� ���� UI �ݱ� (��ü �ݱ� ���)
        }
    }

    // ��ȭ UI Ȱ��ȭ/��Ȱ��ȭ
    public void EnableGoodsUI(bool value)
    {
        // ��ȭ UI ���ӿ�����Ʈ Ȱ��ȭ ���� ����
        m_GoodsUI.gameObject.SetActive(value);

        // Ȱ��ȭ�ϴ� ���
        if (value)
        {
            // ��ȭ ���� ����
            m_GoodsUI.SetValues();
        }
    }

    public void Fade(Color color, float startAlpha, float endAlpha, float duration, float startDelay, bool deactiveOnFinish, Action onFinish = null)
    {
        StartCoroutine(FadeCo(color, startAlpha, endAlpha, duration, startDelay, deactiveOnFinish, onFinish));
    }

    private IEnumerator FadeCo(Color color, float startAlpha, float endAlpha, float duration, float startDelay, bool deactiveOnFinish, Action onFinish)
    {
        yield return new WaitForSeconds(startDelay);

        m_Fade.transform.localScale = Vector3.one;
        m_Fade.color = new Color(color.r, color.g, color.b, startAlpha);

        var startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup - startTime < duration)
        {
            m_Fade.color = new Color(color.r, color.g, color.b, Mathf.Lerp(startAlpha, endAlpha, (Time.realtimeSinceStartup - startTime) / duration));
            yield return null;
        }

        m_Fade.color = new Color(color.r, color.g, color.b, endAlpha);

        if (deactiveOnFinish)
        {
            m_Fade.transform.localScale = Vector3.zero;
        }

        onFinish?.Invoke();
    }

    public void CancelFade()
    {
        m_Fade.transform.localScale = Vector3.zero;
    }
}
