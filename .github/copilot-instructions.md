# ���������N�����c�[�� - GitHub Copilot �J�X�^���C���X�g���N�V����

���̃h�L�������g�́AWindows�������������N�����c�[���̊J���ɂ�����R�[�f�B���O���[���Ɛ݌v�K�C�h���C�����`���Ă��܂��BGitHub Copilot�͈ȉ��̃��[���ɏ]���ăR�[�h�����ƃR�[�h�x�����s���Ă��������B

## �v���W�F�N�g�T�v

- **�v���W�F�N�g��**: �A�v���P�[�V�������������N�����c�[��
- **�ړI**: Windows��ł̃A�v���P�[�V�����o�͉��������A���^�C���ŕ����N����
- **�v���b�g�t�H�[��**: Windows
- **�J������**: C#
- **��v�Z�p**: 
  - WPF (Windows Presentation Foundation)
  - NAudio (�I�[�f�B�I�L���v�`��)
  - Azure Speech Service (�����F��API)
  - MVVM (CommunityToolkit.Mvvm)

## ��{�A�[�L�e�N�`��

```
DesktopAudioTranscriptionApp
������ Modules
��   ������ AudioCapture     # �����L���v�`���֘A
��   ������ SpeechRecognition # �����F���֘A
��   ������ Data             # �f�[�^�Ǘ��֘A
��   ������ UI               # UI�֘A
������ Models               # �r�W�l�X���W�b�N�ƃf�[�^���f��
������ ViewModels           # MVVM�p��ViewModel�N���X
������ Views                # UI�r���[
������ Services             # ���ʃT�[�r�X
```

GitHub Copilot �J�X�^���C���X�g���N�V�����ɃR�~�b�g���b�Z�[�W�̋K����ǉ����܂��B�R�~�b�g���b�Z�[�W�̖`���ɐړ����i�v���t�B�b�N�X�j��t����̂́A�ύX�̎�ނ��Ȍ��Ɏ������߂̈�ʓI�Ȋ��s�ł��B�ȉ��ɂ��̕�����ǉ����܂����F

## �N���X�݌v

### 1. �A�v���P�[�V�����S�̍\��

```
DesktopAudioTranscriptionApp
������ Modules
��   ������ AudioCapture     # �����L���v�`���֘A
��   ������ SpeechRecognition # �����F���֘A
��   ������ Data             # �f�[�^�Ǘ��֘A
��   ������ UI               # UI�֘A
������ Models               # �r�W�l�X���W�b�N�ƃf�[�^���f��
������ ViewModels           # MVVM�p��ViewModel�N���X
������ Views                # UI�r���[
������ Services             # ���ʃT�[�r�X
```

### 2. AudioCapture ���W���[��

```csharp
namespace DesktopAudioTranscriptionApp.Modules.AudioCapture
{
    // �����L���v�`���̂��߂̒��ۃC���^�[�t�F�[�X
    public interface IAudioCaptureService
    {
        event EventHandler<AudioDataAvailableEventArgs> DataAvailable;
        event EventHandler<AudioCaptureStoppedEventArgs> CaptureStopped;
        
        void StartCapture();
        void StopCapture();
        WaveFormat CurrentFormat { get; }
        List<AudioDevice> GetAvailableDevices();
        AudioDevice SelectedDevice { get; set; }
    }

    // �����N���X - NAudio�𗘗p
    public class NAudioCaptureService : IAudioCaptureService, IDisposable
    {
        private WasapiLoopbackCapture _captureInstance;
        // �������\�b�h
    }

    // �C�x���g�����N���X
    public class AudioDataAvailableEventArgs : EventArgs
    {
        public byte[] Buffer { get; set; }
        public int BytesRecorded { get; set; }
    }

    public class AudioCaptureStoppedEventArgs : EventArgs
    {
        public Exception Exception { get; set; }
    }

    public class AudioDevice
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsDefault { get; set; }
    }
}
```

### 3. SpeechRecognition ���W���[��

```csharp
namespace DesktopAudioTranscriptionApp.Modules.SpeechRecognition
{
    // �����F���̂��߂̒��ۃC���^�[�t�F�[�X
    public interface ISpeechRecognitionService
    {
        event EventHandler<IntermediateResultEventArgs> IntermediateResultReceived;
        event EventHandler<FinalResultEventArgs> FinalResultReceived;
        event EventHandler<RecognitionErrorEventArgs> ErrorOccurred;
        
        Task InitializeAsync();
        Task StartRecognitionAsync();
        Task StopRecognitionAsync();
        Task ProcessAudioDataAsync(byte[] buffer, int bytesRecorded);
    }

    // Azure Speech Service����
    public class AzureSpeechRecognitionService : ISpeechRecognitionService, IDisposable
    {
        private SpeechRecognizer _speechRecognizer;
        private PushAudioInputStream _audioInputStream;
        private SpeechConfig _speechConfig;
        private AudioConfig _audioConfig;
        private readonly AzureSpeechSettings _settings;
        
        public AzureSpeechRecognitionService(AzureSpeechSettings settings)
        {
            _settings = settings;
        }
        
        // �������\�b�h
    }

    // �C�x���g����
    public class IntermediateResultEventArgs : EventArgs
    {
        public string Text { get; set; }
    }

    public class FinalResultEventArgs : EventArgs
    {
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }
        public double Confidence { get; set; }
    }

    public class RecognitionErrorEventArgs : EventArgs
    {
        public string ErrorCode { get; set; }
        public string ErrorDetails { get; set; }
    }
}
```

### 4. Data ���W���[��

```csharp
namespace DesktopAudioTranscriptionApp.Modules.Data
{
    // �f�[�^�ۑ��̂��߂̃C���^�[�t�F�[�X
    public interface IDataStorageService
    {
        Task SaveTranscriptionAsync(TranscriptionSession session);
        Task SaveAudioAsync(byte[] audioData, TranscriptionSession session);
        Task<List<TranscriptionSession>> GetSessionsAsync();
        Task<TranscriptionSession> GetSessionAsync(string sessionId);
        Task DeleteSessionAsync(string sessionId);
    }

