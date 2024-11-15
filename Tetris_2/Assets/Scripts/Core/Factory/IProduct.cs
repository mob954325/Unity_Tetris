using System;
using UnityEngine;

public interface IProduct
{
    public string ProductName { get; set; }

    /// <summary>
    /// ������Ʈ�� ��Ȱ��ȭ �ϱ� ���� ����Ǵ� ��������Ʈ (OnDisable)
    /// </summary>
    public Action BeforeDisable { get; set; }

    /// <summary>
    /// ���δ�Ʈ�� �ʱ�ȭ�� ����
    /// </summary>
    public abstract void Initialize();
}