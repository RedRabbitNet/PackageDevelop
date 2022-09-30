using System.Collections.Generic;

/// <summary>
/// ここに実装する機能を宣言する
/// </summary>
public interface IRuntimeDataProvider
{
    
}
public interface IRuntimeDataProvider<DataType> : IRuntimeDataProvider
{
    List<DataType> DataList { get; set; }
}