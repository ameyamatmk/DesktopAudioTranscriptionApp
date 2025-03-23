using DesktopAudioTranscriptionApp.Exceptions.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopAudioTranscriptionApp.Exceptions.SpeechRecognition;

/// <summary>
/// 音声認識に関連する例外の基底クラス
/// </summary>
public class SpeechRecognitionException : AppException
{
    public SpeechRecognitionException(string message) : base(message) { }
    public SpeechRecognitionException(string message, Exception innerException) : base(message, innerException) { }
}

/// <summary>
/// 音声認識サービスの認証エラー
/// </summary>
public class SpeechRecognitionAuthException : SpeechRecognitionException
{
    public SpeechRecognitionAuthException()
        : base("音声認識サービスの認証に失敗しました。APIキーとリージョン設定を確認してください。") { }

    public SpeechRecognitionAuthException(Exception innerException)
        : base("音声認識サービスの認証に失敗しました。APIキーとリージョン設定を確認してください。", innerException) { }
}

/// <summary>
/// 音声認識サービスへの接続エラー
/// </summary>
public class SpeechRecognitionNetworkException : SpeechRecognitionException
{
    public SpeechRecognitionNetworkException()
        : base("音声認識サービスへの接続に失敗しました。ネットワーク接続を確認してください。") { }

    public SpeechRecognitionNetworkException(Exception innerException)
        : base("音声認識サービスへの接続に失敗しました。ネットワーク接続を確認してください。", innerException) { }
}

/// <summary>
/// 音声認識サービスの利用制限超過エラー
/// </summary>
public class SpeechRecognitionQuotaException : SpeechRecognitionException
{
    public SpeechRecognitionQuotaException()
        : base("音声認識サービスの利用制限を超過しました。しばらく待つか、Azure Portalで使用量を確認してください。") { }

    public SpeechRecognitionQuotaException(Exception innerException)
        : base("音声認識サービスの利用制限を超過しました。しばらく待つか、Azure Portalで使用量を確認してください。", innerException) { }
}

/// <summary>
/// 音声認識の処理中エラー
/// </summary>
public class SpeechRecognitionProcessingException : SpeechRecognitionException
{
    public SpeechRecognitionProcessingException(string message) : base(message) { }
    public SpeechRecognitionProcessingException(string message, Exception innerException) : base(message, innerException) { }
}

/// <summary>
/// 音声認識サービスが利用できないエラー
/// </summary>
public class SpeechRecognitionServiceUnavailableException : SpeechRecognitionException
{
    public SpeechRecognitionServiceUnavailableException()
        : base("音声認識サービスが一時的に利用できません。しばらく経ってから再試行してください。") { }

    public SpeechRecognitionServiceUnavailableException(Exception innerException)
        : base("音声認識サービスが一時的に利用できません。しばらく経ってから再試行してください。", innerException) { }
}