    // �t�@�C���x�[�X�̎���
    public class FileStorageService : IDataStorageService
    {
        private readonly AppSettings _settings;
        
        public FileStorageService(ISettingsService settingsService)
        {
            _settings = settingsService.LoadSettings();
        }
        
        // �������\�b�h
    }

    // �g�����X�N���v�V�����@�\�̂��߂̃}�l�[�W���[
    public class TranscriptionManager
    {
        private readonly IDataStorageService _storageService;
        private TranscriptionSession _currentSession;
        
        public event EventHandler<TranscriptionItemAddedEventArgs> ItemAdded;
        
        // ���\�b�h
        public void StartNewSession();
        public void EndCurrentSession();
        public void AddTranscriptionItem(string text);
        public Task ExportSessionTextAsync(string sessionId, string filePath, ExportOptions options);
    }

    public class ExportOptions
    {
        public bool IncludeTimestamp { get; set; } = true;
        public TimestampFormat TimestampFormat { get; set; } = TimestampFormat.ActualTime;
        public bool IncludeSessionInfo { get; set; } = true;
    }

    // �C�x���g����
    public class TranscriptionItemAddedEventArgs : EventArgs
    {
        public TranscriptionItem Item { get; set; }
    }
}
```

### 5. Models

```csharp
namespace DesktopAudioTranscriptionApp.Models
{
    // �g�����X�N���v�V�����Z�b�V����
    public class TranscriptionSession
    {
        public string Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public List<TranscriptionItem> Items { get; set; } = new List<TranscriptionItem>();
        public string AudioFilePath { get; set; }
        public string TextFilePath { get; set; }
    }

    // �X�̃g�����X�N���v�V�����A�C�e��
    public class TranscriptionItem
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }
        public TimeSpan RelativeTime { get; set; } // �Z�b�V�����J�n����̎���
    }

    // �����F���T�[�r�X�ݒ�̊��N���X
    public abstract class RecognitionServiceSettings
    {
        public string ServiceType { get; }

        protected RecognitionServiceSettings(string serviceType)
        {
            ServiceType = serviceType;
        }
    }

    // Azure Speech Service�ݒ�
    public class AzureSpeechSettings : RecognitionServiceSettings
    {
        public string Key { get; set; }
        public string Region { get; set; }
        public string Language { get; set; } = "ja-JP";
        
        public AzureSpeechSettings() : base("Azure") { }
    }

    // �A�v���P�[�V�����ݒ�
    public class AppSettings
    {
        // ��{�ݒ�
        public RecognitionServiceSettings RecognitionSettings { get; set; }
        public bool AlwaysOnTop { get; set; }
        public int FontSize { get; set; }
        public TimestampFormat TimestampFormat { get; set; }
        
        // �����ݒ�
        public string AudioDeviceId { get; set; }
        public bool AutoDetectDevices { get; set; }
        public int SampleRate { get; set; }
        public int Channels { get; set; }
        public int BufferSize { get; set; }
        
        // �ۑ��ݒ�
        public bool AutoSaveTranscription { get; set; }
        public string TranscriptionSavePath { get; set; }
        public string TranscriptionFileNameFormat { get; set; }
        public bool SaveAudio { get; set; }
        public AudioFileFormat AudioFileFormat { get; set; }
    }

    // �񋓌^
    public enum TimestampFormat
    {
        ActualTime,
        RelativeTime,
        Both
    }

    public enum AudioFileFormat
    {
        WAV,
        MP3
    }
}
```

### 6. ViewModels

```csharp
namespace DesktopAudioTranscriptionApp.ViewModels
{
    // ���C����ʂ�ViewModel
    public partial class MainViewModel : ObservableObject
    {
        private readonly IAudioCaptureService _audioCaptureService;
        private readonly ISpeechRecognitionService _speechRecognitionService;
        private readonly TranscriptionManager _transcriptionManager;
        private readonly ISettingsService _settingsService;
        
        // �v���p�e�B
        [ObservableProperty]
        private ObservableCollection<TranscriptionItemViewModel> _transcriptionItems = new();
        
        [ObservableProperty]
        private string _intermediateResult;
        
        [ObservableProperty]
        private bool _isRecording;
        
        [ObservableProperty]
        private bool _alwaysOnTop;
        
        [ObservableProperty]
        private bool _showDiagnostics;
        
        [ObservableProperty]
        private string _statusMessage;
        
