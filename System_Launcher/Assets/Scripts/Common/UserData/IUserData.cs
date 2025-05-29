using UnityEngine;
// 사용자 데이터 인터페이스
public interface IUserData
{
    // 기본값으로 데이터 초기화
    void SetDefaultData();
    // data load
    bool LoadData();
    // data save
    bool SaveData();
}
