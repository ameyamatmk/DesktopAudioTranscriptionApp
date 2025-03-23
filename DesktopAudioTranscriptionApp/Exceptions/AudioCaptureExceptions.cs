using DesktopAudioTranscriptionApp.Exceptions.Core;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopAudioTranscriptionApp.Exceptions.AudioCapture;

/// <summary>
/// 音声キャプチャに関連する例外の基底クラス
/// </summary>
public class AudioCaptureException : AppException
{
    public AudioCaptureException(string message) : base(message) { }
    public AudioCaptureException(string message, Exception innerException) : base(message, innerException) { }
}

/// <summary>
/// 音声デバイスが見つからない場合の例外
/// </summary>
public class DeviceNotFoundException : AudioCaptureException
{
    public string DeviceId { get; }

    public DeviceNotFoundException(string deviceId)
        : base($"音声デバイス '{deviceId}' が見つかりません。")
    {
        DeviceId = deviceId;
    }

    public DeviceNotFoundException(string deviceId, Exception innerException)
        : base($"音声デバイス '{deviceId}' が見つかりません。", innerException)
    {
        DeviceId = deviceId;
    }
}

/// <summary>
/// 音声デバイスへのアクセス権限がない場合の例外
/// </summary>
public class DeviceAccessDeniedException : AudioCaptureException
{
    public DeviceAccessDeniedException()
        : base("音声デバイスへのアクセス権限がありません。管理者権限で実行してください。") { }

    public DeviceAccessDeniedException(Exception innerException)
        : base("音声デバイスへのアクセス権限がありません。管理者権限で実行してください。", innerException) { }
}

/// <summary>
/// 音声デバイスが他のアプリケーションで使用中の例外
/// </summary>
public class DeviceInUseException : AudioCaptureException
{
    public DeviceInUseException()
        : base("音声デバイスが他のアプリケーションで使用中です。") { }

    public DeviceInUseException(Exception innerException)
        : base("音声デバイスが他のアプリケーションで使用中です。", innerException) { }
}

/// <summary>
/// 音声フォーマットが非対応の場合の例外
/// </summary>
public class UnsupportedAudioFormatException : AudioCaptureException
{
    public WaveFormat Format { get; }

    public UnsupportedAudioFormatException(WaveFormat format)
        : base($"非対応の音声フォーマットです: {format}")
    {
        Format = format;
    }

    public UnsupportedAudioFormatException(WaveFormat format, Exception innerException)
        : base($"非対応の音声フォーマットです: {format}", innerException)
    {
        Format = format;
    }
}