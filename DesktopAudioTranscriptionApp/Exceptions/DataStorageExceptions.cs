using DesktopAudioTranscriptionApp.Exceptions.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopAudioTranscriptionApp.Exceptions.DataStorage;

/// <summary>
/// データストレージに関連する例外の基底クラス
/// </summary>
public class DataStorageException : AppException
{
    public DataStorageException(string message) : base(message) { }
    public DataStorageException(string message, Exception innerException) : base(message, innerException) { }
}

/// <summary>
/// ファイルアクセスエラー
/// </summary>
public class FileAccessException : DataStorageException
{
    public string FilePath { get; }

    public FileAccessException(string filePath)
        : base($"ファイル '{filePath}' へのアクセスが拒否されました。")
    {
        FilePath = filePath;
    }

    public FileAccessException(string filePath, Exception innerException)
        : base($"ファイル '{filePath}' へのアクセスが拒否されました。", innerException)
    {
        FilePath = filePath;
    }
}

/// <summary>
/// ディスク容量不足エラー
/// </summary>
public class DiskSpaceException : DataStorageException
{
    public string DrivePath { get; }

    public DiskSpaceException(string drivePath)
        : base($"ディスク '{drivePath}' の空き容量が不足しています。")
    {
        DrivePath = drivePath;
    }

    public DiskSpaceException(string drivePath, Exception innerException)
        : base($"ディスク '{drivePath}' の空き容量が不足しています。", innerException)
    {
        DrivePath = drivePath;
    }
}

/// <summary>
/// ファイルが見つからないエラー
/// </summary>
public class FileNotFoundException : DataStorageException
{
    public string FilePath { get; }

    public FileNotFoundException(string filePath)
        : base($"ファイル '{filePath}' が見つかりません。")
    {
        FilePath = filePath;
    }

    public FileNotFoundException(string filePath, Exception innerException)
        : base($"ファイル '{filePath}' が見つかりません。", innerException)
    {
        FilePath = filePath;
    }
}

/// <summary>
/// ファイルフォーマットエラー
/// </summary>
public class FileFormatException : DataStorageException
{
    public string FilePath { get; }

    public FileFormatException(string filePath)
        : base($"ファイル '{filePath}' のフォーマットが無効です。")
    {
        FilePath = filePath;
    }

    public FileFormatException(string filePath, Exception innerException)
        : base($"ファイル '{filePath}' のフォーマットが無効です。", innerException)
    {
        FilePath = filePath;
    }
}