using UnityEngine;
// ����� ������ �������̽�
public interface IUserData
{
    // �⺻������ ������ �ʱ�ȭ
    void SetDefaultData();
    // data load
    bool LoadData();
    // data save
    bool SaveData();
}
