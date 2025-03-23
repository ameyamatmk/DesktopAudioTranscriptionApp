using DesktopAudioTranscriptionApp.Exceptions.Utilities;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.Collections.Generic;

namespace DesktopAudioTranscriptionApp.Modules.AudioCapture
{
    /// <summary>
    /// NAudioを使用した音声キャプチャサービスの実装
    /// </summary>
    public class NAudioCaptureService : IAudioCaptureService, IDisposable
    {
        private WasapiLoopbackCapture _captureInstance;
        private bool _disposed = false;

        /// <summary>
        /// 音声データが利用可能になったときに発生するイベント
        /// </summary>
        public event EventHandler<AudioDataAvailableEventArgs> DataAvailable;

        /// <summary>
        /// 音声キャプチャが停止したときに発生するイベント
        /// </summary>
        public event EventHandler<AudioCaptureStoppedEventArgs> CaptureStopped;

        /// <summary>
        /// 現在の音声フォーマットを取得します
        /// </summary>
        public WaveFormat CurrentFormat => _captureInstance?.WaveFormat;

        /// <summary>
        /// 利用可能なオーディオデバイスのリストを取得します
        /// </summary>
        /// <returns>利用可能なオーディオデバイスのリスト</returns>
        public List<AudioDevice> GetAvailableDevices()
        {
            var devices = new List<AudioDevice>();
            var enumerator = new MMDeviceEnumerator();
            foreach (var device in enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active))
            {
                devices.Add(new AudioDevice
                {
                    Id = device.ID,
                    Name = device.FriendlyName,
                    IsDefault = device.ID == enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia).ID
                });
            }
            return devices;
        }

        /// <summary>
        /// 選択されたオーディオデバイス
        /// </summary>
        public AudioDevice SelectedDevice { get; set; }

        /// <summary>
        /// 音声キャプチャを開始します
        /// </summary>
        /// <exception cref="InvalidOperationException">SelectedDeviceが設定されていない場合にスローされます</exception>
        public void StartCapture()
        {
            if (SelectedDevice == null)
            {
                throw ExceptionConverter.ConvertNAudioException(new InvalidOperationException("SelectedDevice is not set."));
            }

            try
            {
                var enumerator = new MMDeviceEnumerator();
                var device = enumerator.GetDevice(SelectedDevice.Id);

                _captureInstance = new WasapiLoopbackCapture(device);
                _captureInstance.DataAvailable += OnDataAvailable;
                _captureInstance.RecordingStopped += OnRecordingStopped;
                _captureInstance.StartRecording();
            }
            catch (Exception ex)
            {
                throw ExceptionConverter.ConvertNAudioException(ex, SelectedDevice.Id);
            }
        }

        /// <summary>
        /// 音声キャプチャを停止します
        /// </summary>
        public void StopCapture()
        {
            try
            {
                _captureInstance?.StopRecording();
            }
            catch (Exception ex)
            {
                throw ExceptionConverter.ConvertNAudioException(ex, SelectedDevice.Id);
            }
        }

        private void OnDataAvailable(object sender, WaveInEventArgs e)
        {
            DataAvailable?.Invoke(this, new AudioDataAvailableEventArgs
            {
                Buffer = e.Buffer,
                BytesRecorded = e.BytesRecorded
            });
        }

        private void OnRecordingStopped(object sender, StoppedEventArgs e)
        {
            CaptureStopped?.Invoke(this, new AudioCaptureStoppedEventArgs
            {
                Exception = e.Exception
            });
        }

        /// <summary>
        /// リソースを解放します
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// リソースを解放します
        /// </summary>
        /// <param name="disposing">マネージドリソースを解放するかどうか</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    try
                    {
                        _captureInstance?.Dispose();
                    }
                    catch (Exception ex)
                    {
                        throw ExceptionConverter.ConvertNAudioException(ex, SelectedDevice?.Id);
                    }
                }
                _disposed = true;
            }
        }
    }
}
