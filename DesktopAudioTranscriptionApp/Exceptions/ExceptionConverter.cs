using System;
using NAudio.CoreAudioApi;
using DesktopAudioTranscriptionApp.Modules.AudioCapture;
using DesktopAudioTranscriptionApp.Exceptions.AudioCapture;
using DesktopAudioTranscriptionApp.Exceptions.DataStorage;
using DesktopAudioTranscriptionApp.Exceptions.SpeechRecognition;

namespace DesktopAudioTranscriptionApp.Exceptions.Utilities;

/// <summary>
/// 例外変換ユーティリティクラス
/// </summary>
public static class ExceptionConverter
{
    /// <summary>
    /// NAudio例外をドメイン固有例外に変換
    /// </summary>
    public static AudioCaptureException ConvertNAudioException(Exception ex, string deviceId = null)
    {
        // NAudio例外のタイプに基づいて変換
        if (ex is NAudio.MmException mmException)
        {
            switch (mmException.Result)
            {
                case NAudio.MmResult.BadDeviceId:
                    return new DeviceNotFoundException(deviceId ?? "unknown", ex);

                case NAudio.MmResult.AlreadyAllocated:
                    return new DeviceInUseException(ex);

                case NAudio.MmResult.NoDriver:
                    return new DeviceAccessDeniedException(ex);

                case NAudio.MmResult.WaveBadFormat:
                    return new UnsupportedAudioFormatException(null, ex);

                default:
                    return new AudioCaptureException($"音声キャプチャエラー: {mmException.Message}", ex);
            }
        }

        // その他の例外パターン
        if (ex.Message.Contains("access") || ex.Message.Contains("permission"))
        {
            return new DeviceAccessDeniedException(ex);
        }

        if (ex.Message.Contains("format") || ex.Message.Contains("unsupported"))
        {
            return new UnsupportedAudioFormatException(null, ex);
        }

        if (ex.Message.Contains("not found") || ex.Message.Contains("missing"))
        {
            return new DeviceNotFoundException(deviceId ?? "unknown", ex);
        }

        // デフォルト
        return new AudioCaptureException($"音声キャプチャエラー: {ex.Message}", ex);
    }

    /// <summary>
    /// Azure Speech Service例外をドメイン固有例外に変換
    /// </summary>
    public static SpeechRecognitionException ConvertAzureSpeechException(Exception ex)
    {
        // Azure認証エラー
        if (ex.Message.Contains("authentication"))
        {
            return new SpeechRecognitionAuthException(ex);
        }

        // HTTPリクエストエラー
        if (ex is System.Net.Http.HttpRequestException ||
            ex.Message.Contains("network") ||
            ex.Message.Contains("connect"))
        {
            return new SpeechRecognitionNetworkException(ex);
        }

        // メッセージに基づく分類
        string message = ex.Message.ToLowerInvariant();

        if (message.Contains("quota") || message.Contains("limit") || message.Contains("429"))
        {
            return new SpeechRecognitionQuotaException(ex);
        }

        if (message.Contains("unauthorized") || message.Contains("auth") ||
            message.Contains("key") || message.Contains("401"))
        {
            return new SpeechRecognitionAuthException(ex);
        }

        if (message.Contains("service") &&
            (message.Contains("unavailable") || message.Contains("503")))
        {
            return new SpeechRecognitionServiceUnavailableException(ex);
        }

        if (message.Contains("processing") || message.Contains("recognize"))
        {
            return new SpeechRecognitionProcessingException("音声認識の処理中にエラーが発生しました。", ex);
        }

        // デフォルト
        return new SpeechRecognitionException($"音声認識エラー: {ex.Message}", ex);
    }

    /// <summary>
    /// IOエラーをドメイン固有例外に変換
    /// </summary>
    public static DataStorageException ConvertIOException(Exception ex, string filePath)
    {
        if (ex is System.IO.IOException ioException)
        {
            // ディスク容量不足
            if (ioException.Message.Contains("disk") &&
                (ioException.Message.Contains("full") || ioException.Message.Contains("space")))
            {
                string drivePath = System.IO.Path.GetPathRoot(filePath);
                return new DiskSpaceException(drivePath, ex);
            }

            // ファイルアクセス拒否
            if (ioException.Message.Contains("access") ||
                ioException.Message.Contains("denied") ||
                ioException.Message.Contains("permission"))
            {
                return new FileAccessException(filePath, ex);
            }
        }

        // ファイルが見つからない
        if (ex is System.IO.FileNotFoundException)
        {
            return new FileNotFoundException(filePath, ex);
        }

        // フォーマットエラー（JSONや設定ファイルなど）
        if (ex is System.FormatException ||
            ex is Newtonsoft.Json.JsonException ||
            ex is System.Xml.XmlException)
        {
            return new FileFormatException(filePath, ex);
        }

        // デフォルト
        return new DataStorageException($"ファイル操作エラー: {ex.Message}", ex);
    }
}
