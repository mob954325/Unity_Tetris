using System;
using UnityEngine;

public interface IProduct
{
    public string ProductName { get; set; }

    /// <summary>
    /// 오브젝트가 비활성화 하기 전에 실행되는 델리게이트 (OnDisable)
    /// </summary>
    public Action BeforeDisable { get; set; }

    /// <summary>
    /// 프로덕트가 초기화할 내용
    /// </summary>
    public abstract void Initialize();
}