        [ObservableProperty]
        private string _diagnosticsInfo;
        
        // �R�}���h
        [RelayCommand]
        private async Task StartStopRecording()
        {
            if (IsRecording)
            {
                await StopRecordingAsync();
            }
            else
            {
                await StartRecordingAsync();
            }
        }
        
        [RelayCommand]
        private void CopyToClipboard()
        {
            // ����
        }
        
        [RelayCommand]
        private void Clear()
        {
            // ����
        }
        
        [RelayCommand]
        private void OpenSettings()
        {
            // ����
        }
        
        // ���\�b�h
        private async Task StartRecordingAsync()
        {
            // ����
        }
        
        private async Task StopRecordingAsync()
        {
            // ����
        }
    }

    // �ݒ��ʂ�ViewModel
    public partial class SettingsViewModel : ObservableObject
    {
        private readonly ISettingsService _settingsService;
        private AppSettings _settings;
        
        // �v���p�e�B (��������)
        [ObservableProperty]
        private string _azureSpeechKey;
        
        [ObservableProperty]
        private string _azureSpeechRegion;
        
        [ObservableProperty]
        private bool _alwaysOnTop;
        
        [ObservableProperty]
        private string _selectedFontSize;
        
        // ���̐ݒ�v���p�e�B...
        
        // �R�}���h
        [RelayCommand]
        private async Task Save()
        {
            // �ݒ��ۑ�
        }
        
        [RelayCommand]
        private void Cancel()
        {
            // �L�����Z������
        }
        
        [RelayCommand]
        private void BrowseFolder()
        {
            // �t�H���_�I���_�C�A���O��\��
        }
    }

    // �g�����X�N���v�V�����A�C�e����ViewModel
    public class TranscriptionItemViewModel : ObservableObject
    {
        private readonly TranscriptionItem _model;
        private readonly TimestampFormat _format;
        
        // �v���p�e�B
        public string Id => _model.Id;
        public string Text => _model.Text;
        
        public string FormattedTimestamp
        {
            get
            {
                return _format switch
                {
                    TimestampFormat.ActualTime => $"[{_model.Timestamp:HH:mm:ss}]",
                    TimestampFormat.RelativeTime => $"[{_model.RelativeTime:hh\\:mm\\:ss}]",
                    TimestampFormat.Both => $"[{_model.Timestamp:HH:mm:ss}/{_model.RelativeTime:hh\\:mm\\:ss}]",
                    _ => $"[{_model.Timestamp:HH:mm:ss}]"
                };
            }
        }
    }
}
```

### 7. Services

```csharp
namespace DesktopAudioTranscriptionApp.Services
{
    // �ݒ�Ǘ��T�[�r�X
    public interface ISettingsService
    {
        AppSettings LoadSettings();
        Task SaveSettingsAsync(AppSettings settings);
    }

    // JSON�ݒ�t�@�C������
    public class JsonSettingsService : ISettingsService
    {
        private readonly string _settingsFilePath;
        private readonly SecureStorageService _secureStorage;
        
        public JsonSettingsService(SecureStorageService secureStorage)
        {
            _secureStorage = secureStorage;
            _settingsFilePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "DesktopAudioTranscriptionApp",
                "settings.json");
        }
        
        // �������\�b�h
    }

    // API�L�[���S�Ǘ��T�[�r�X
    public class SecureStorageService
    {
        public void StoreSecureData(string key, string value)
        {
            // Windows DPAPI���g�p���Ĉ��S�ɕۑ�
        }
        
        public string RetrieveSecureData(string key)
        {
            // Windows DPAPI���g�p���Ď擾
        }
    }

    // ���M���O�T�[�r�X
    public interface ILogService
    {
        void LogInfo(string message);
        void LogWarning(string message);
        void LogError(string message, Exception ex = null);
    }

    // �t�@�C���x�[�X�̃��O����
    public class FileLogService : ILogService
    {
        private readonly string _logFilePath;
        
        public FileLogService()
        {
            string logDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "DesktopAudioTranscriptionApp",
                "Logs");
                
            Directory.CreateDirectory(logDirectory);
            _logFilePath = Path.Combine(logDirectory, $"log_{DateTime.Now:yyyyMMdd}.txt");
        }
        
        // �������\�b�h
    }

    // �A�v���P�[�V�����X�V�m�F�T�[�r�X
    public class UpdateCheckService
    {
        public async Task<UpdateInfo> CheckForUpdatesAsync()
        {
            // ����
        }
    }

    public class UpdateInfo
    {
        public bool UpdateAvailable { get; set; }
        public string CurrentVersion { get; set; }
        public string NewVersion { get; set; }
        public string ReleaseNotes { get; set; }
        public string DownloadUrl { get; set; }
    }
}
```

## �f�[�^�݌v

### 1. �ݒ�t�@�C���iJSON�`���j

