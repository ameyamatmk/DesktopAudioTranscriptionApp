using DesktopAudioTranscriptionApp.Exceptions.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopAudioTranscriptionApp.Exceptions.Security;

/// <summary>
/// セキュリティに関連する例外の基底クラス
/// </summary>
public class SecurityException : AppException
{
    public SecurityException(string message) : base(message) { }
    public SecurityException(string message, Exception innerException) : base(message, innerException) { }
}

/// <summary>
/// 暗号化エラー
/// </summary>
public class EncryptionException : SecurityException
{
    public EncryptionException()
        : base("データの暗号化に失敗しました。") { }

    public EncryptionException(Exception innerException)
        : base("データの暗号化に失敗しました。", innerException) { }
}

/// <summary>
/// 復号化エラー
/// </summary>
public class DecryptionException : SecurityException
{
    public DecryptionException()
        : base("データの復号化に失敗しました。") { }

    public DecryptionException(Exception innerException)
        : base("データの復号化に失敗しました。", innerException) { }
}