using DesktopAudioTranscriptionApp.Exceptions.Utilities;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.Collections.Generic;

namespace DesktopAudioTranscriptionApp.Modules.AudioCapture
{
    /// <summary>
    /// NAudio���g�p���������L���v�`���T�[�r�X�̎���
    /// </summary>
    public class NAudioCaptureService : IAudioCaptureService, IDisposable
    {
        private WasapiLoopbackCapture _captureInstance;
        private bool _disposed = false;

        /// <summary>
        /// �����f�[�^�����p�\�ɂȂ����Ƃ��ɔ�������C�x���g
        /// </summary>
        public event EventHandler<AudioDataAvailableEventArgs> DataAvailable;

        /// <summary>
        /// �����L���v�`������~�����Ƃ��ɔ�������C�x���g
        /// </summary>
        public event EventHandler<AudioCaptureStoppedEventArgs> CaptureStopped;

        /// <summary>
        /// ���݂̉����t�H�[�}�b�g���擾���܂�
        /// </summary>
        public WaveFormat CurrentFormat => _captureInstance?.WaveFormat;

        /// <summary>
        /// ���p�\�ȃI�[�f�B�I�f�o�C�X�̃��X�g���擾���܂�
        /// </summary>
        /// <returns>���p�\�ȃI�[�f�B�I�f�o�C�X�̃��X�g</returns>
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
        /// �I�����ꂽ�I�[�f�B�I�f�o�C�X
        /// </summary>
        public AudioDevice SelectedDevice { get; set; }

        /// <summary>
        /// �����L���v�`�����J�n���܂�
        /// </summary>
        /// <exception cref="InvalidOperationException">SelectedDevice���ݒ肳��Ă��Ȃ��ꍇ�ɃX���[����܂�</exception>
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
        /// �����L���v�`�����~���܂�
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
        /// ���\�[�X��������܂�
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// ���\�[�X��������܂�
        /// </summary>
        /// <param name="disposing">�}�l�[�W�h���\�[�X��������邩�ǂ���</param>
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