```json
{
  "RecognitionSettings": {
    "$type": "DesktopAudioTranscriptionApp.Models.AzureSpeechSettings, DesktopAudioTranscriptionApp",
    "Key": "encrypted_key_placeholder",
    "Region": "japaneast",
    "Language": "ja-JP"
  },
  "UI": {
    "AlwaysOnTop": false,
    "FontSize": 14,
    "TimestampFormat": "ActualTime"
  },
  "AudioCapture": {
    "DeviceId": "default",
    "AutoDetectDevices": true,
    "SampleRate": 16000,
    "Channels": 1,
    "BufferSize": 2048
  },
  "Storage": {
    "AutoSaveTranscription": true,
    "TranscriptionSavePath": "C:\\Users\\Username\\Documents\\Transcriptions",
    "TranscriptionFileNameFormat": "transcript_{yyyy-MM-dd_HH-mm-ss}",
    "SaveAudio": false,
    "AudioFileFormat": "WAV"
  },
  "Updates": {
    "AutoCheck": true,
    "LastCheckDate": "2025-03-23T00:00:00Z"
  }
}
```

### 2. �g�����X�N���v�V�����Z�b�V�����f�[�^�iJSON�j

```json
{
  "SessionId": "20250323_145502",
  "StartTime": "2025-03-23T14:55:02.123Z",
  "EndTime": "2025-03-23T15:10:45.678Z",
  "Items": [
    {
      "Id": "item1",
      "Text": "����͉����F���̃e�X�g�ł��B",
      "Timestamp": "2025-03-23T14:55:20.456Z",
      "RelativeTime": "00:00:18.333"
    },
    {
      "Id": "item2",
      "Text": "Windows�A�v���P�[�V�����̉��������A���^�C���ŕ����ɋN�����܂��B",
      "Timestamp": "2025-03-23T14:55:32.789Z",
      "RelativeTime": "00:00:30.666"
    }
  ],
  "AudioFilePath": "audio_20250323_145502.wav",
  "TextFilePath": "transcript_20250323_145502.txt"
}
```

### 3. �e�L�X�g�G�N�X�|�[�g�`��

#### ��{�e�L�X�g�`���i�^�C���X�^���v����j

```
# ���������N���� - 2025/03/23 14:55:02

[14:55:20] ����͉����F���̃e�X�g�ł��B
[14:55:32] Windows�A�v���P�[�V�����̉��������A���^�C���ŕ����ɋN�����܂��B
[14:56:05] NAudio���g�p���ăf�X�N�g�b�v�������L���v�`�����Ă��܂��B
[14:56:40] Azure Speech Service�ɂ�鉹���F���̐��x�͔��ɍ����ł��B
```

#### ��{�e�L�X�g�`���i�^�C���X�^���v�Ȃ��j

```
����͉����F���̃e�X�g�ł��B
Windows�A�v���P�[�V�����̉��������A���^�C���ŕ����ɋN�����܂��B
NAudio���g�p���ăf�X�N�g�b�v�������L���v�`�����Ă��܂��B
Azure Speech Service�ɂ�鉹���F���̐��x�͔��ɍ����ł��B
```

#### �}�[�N�_�E���`��

```markdown
# ���������N��������

## �Z�b�V�������
- �J�n����: 2025/03/23 14:55:02
- �I������: 2025/03/23 15:10:45
- �p������: 15��43�b

## �F���e�L�X�g

### [14:55:20]
����͉����F���̃e�X�g�ł��B

### [14:55:32]
Windows�A�v���P�[�V�����̉��������A���^�C���ŕ����ɋN�����܂��B

### [14:56:05]
NAudio���g�p���ăf�X�N�g�b�v�������L���v�`�����Ă��܂��B

### [14:56:40]
Azure Speech Service�ɂ�鉹���F���̐��x�͔��ɍ����ł��B
```

### 4. �ˑ��������̐ݒ�

```csharp
// App.xaml.cs�ł̎���
protected override void OnStartup(StartupEventArgs e)
{
    base.OnStartup(e);
    
    var services = new ServiceCollection();
    
    // �T�[�r�X�̓o�^
    ConfigureServices(services);
    
    ServiceProvider = services.BuildServiceProvider();
    
    // ���C���E�B���h�E�̕\��
    var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
    mainWindow.Show();
}

