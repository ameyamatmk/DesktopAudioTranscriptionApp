using System;
using System.Threading.Tasks;
using DesktopAudioTranscriptionApp.Exceptions.AudioCapture;
using DesktopAudioTranscriptionApp.Modules.AudioCapture;
using Xunit;

namespace DesktopAudioTranscriptionApp.Tests;

public class AudioCaptureServiceTests
{
    [Fact]
    public async Task StartCapture_ShouldRaiseDataAvailableEvent()
    {
        // Arrange
        var audioCaptureService = new NAudioCaptureService();
        // 最初のデバイスを選択
        var devices = audioCaptureService.GetAvailableDevices();
        audioCaptureService.SelectedDevice = devices[0];
        // データが利用可能になったときに発生するイベントを待機するためのTaskCompletionSourceを作成
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
        // 最初のデバイスを選択
        var devices = audioCaptureService.GetAvailableDevices();
        audioCaptureService.SelectedDevice = devices[0];
        // キャプチャ停止時に発生するイベントを待機するためのTaskCompletionSourceを作成
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

    [Fact]
    public void StartCapture_ShouldThrowException_WhenDeviceNotFound()
    {
        // Arrange
        var audioCaptureService = new NAudioCaptureService();
        audioCaptureService.SelectedDevice = null; // デバイスが選択されていない状態

        // Act
        var exception = Record.Exception(() => audioCaptureService.StartCapture());

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<AudioCaptureException>(exception);
    }
}
