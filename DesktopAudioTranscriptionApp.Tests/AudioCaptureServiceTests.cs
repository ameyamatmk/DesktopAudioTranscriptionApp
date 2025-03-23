using System;
using System.Threading.Tasks;
using DesktopAudioTranscriptionApp.Modules.AudioCapture;
using Xunit;

namespace DesktopAudioTranscriptionApp.Tests
{
    public class AudioCaptureServiceTests
    {
        [Fact]
        public async Task StartCapture_ShouldRaiseDataAvailableEvent()
        {
            // Arrange
            var audioCaptureService = new NAudioCaptureService();
            // �ŏ��̃f�o�C�X��I��
            var devices = audioCaptureService.GetAvailableDevices();
            audioCaptureService.SelectedDevice = devices[0];
            // �f�[�^�����p�\�ɂȂ����Ƃ��ɔ�������C�x���g��ҋ@���邽�߂�TaskCompletionSource���쐬
            var tcs = new TaskCompletionSource<bool>();
            audioCaptureService.DataAvailable += (sender, e) => tcs.SetResult(true);

            // Act
            audioCaptureService.StartCapture();

            // Assert
            var eventRaised = await tcs.Task;
            Assert.True(eventRaised);

            // Cleanup
            audioCaptureService.StopCapture();
        }

        [Fact]
        public async Task StopCapture_ShouldRaiseCaptureStoppedEvent()
        {
            // Arrange
            var audioCaptureService = new NAudioCaptureService();
            // �ŏ��̃f�o�C�X��I��
            var devices = audioCaptureService.GetAvailableDevices();
            audioCaptureService.SelectedDevice = devices[0];
            // �L���v�`����~���ɔ�������C�x���g��ҋ@���邽�߂�TaskCompletionSource���쐬
            var tcs = new TaskCompletionSource<bool>();
            audioCaptureService.CaptureStopped += (sender, e) => tcs.SetResult(true);

            // Act
            audioCaptureService.StartCapture();
            audioCaptureService.StopCapture();

            // Assert
            var eventRaised = await tcs.Task;
            Assert.True(eventRaised);
        }

        [Fact]
        public void GetAvailableDevices_ShouldReturnDeviceList()
        {
            // Arrange
            var audioCaptureService = new NAudioCaptureService();

            // Act
            var devices = audioCaptureService.GetAvailableDevices();

            // Assert
            Assert.NotNull(devices);
            Assert.NotEmpty(devices);
        }
    }
}