private void ConfigureServices(IServiceCollection services)
{
    // �V���O���g���T�[�r�X
    services.AddSingleton<ILogService, FileLogService>();
    services.AddSingleton<SecureStorageService>();
    services.AddSingleton<ISettingsService, JsonSettingsService>();
    services.AddSingleton<TranscriptionManager>();
    services.AddSingleton<UpdateCheckService>();
    
    // �g�����W�F���g�T�[�r�X
    services.AddTransient<IAudioCaptureService, NAudioCaptureService>();
    services.AddTransient<ISpeechRecognitionService, AzureSpeechRecognitionService>();
    services.AddTransient<IDataStorageService, FileStorageService>();
    
    // ViewModels
    services.AddTransient<MainViewModel>();
    services.AddTransient<SettingsViewModel>();
    
    // Views
    services.AddTransient<MainWindow>();
    services.AddTransient<SettingsWindow>();
}
```

## �݌v�̃|�C���g

1. **MVVM�A�[�L�e�N�`��**�F
   - CommunityToolkit.Mvvm���g�p����MVVM�p�^�[���̎���
   - ObservableProperty��RelayCommand�ɂ��Ȍ��ȃR�[�h

2. **�g����**�F
   - �����L���v�`���Ɖ����F���T�[�r�X�̒��ۉ�
   - RecognitionServiceSettings�̌p���ɂ��^���S�Ȑݒ�

3. **�G���[�n���h�����O**�F
   - �e�T�[�r�X�ł̓K�؂ȃG���[�ʒm���J�j�Y��
   - ���M���O�T�[�r�X�ɂ��G���[�L�^

4. **�f�[�^�Ǘ�**�F
   - JSON�x�[�X�̃V���v���ȃt�@�C���ۑ�
   - �Z�L���A�X�g���[�W�ɂ��API�L�[�̕ی�

5. **UI**�F
   - �V���v���Ŏg���₷���C���^�[�t�F�[�X
   - �őO�ʕ\����t�H���g�T�C�Y�Ȃǂ̃J�X�^�}�C�Y

6. **�ݒ�**�F
   - �A�v���P�[�V�����S�̂̐ݒ���ꌳ�Ǘ�
   - JSON�t�@�C���ł̊ȒP�ȕۑ��Ɠǂݍ���

## 1. �����K��

### �N���X�E�C���^�[�t�F�[�X
- �p�X�J���P�[�X�iPascalCase�j���g�p����i��: `AudioCaptureService`, `TranscriptionManager`�j
- �C���^�[�t�F�[�X�� "I" �v���t�B�b�N�X���g�p����i��: `IAudioCaptureService`�j
- ���ۃN���X�ɂ� "Base" ��ڔ����Ƃ��Ďg�p����i��: `ServiceBase`�j
- ��O�N���X�ɂ� "Exception" ��ڔ����Ƃ��Ďg�p����i��: `AudioCaptureException`�j

### �ϐ��E�t�B�[���h
- �v���C�x�[�g�t�B�[���h�ɂ� "_" �v���t�B�b�N�X���g�p����i��: `_captureInstance`�j
- CommunityToolkit.Mvvm��`[ObservableProperty]`�������g�p����ꍇ���v���t�B�b�N�X "_" ���g�p����
- �p�u���b�N�v���p�e�B�ɂ̓p�X�J���P�[�X���g�p����i��: `IsRecording`�j
- ���[�J���ϐ��ɂ̓L�������P�[�X�icamelCase�j���g�p����i��: `audioBuffer`�j
- �萔�͂��ׂđ啶���ŁA�P��̋�؂�ɂ̓A���_�[�X�R�A���g�p����i��: `MAX_BUFFER_SIZE`�j

### ���\�b�h�E�C�x���g
- �p�X�J���P�[�X���g�p����i��: `StartRecording()`, `DataAvailable`�j
- �C�x���g�n���h���� "Handler" �ڔ������g�p����i��: `ButtonClickHandler`�j
- Async���\�b�h�ɂ�Async�ڔ������g�p����i��: `StartRecordingAsync()`�j

## 2. �R�[�h�X�^�C��

### ���C�A�E�g
- �C���f���g�̓X�y�[�X4���g�p����
- 1�s�̍ő啶������120�����܂łƂ���
- �g���ʂ͓����s�ɊJ�n���A�V�����s�ŏI������i��: `if (condition) {`�j
- ���\�b�h�Ԃ�1�s�̋�s������
- �t�@�C���̍Ō�ɂ͋�s������

### �R�����g
- �R�[�h�ɔ񎩖��ȃ��W�b�N������ꍇ�̓R�����g��t����
- ���\�b�h�ɂ͗v��R�����g��t����i/// XML�h�L�������g�R�����g�j
- �p�u���b�NAPI�ɂ͊��S��XML�h�L�������g�R�����g��t����
- TODO�R�����g�ɂ͒S���҂ƃ`�P�b�g�ԍ����܂߂�i��: `// TODO: [username] #123: ����������������`�j
- ���{��R�����g����{�Ƃ���

```csharp
/// <summary>
/// �I�[�f�B�I�L���v�`�����J�n���܂�
/// </summary>
/// <param name="deviceId">�L���v�`������f�o�C�X��ID�Bnull�̏ꍇ�̓f�t�H���g�f�o�C�X���g�p���܂�</param>
/// <returns>�L���v�`���̊J�n�������������ǂ���</returns>
public async Task<bool> StartCaptureAsync(string deviceId = null)
{
    // ����
}
```

## 3. MVVM �p�^�[���̎���

### ViewModel�̋K��
- CommunityToolkit.Mvvm�p�b�P�[�W���g�p����
- `[ObservableProperty]` �������g�p���ăv���p�e�B���`����
- `[RelayCommand]` �������g�p���ăR�}���h���`����
- ViewModel��UI���W�b�N�݂̂��܂ށi�r�W�l�X���W�b�N�͊܂܂Ȃ��j
- ViewModel�N���X�ɂ� `partial` �L�[���[�h���g�p����

```csharp
public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _isRecording;

    [ObservableProperty]
    private string _statusMessage = "��������";

    [RelayCommand]
    private async Task StartRecordingAsync()
    {
        // ����
        IsRecording = true;
        StatusMessage = "�^����...";
    }
}
```

### ���f���Ƃ̕���
- ViewModel�̓��f���𒼐ڎQ�Ƃ����A�K�v�ȃf�[�^���R�s�[����
- �o�����o�C���f�B���O�̏ꍇ�͖����I�ȕϊ����s��
- �R���N�V������ObservableCollection���g�p����

## 4. �G���[����

### ��O����
- �\���\�ȗ�O�͓K�؂ɃL���b�`���A���[�U�[�t�����h���[�ȃ��b�Z�[�W��\������
- �჌�x���ȗ�O�͏�ʃ��C���[�ŃL���b�`���A�K�؂Ƀ��b�v����
- �T�[�r�X���\�b�h�ł͗�O���X���[�����AResult<T>�p�^�[�����g�p����
- �����F���T�[�r�X�Ƃ̒ʐM�G���[�͐�p�̗�O�N���X `SpeechRecognitionException` ���g�p����

```csharp
public async Task<Result<TranscriptionSession>> LoadSessionAsync(string id)
{
    try
    {
        // ����
        return Result<TranscriptionSession>.Success(session);
    }
    catch (Exception ex)
    {
        _logService.LogError($"Session loading failed: {ex.Message}", ex);
        return Result<TranscriptionSession>.Failure("�Z�b�V�����̓ǂݍ��݂Ɏ��s���܂���");
    }
}
```

### ���M���O
- �G���[�ƃ��[�j���O�͕K���L�^����
- �f�o�b�O���̓f�o�b�O���x���ŋL�^����
- �l����API�L�[�Ȃǂ̋@�����̓��O�ɋL�^���Ȃ�
- Azure Speech Service�Ƃ̒ʐM���O�̓f�o�b�O���x���ŏڍׂɋL�^����

### ���m�̃G���[�p�^�[��
- �F�؃G���[�iAPI�L�[�����j: ���[�U�[�ɐݒ��ʂ�\��
- �l�b�g���[�N�G���[: �Ď��s�I�v�V�������
- �f�o�C�X�A�N�Z�X�G���[: �f�o�C�X�I����ʂ�\��

## 5. �񓯊��v���O���~���O

### ��{���[��
- UI���u���b�N����\���̂��鑀��͔񓯊��ɂ���
- �񓯊����\�b�h�ɂ� "Async" �ڔ�����t����i��: `LoadDataAsync()`�j
- `async void` �͔����A����� `async Task` ���g�p����i�C�x���g�n���h���������j
- �L�����Z���g�[�N����K�؂Ɏg�p���A�����Ԏ��s����鑀��̓L�����Z���\�ɂ���
- �����F�������͕K���L�����Z���g�[�N�����󂯓����悤�ɂ���

### �񓯊��C�x���g
- �񓯊��C�x���g�ɂ� `EventHandler<TEventArgs>` �̑���� `Func<TEventArgs, Task>` ���g�p����

```csharp
public event Func<AudioDataEventArgs, Task> AudioDataAvailableAsync;

// �Ăяo����
private async Task OnAudioDataAvailableAsync(AudioDataEventArgs e)
{
    var handlers = AudioDataAvailableAsync;
    if (handlers != null)
    {
        foreach (var handler in handlers.GetInvocationList().Cast<Func<AudioDataEventArgs, Task>>())
        {
            await handler(e);
        }
    }
}
```

## 6. �ˑ�������

### �K��
- �R���X�g���N�^�C���W�F�N�V�������g�p����
- �T�[�r�X�̓C���^�[�t�F�[�X�Ƃ��Ē�������
- �R���X�g���N�^�p�����[�^�͕K�{�ˑ��֌W�݂̂��󂯎��
- �I�v�V�����̈ˑ��֌W�̓v���p�e�B�C���W�F�N�V�������g�p����
- Microsoft.Extensions.DependencyInjection���g�p����

```csharp
public class TranscriptionService
{
    private readonly IAudioCaptureService _audioCaptureService;
    private readonly ISpeechRecognitionService _speechRecognitionService;
    private readonly ILogService _logService;
    
    public TranscriptionService(
        IAudioCaptureService audioCaptureService,
        ISpeechRecognitionService speechRecognitionService,
        ILogService logService)
    {
        _audioCaptureService = audioCaptureService;
        _speechRecognitionService = speechRecognitionService;
        _logService = logService;
    }
}
```

### �T�[�r�X�o�^
- �V���O���g���Ƃ��ēo�^����T�[�r�X:
  - ILogService
  - ISettingsService
  - TranscriptionManager
- �g�����W�F���g�Ƃ��ēo�^����T�[�r�X:
  - IAudioCaptureService
  - ISpeechRecognitionService
  - ViewModel�N���X

## 7. �e�X�g

### �P�̃e�X�g
- �e�N���X�ɂ͑Ή�����P�̃e�X�g���쐬����
- �e�X�g�N���X���͑ΏۃN���X�� + "Tests" �Ƃ���i��: `AudioCaptureServiceTests`�j
- �e�X�g���\�b�h���́u�e�X�g�Ώ�_����_���҂���錋�ʁv�̌`���Ƃ���i��: `ProcessAudio_EmptyBuffer_ThrowsArgumentException`�j
- ���b�N�t���[�����[�N�iMoq�j���g�p���Ĉˑ��֌W�����b�N������
- Azure��API�Ăяo���͕K�����b�N������

### �����e�X�g
- �d�v�ȃ��[�X�P�[�X�ɑ΂��ē����e�X�g���쐬����
- �O���ˑ��֌W�iAzure�Ȃǂ�API�j�̓��b�N������
- �����e�X�g�ł͎��ۂ�UI���g�p�����AViewModel���x���Ńe�X�g����

## 8. �Z�L�����e�B

### API�L�[�Ǘ�
- API�L�[�̓\�[�X�R�[�h�ɒ��ڋL�q���Ȃ�
- Windows DPAPI���g�p���ĈÍ������ĕۑ�����
- ���[�U�[�ݒ�ł�API�L�[���}�X�N�\������
- �J�����ł�User Secrets���g�p����

```csharp
public class SecureStorageService
{
    public void StoreSecureData(string key, string value)
    {
        byte[] encryptedData = ProtectedData.Protect(
            Encoding.UTF8.GetBytes(value),
            null,
            DataProtectionScope.CurrentUser);
            
        // �Í����f�[�^�̕ۑ����W�b�N
    }
}
```

### �G���[���b�Z�[�W
- �G���[���b�Z�[�W�ɃX�^�b�N�g���[�X��Z�p�I�ڍׂ��܂߂Ȃ�
- �O������̃G���[���b�Z�[�W�͒��ڕ\�������A���[�U�[�t�����h���[�ȃ��b�Z�[�W�ɕϊ�����
- �G���[���b�Z�[�W�͓��{��ŕ\������

## 9. �p�t�H�[�}���X

### ��������
- �I�[�f�B�I�o�b�t�@�T�C�Y�͓K�؂ɐݒ肷��i����l�F2048�T���v���j
- �����L���v�`���͕ʃX���b�h�Ŏ��s����
- ��ʂ̃f�[�^�������̓������g�p�ʂɒ��ӂ���
- ���ׂĂ̏����ɂ�����1�t���[��������̏������Ԃ�16ms(60fps)�𒴂��Ȃ��悤�ɂ���
- Azure Speech Service�ւ̑��M�f�[�^�͕K�v�ɉ����Ĉ��k�܂��͊Ԉ������s��

```csharp
private void CaptureInstance_DataAvailable(object sender, WaveInEventArgs e)
{
    if (_waveFileWriter != null)
    {
        // �����f�[�^���t�@�C���ɏ�������
        _waveFileWriter.Write(e.Buffer, 0, e.BytesRecorded);
    }

    // Azure Speech Service�Ƀf�[�^�𑗐M
    if (_audioInputStream != null && _azureInitialized)
    {
        // �����f�[�^��Azure�ɑ��M
        _audioInputStream.Write(e.Buffer, e.BytesRecorded);
    }
}
```

### ���\�[�X�Ǘ�
- IDisposable����������N���X�ł́A���\�[�X��K�؂ɉ������
- using �X�e�[�g�����g���g�p���邩�Atry-finally �p�^�[����K�p����
- ���ׂĂ�IDisposable�����N���X��Dispose()���\�b�h����������
- �A���}�l�[�W�h���\�[�X��ێ�����N���X�ł́A�t�@�C�i���C�U����������

```csharp
public class AudioCaptureService : IAudioCaptureService, IDisposable
{
    private WasapiLoopbackCapture _captureInstance;
    private bool _disposed = false;
    
    // ����
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // �}�l�[�W�h���\�[�X�̉��
                _captureInstance?.Dispose();
            }
            
            // �A���}�l�[�W�h���\�[�X�̉��
            
            _disposed = true;
        }
    }
}
```

## 10. �v���W�F�N�g�\��

### �t�H���_�\��
- �@�\���W���[�����ƂɃt�H���_�𕪂���i��: `AudioCapture`, `SpeechRecognition`�j
- ���ʃR���|�[�l���g�� `Common` �t�H���_�ɔz�u����
- ViewModel�͑Ή�����View�Ɠ������O��Ԃɔz�u����
- ���W���[�����ƂɃC���^�[�t�F�[�X�Ǝ����𕪂���

```
DesktopAudioTranscriptionApp
������ Modules
��   ������ AudioCapture
��   ��   ������ Interfaces
��   ��   ������ Services
��   ��   ������ Models
��   ������ SpeechRecognition
��   ��   ������ Interfaces
��   ��   ������ Services
��   ��   ������ Models
��   ������ Data
��       ������ Interfaces
��       ������ Services
��       ������ Models
������ Common
��   ������ Results
��   ������ Extensions
��   ������ Helpers
������ ViewModels
������ Views
������ Services
    ������ Interfaces
    ������ Implementations
```

### �A�Z���u���Q��
- �z�Q�Ƃ������
- �K�v�ŏ����̎Q�Ƃ݂̂�ǉ�����
- NuGet�p�b�P�[�W�͖����I�ȃo�[�W�����w����s��
- ��v�p�b�P�[�W�̃o�[�W����:
  - NAudio: 2.2.0�ȏ�
  - Microsoft.CognitiveServices.Speech: 1.32.0�ȏ�
  - CommunityToolkit.Mvvm: 8.2.0�ȏ�

## 11. UI�݌v��XAML�R�[�f�B���O�K��

### XAML�\��
- �v�f�̑�����1�s��1���L�q����i�����̏ꍇ�j
- ���ʃX�^�C���̓��\�[�X�f�B�N�V���i���ɒ�`����
- �o�C���f�B���O��xaml�ōs���A�R�[�h�r�n�C���h�ł̒��ړI��UI����͔�����
- ���{�ꃊ�\�[�X�͑S�ă��\�[�X�t�@�C���ɒ�`����

```xml
<Button 
    x:Name="startButton"
    Content="{Binding StartButtonText}" 
    Command="{Binding StartStopRecordingCommand}"
    Margin="10"
    Width="120"
    Height="30" />
```

### �f�U�C���p�^�[��
- UserControl���g�p���čė��p�\��UI�v�f���쐬����
- DataTemplate���g�p���ăf�[�^�̕\�����@���`����
- �R���o�[�^�[���g�p���ĕ\���t�H�[�}�b�g�𒲐�����
- ListView�̗��p���̓J�X�^��ItemTemplate���`����

### �e�[�}�ƃX�^�C��
- ���C���J���[: #3498db (��)
- �Z�J���_���J���[: #2c3e50 (��)
- �A�N�Z���g�J���[: #e74c3c (��)
- �t�H���g: ���C���I (UI)�AConsolas (���O)

## 12. �R�[�h�i���Ǘ�

### �ÓI���
- Microsoft.CodeAnalysis.NetAnalyzers ���g�p����
- �x���͉������邩�A�����I�ɗ}�����R���R�����g�ŋL�q����
- �R�[�h�X�^�C���̈�ѐ���ۂ��߂�EditorConfig���g�p����
- �ȉ��̌x���͖������Ȃ�:
  - CS0618: �p�~�\���API�g�p
  - CS8602: null�Q�Ƃ̉\��
  - CS8604: null�̉\��������l�̈����n��

### �R�[�h���r���[
- ���ׂĂ̕ύX�̓��r���[���󂯂�
- ���r���[�ł͋@�\���A�Z�L�����e�B�A�p�t�H�[�}���X�A�e�X�g�\�����m�F����
- �R�~�b�g���b�Z�[�W�͊֘A����`�P�b�g�ԍ�����n�߂�i��: `#123: �����F���@�\�̎���`�j

## 13. �v���W�F�N�g�ŗL�̃��[��

### NAudio �֘A
- WasapiLoopbackCapture���g�p���ăf�X�N�g�b�v�������L���v�`������
- �����t�H�[�}�b�g��16kHz, 16bit, ���m��������{�Ƃ���
- �I�[�f�B�I�f�o�C�X�̃��X�g�͒���I�ɍX�V����

### Azure Speech Service �֘A
- ���{��F������{�Ƃ��� (`ja-JP`)
- �F���T�[�r�X�̏������͔񓯊��ōs��
- �F���L�����Z�����͓K�؂ȃC�x���g�ʒm���s��
- �e�F�����ʂɂ̓^�C���X�^���v��t�^����

### �f�[�^�ۑ�
- �e�L�X�g�ۑ��� UTF-8 �G���R�[�f�B���O���g�p����
- �����t�@�C���� WAV �`���ŕۑ�����
- �t�@�C�����ɂ͓������܂߂�`���Ƃ���
- �����ۑ��̗L��/������؂�ւ��\�ɂ���

### UI������
- �����F�����̓v���O���X�C���W�P�[�^��\������
- ��������UI�̃t���[�Y��h�����߁A�d�������͕ʃX���b�h�Ŏ��s����
- �^�C���X�^���v�͌��₷���悤�ɐF�t���\������

## 14. GitHub Copilot �����w��

### �R�[�h�������f��
- NAudio���g�p���������L���v�`���R�[�h�̃p�^�[��:
  ```csharp
  private void StartCapture()
  {
      _captureInstance = new WasapiLoopbackCapture();
      _captureInstance.DataAvailable += CaptureInstance_DataAvailable;
      _captureInstance.RecordingStopped += CaptureInstance_RecordingStopped;
      _captureInstance.StartRecording();
  }
  ```

- Azure Speech Service�������p�^�[��:
  ```csharp
  private async Task InitializeSpeechRecognizerAsync()
  {
      _speechConfig = SpeechConfig.FromSubscription(_apiKey, _region);
      _speechConfig.SpeechRecognitionLanguage = "ja-JP";
      
      _audioInputStream = AudioInputStream.CreatePushStream();
      _audioConfig = AudioConfig.FromStreamInput(_audioInputStream);
      
      _recognizer = new SpeechRecognizer(_speechConfig, _audioConfig);
      _recognizer.Recognized += Recognizer_Recognized;
      
      await _recognizer.StartContinuousRecognitionAsync();
  }
  ```

### ������ׂ����p�^�[��
- API�L�[�̃n�[�h�R�[�h
- UI�X���b�h�ł̒����ԏ���
- �񓯊�����̓����I�ȑҋ@
- ���\�[�X�̔���
- ��O�َ̖E

### �ėp�I�e���v���[�g
- Result<T>�p�^�[��:
  ```csharp
  public class Result<T>
  {
      public bool IsSuccess { get; }
      public T Value { get; }
      public string ErrorMessage { get; }
      
      private Result(bool isSuccess, T value, string errorMessage)
      {
          IsSuccess = isSuccess;
          Value = value;
          ErrorMessage = errorMessage;
      }
      
      public static Result<T> Success(T value) => new Result<T>(true, value, null);
      public static Result<T> Failure(string errorMessage) => new Result<T>(false, default, errorMessage);
  }
  ```
