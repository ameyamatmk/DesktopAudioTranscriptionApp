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
