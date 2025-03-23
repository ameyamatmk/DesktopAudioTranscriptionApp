using System;
using NAudio.CoreAudioApi;
using DesktopAudioTranscriptionApp.Modules.AudioCapture;
using DesktopAudioTranscriptionApp.Exceptions.AudioCapture;
using DesktopAudioTranscriptionApp.Exceptions.DataStorage;
using DesktopAudioTranscriptionApp.Exceptions.SpeechRecognition;

namespace DesktopAudioTranscriptionApp.Exceptions.Utilities;

/// <summary>
/// ��O�ϊ����[�e�B���e�B�N���X
/// </summary>
public static class ExceptionConverter
{
    /// <summary>
    /// NAudio��O���h���C���ŗL��O�ɕϊ�
    /// </summary>
    public static AudioCaptureException ConvertNAudioException(Exception ex, string deviceId = null)
    {
        // NAudio��O�̃^�C�v�Ɋ�Â��ĕϊ�
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
                    return new AudioCaptureException($"�����L���v�`���G���[: {mmException.Message}", ex);
            }
        }

        // ���̑��̗�O�p�^�[��
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

        // �f�t�H���g
        return new AudioCaptureException($"�����L���v�`���G���[: {ex.Message}", ex);
    }

    /// <summary>
    /// Azure Speech Service��O���h���C���ŗL��O�ɕϊ�
    /// </summary>
    public static SpeechRecognitionException ConvertAzureSpeechException(Exception ex)
    {
        // Azure�F�؃G���[
        if (ex.Message.Contains("authentication"))
        {
            return new SpeechRecognitionAuthException(ex);
        }

        // HTTP���N�G�X�g�G���[
        if (ex is System.Net.Http.HttpRequestException ||
            ex.Message.Contains("network") ||
            ex.Message.Contains("connect"))
        {
            return new SpeechRecognitionNetworkException(ex);
        }

        // ���b�Z�[�W�Ɋ�Â�����
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
            return new SpeechRecognitionProcessingException("�����F���̏������ɃG���[���������܂����B", ex);
        }

        // �f�t�H���g
        return new SpeechRecognitionException($"�����F���G���[: {ex.Message}", ex);
    }

    /// <summary>
    /// IO�G���[���h���C���ŗL��O�ɕϊ�
    /// </summary>
    public static DataStorageException ConvertIOException(Exception ex, string filePath)
    {
        if (ex is System.IO.IOException ioException)
        {
            // �f�B�X�N�e�ʕs��
            if (ioException.Message.Contains("disk") &&
                (ioException.Message.Contains("full") || ioException.Message.Contains("space")))
            {
                string drivePath = System.IO.Path.GetPathRoot(filePath);
                return new DiskSpaceException(drivePath, ex);
            }

            // �t�@�C���A�N�Z�X����
            if (ioException.Message.Contains("access") ||
                ioException.Message.Contains("denied") ||
                ioException.Message.Contains("permission"))
            {
                return new FileAccessException(filePath, ex);
            }
        }

        // �t�@�C����������Ȃ�
        if (ex is System.IO.FileNotFoundException)
        {
            return new FileNotFoundException(filePath, ex);
        }

        // �t�H�[�}�b�g�G���[�iJSON��ݒ�t�@�C���Ȃǁj
        if (ex is System.FormatException ||
            ex is Newtonsoft.Json.JsonException ||
            ex is System.Xml.XmlException)
        {
            return new FileFormatException(filePath, ex);
        }

        // �f�t�H���g
        return new DataStorageException($"�t�@�C������G���[: {ex.Message}", ex);
    }
}
