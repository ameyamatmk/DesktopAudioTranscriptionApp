using DesktopAudioTranscriptionApp.Exceptions.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopAudioTranscriptionApp.Exceptions.Settings;

/// <summary>
/// 設定に関連する例外の基底クラス
/// </summary>
public class SettingsException : AppException
{
    public SettingsException(string message) : base(message) { }
    public SettingsException(string message, Exception innerException) : base(message, innerException) { }
}

/// <summary>
/// 設定ファイルの読み込みエラー
/// </summary>
public class SettingsLoadException : SettingsException
{
    public SettingsLoadException()
        : base("設定ファイルの読み込みに失敗しました。") { }

    public SettingsLoadException(Exception innerException)
        : base("設定ファイルの読み込みに失敗しました。", innerException) { }
}

/// <summary>
/// 設定ファイルの保存エラー
/// </summary>
public class SettingsSaveException : SettingsException
{
    public SettingsSaveException()
        : base("設定ファイルの保存に失敗しました。") { }

    public SettingsSaveException(Exception innerException)
        : base("設定ファイルの保存に失敗しました。", innerException) { }
}

/// <summary>
/// 無効な設定値のエラー
/// </summary>
public class InvalidSettingException : SettingsException
{
    public string SettingName { get; }
    public string SettingValue { get; }

    public InvalidSettingException(string settingName, string settingValue)
        : base($"設定 '{settingName}' の値 '{settingValue}' が無効です。")
    {
        SettingName = settingName;
        SettingValue = settingValue;
    }

    public InvalidSettingException(string settingName, string settingValue, Exception innerException)
        : base($"設定 '{settingName}' の値 '{settingValue}' が無効です。", innerException)
    {
        SettingName = settingName;
        SettingValue = settingValue;
    }
}