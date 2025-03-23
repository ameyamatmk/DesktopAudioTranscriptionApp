using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace DesktopAudioTranscriptionApp.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _isRecording;
    [ObservableProperty]
    private string _statusMessage = "準備完了";
    [ObservableProperty]
    private string _startButtonText = "録音開始";
    [RelayCommand]
    private void StartStopRecording()
    {
        IsRecording = true;
        StatusMessage = "録音中...";
        StartButtonText = "録音停止";
    }
}
