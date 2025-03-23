using DesktopAudioTranscriptionApp.Exceptions.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopAudioTranscriptionApp.Exceptions.Updates;

/// <summary>
/// アプリケーション更新に関連する例外の基底クラス
/// </summary>
public class UpdateException : AppException
{
    public UpdateException(string message) : base(message) { }
    public UpdateException(string message, Exception innerException) : base(message, innerException) { }
}

/// <summary>
/// 更新確認エラー
/// </summary>
public class UpdateCheckException : UpdateException
{
    public UpdateCheckException()
        : base("更新の確認に失敗しました。ネットワーク接続を確認してください。") { }

    public UpdateCheckException(Exception innerException)
        : base("更新の確認に失敗しました。ネットワーク接続を確認してください。", innerException) { }
}

/// <summary>
/// 更新ダウンロードエラー
/// </summary>
public class UpdateDownloadException : UpdateException
{
    public UpdateDownloadException()
        : base("更新のダウンロードに失敗しました。") { }

    public UpdateDownloadException(Exception innerException)
        : base("更新のダウンロードに失敗しました。", innerException) { }
}