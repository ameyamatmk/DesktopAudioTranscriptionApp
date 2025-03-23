# 音声文字起こしツール - GitHub Copilot カスタムインストラクション

このドキュメントは、Windows向け音声文字起こしツールの開発におけるコーディングルールと設計ガイドラインを定義しています。GitHub Copilotは以下のルールに従ってコード生成とコード支援を行ってください。

## プロジェクト概要

- **プロジェクト名**: アプリケーション音声文字起こしツール
- **目的**: Windows上でのアプリケーション出力音声をリアルタイムで文字起こし
- **プラットフォーム**: Windows
- **開発言語**: C#
- **主要技術**: 
  - WPF (Windows Presentation Foundation)
  - NAudio (オーディオキャプチャ)
  - Azure Speech Service (音声認識API)
  - MVVM (CommunityToolkit.Mvvm)

## 基本アーキテクチャ

```
DesktopAudioTranscriptionApp
├── Modules
│   ├── AudioCapture     # 音声キャプチャ関連
│   ├── SpeechRecognition # 音声認識関連
│   ├── Data             # データ管理関連
│   └── UI               # UI関連
├── Models               # ビジネスロジックとデータモデル
├── ViewModels           # MVVM用のViewModelクラス
├── Views                # UIビュー
└── Services             # 共通サービス
```

GitHub Copilot カスタムインストラクションにコミットメッセージの規則を追加します。コミットメッセージの冒頭に接頭辞（プレフィックス）を付けるのは、変更の種類を簡潔に示すための一般的な慣行です。以下にその部分を追加しました：

## クラス設計

### 1. アプリケーション全体構造

```
DesktopAudioTranscriptionApp
├── Modules
│   ├── AudioCapture     # 音声キャプチャ関連
│   ├── SpeechRecognition # 音声認識関連
│   ├── Data             # データ管理関連
│   └── UI               # UI関連
├── Models               # ビジネスロジックとデータモデル
├── ViewModels           # MVVM用のViewModelクラス
├── Views                # UIビュー
└── Services             # 共通サービス
```

### 2. AudioCapture モジュール

```csharp
namespace DesktopAudioTranscriptionApp.Modules.AudioCapture
{
    // 音声キャプチャのための抽象インターフェース
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

    // 実装クラス - NAudioを利用
    public class NAudioCaptureService : IAudioCaptureService, IDisposable
    {
        private WasapiLoopbackCapture _captureInstance;
        // 実装メソッド
    }

    // イベント引数クラス
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

### 3. SpeechRecognition モジュール

```csharp
namespace DesktopAudioTranscriptionApp.Modules.SpeechRecognition
{
    // 音声認識のための抽象インターフェース
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

    // Azure Speech Service実装
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
        
        // 実装メソッド
    }

    // イベント引数
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

### 4. Data モジュール

```csharp
namespace DesktopAudioTranscriptionApp.Modules.Data
{
    // データ保存のためのインターフェース
    public interface IDataStorageService
    {
        Task SaveTranscriptionAsync(TranscriptionSession session);
        Task SaveAudioAsync(byte[] audioData, TranscriptionSession session);
        Task<List<TranscriptionSession>> GetSessionsAsync();
        Task<TranscriptionSession> GetSessionAsync(string sessionId);
        Task DeleteSessionAsync(string sessionId);
    }

    // ファイルベースの実装
    public class FileStorageService : IDataStorageService
    {
        private readonly AppSettings _settings;
        
        public FileStorageService(ISettingsService settingsService)
        {
            _settings = settingsService.LoadSettings();
        }
        
        // 実装メソッド
    }

    // トランスクリプション機能のためのマネージャー
    public class TranscriptionManager
    {
        private readonly IDataStorageService _storageService;
        private TranscriptionSession _currentSession;
        
        public event EventHandler<TranscriptionItemAddedEventArgs> ItemAdded;
        
        // メソッド
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

    // イベント引数
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
    // トランスクリプションセッション
    public class TranscriptionSession
    {
        public string Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public List<TranscriptionItem> Items { get; set; } = new List<TranscriptionItem>();
        public string AudioFilePath { get; set; }
        public string TextFilePath { get; set; }
    }

    // 個々のトランスクリプションアイテム
    public class TranscriptionItem
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }
        public TimeSpan RelativeTime { get; set; } // セッション開始からの時間
    }

    // 音声認識サービス設定の基底クラス
    public abstract class RecognitionServiceSettings
    {
        public string ServiceType { get; }

        protected RecognitionServiceSettings(string serviceType)
        {
            ServiceType = serviceType;
        }
    }

    // Azure Speech Service設定
    public class AzureSpeechSettings : RecognitionServiceSettings
    {
        public string Key { get; set; }
        public string Region { get; set; }
        public string Language { get; set; } = "ja-JP";
        
        public AzureSpeechSettings() : base("Azure") { }
    }

    // アプリケーション設定
    public class AppSettings
    {
        // 基本設定
        public RecognitionServiceSettings RecognitionSettings { get; set; }
        public bool AlwaysOnTop { get; set; }
        public int FontSize { get; set; }
        public TimestampFormat TimestampFormat { get; set; }
        
        // 音声設定
        public string AudioDeviceId { get; set; }
        public bool AutoDetectDevices { get; set; }
        public int SampleRate { get; set; }
        public int Channels { get; set; }
        public int BufferSize { get; set; }
        
        // 保存設定
        public bool AutoSaveTranscription { get; set; }
        public string TranscriptionSavePath { get; set; }
        public string TranscriptionFileNameFormat { get; set; }
        public bool SaveAudio { get; set; }
        public AudioFileFormat AudioFileFormat { get; set; }
    }

    // 列挙型
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
    // メイン画面のViewModel
    public partial class MainViewModel : ObservableObject
    {
        private readonly IAudioCaptureService _audioCaptureService;
        private readonly ISpeechRecognitionService _speechRecognitionService;
        private readonly TranscriptionManager _transcriptionManager;
        private readonly ISettingsService _settingsService;
        
        // プロパティ
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
        
        // コマンド
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
            // 実装
        }
        
        [RelayCommand]
        private void Clear()
        {
            // 実装
        }
        
        [RelayCommand]
        private void OpenSettings()
        {
            // 実装
        }
        
        // メソッド
        private async Task StartRecordingAsync()
        {
            // 実装
        }
        
        private async Task StopRecordingAsync()
        {
            // 実装
        }
    }

    // 設定画面のViewModel
    public partial class SettingsViewModel : ObservableObject
    {
        private readonly ISettingsService _settingsService;
        private AppSettings _settings;
        
        // プロパティ (自動生成)
        [ObservableProperty]
        private string _azureSpeechKey;
        
        [ObservableProperty]
        private string _azureSpeechRegion;
        
        [ObservableProperty]
        private bool _alwaysOnTop;
        
        [ObservableProperty]
        private string _selectedFontSize;
        
        // 他の設定プロパティ...
        
        // コマンド
        [RelayCommand]
        private async Task Save()
        {
            // 設定を保存
        }
        
        [RelayCommand]
        private void Cancel()
        {
            // キャンセル処理
        }
        
        [RelayCommand]
        private void BrowseFolder()
        {
            // フォルダ選択ダイアログを表示
        }
    }

    // トランスクリプションアイテムのViewModel
    public class TranscriptionItemViewModel : ObservableObject
    {
        private readonly TranscriptionItem _model;
        private readonly TimestampFormat _format;
        
        // プロパティ
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
    // 設定管理サービス
    public interface ISettingsService
    {
        AppSettings LoadSettings();
        Task SaveSettingsAsync(AppSettings settings);
    }

    // JSON設定ファイル実装
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
        
        // 実装メソッド
    }

    // APIキー安全管理サービス
    public class SecureStorageService
    {
        public void StoreSecureData(string key, string value)
        {
            // Windows DPAPIを使用して安全に保存
        }
        
        public string RetrieveSecureData(string key)
        {
            // Windows DPAPIを使用して取得
        }
    }

    // ロギングサービス
    public interface ILogService
    {
        void LogInfo(string message);
        void LogWarning(string message);
        void LogError(string message, Exception ex = null);
    }

    // ファイルベースのログ実装
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
        
        // 実装メソッド
    }

    // アプリケーション更新確認サービス
    public class UpdateCheckService
    {
        public async Task<UpdateInfo> CheckForUpdatesAsync()
        {
            // 実装
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

## データ設計

### 1. 設定ファイル（JSON形式）

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

### 2. トランスクリプションセッションデータ（JSON）

```json
{
  "SessionId": "20250323_145502",
  "StartTime": "2025-03-23T14:55:02.123Z",
  "EndTime": "2025-03-23T15:10:45.678Z",
  "Items": [
    {
      "Id": "item1",
      "Text": "これは音声認識のテストです。",
      "Timestamp": "2025-03-23T14:55:20.456Z",
      "RelativeTime": "00:00:18.333"
    },
    {
      "Id": "item2",
      "Text": "Windowsアプリケーションの音声をリアルタイムで文字に起こします。",
      "Timestamp": "2025-03-23T14:55:32.789Z",
      "RelativeTime": "00:00:30.666"
    }
  ],
  "AudioFilePath": "audio_20250323_145502.wav",
  "TextFilePath": "transcript_20250323_145502.txt"
}
```

### 3. テキストエクスポート形式

#### 基本テキスト形式（タイムスタンプあり）

```
# 音声文字起こし - 2025/03/23 14:55:02

[14:55:20] これは音声認識のテストです。
[14:55:32] Windowsアプリケーションの音声をリアルタイムで文字に起こします。
[14:56:05] NAudioを使用してデスクトップ音声をキャプチャしています。
[14:56:40] Azure Speech Serviceによる音声認識の精度は非常に高いです。
```

#### 基本テキスト形式（タイムスタンプなし）

```
これは音声認識のテストです。
Windowsアプリケーションの音声をリアルタイムで文字に起こします。
NAudioを使用してデスクトップ音声をキャプチャしています。
Azure Speech Serviceによる音声認識の精度は非常に高いです。
```

#### マークダウン形式

```markdown
# 音声文字起こし結果

## セッション情報
- 開始時間: 2025/03/23 14:55:02
- 終了時間: 2025/03/23 15:10:45
- 継続時間: 15分43秒

## 認識テキスト

### [14:55:20]
これは音声認識のテストです。

### [14:55:32]
Windowsアプリケーションの音声をリアルタイムで文字に起こします。

### [14:56:05]
NAudioを使用してデスクトップ音声をキャプチャしています。

### [14:56:40]
Azure Speech Serviceによる音声認識の精度は非常に高いです。
```

### 4. 依存性注入の設定

```csharp
// App.xaml.csでの実装
protected override void OnStartup(StartupEventArgs e)
{
    base.OnStartup(e);
    
    var services = new ServiceCollection();
    
    // サービスの登録
    ConfigureServices(services);
    
    ServiceProvider = services.BuildServiceProvider();
    
    // メインウィンドウの表示
    var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
    mainWindow.Show();
}

private void ConfigureServices(IServiceCollection services)
{
    // シングルトンサービス
    services.AddSingleton<ILogService, FileLogService>();
    services.AddSingleton<SecureStorageService>();
    services.AddSingleton<ISettingsService, JsonSettingsService>();
    services.AddSingleton<TranscriptionManager>();
    services.AddSingleton<UpdateCheckService>();
    
    // トランジェントサービス
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

## 設計のポイント

1. **MVVMアーキテクチャ**：
   - CommunityToolkit.Mvvmを使用したMVVMパターンの実装
   - ObservablePropertyやRelayCommandによる簡潔なコード

2. **拡張性**：
   - 音声キャプチャと音声認識サービスの抽象化
   - RecognitionServiceSettingsの継承による型安全な設定

3. **エラーハンドリング**：
   - 各サービスでの適切なエラー通知メカニズム
   - ロギングサービスによるエラー記録

4. **データ管理**：
   - JSONベースのシンプルなファイル保存
   - セキュアストレージによるAPIキーの保護

5. **UI**：
   - シンプルで使いやすいインターフェース
   - 最前面表示やフォントサイズなどのカスタマイズ

6. **設定**：
   - アプリケーション全体の設定を一元管理
   - JSONファイルでの簡単な保存と読み込み

## 1. 命名規則

### クラス・インターフェース
- パスカルケース（PascalCase）を使用する（例: `AudioCaptureService`, `TranscriptionManager`）
- インターフェースは "I" プレフィックスを使用する（例: `IAudioCaptureService`）
- 抽象クラスには "Base" を接尾辞として使用する（例: `ServiceBase`）
- 例外クラスには "Exception" を接尾辞として使用する（例: `AudioCaptureException`）

### 変数・フィールド
- プライベートフィールドには "_" プレフィックスを使用する（例: `_captureInstance`）
- CommunityToolkit.Mvvmの`[ObservableProperty]`属性を使用する場合もプレフィックス "_" を使用する
- パブリックプロパティにはパスカルケースを使用する（例: `IsRecording`）
- ローカル変数にはキャメルケース（camelCase）を使用する（例: `audioBuffer`）
- 定数はすべて大文字で、単語の区切りにはアンダースコアを使用する（例: `MAX_BUFFER_SIZE`）

### メソッド・イベント
- パスカルケースを使用する（例: `StartRecording()`, `DataAvailable`）
- イベントハンドラは "Handler" 接尾辞を使用する（例: `ButtonClickHandler`）
- AsyncメソッドにはAsync接尾辞を使用する（例: `StartRecordingAsync()`）

## 2. コードスタイル

### レイアウト
- インデントはスペース4つを使用する
- 1行の最大文字数は120文字までとする
- 波括弧は同じ行に開始し、新しい行で終了する（例: `if (condition) {`）
- メソッド間に1行の空行を入れる
- ファイルの最後には空行を入れる

### コメント
- コードに非自明なロジックがある場合はコメントを付ける
- メソッドには要約コメントを付ける（/// XMLドキュメントコメント）
- パブリックAPIには完全なXMLドキュメントコメントを付ける
- TODOコメントには担当者とチケット番号を含める（例: `// TODO: [username] #123: 実装を完了させる`）
- 日本語コメントを基本とする

```csharp
/// <summary>
/// オーディオキャプチャを開始します
/// </summary>
/// <param name="deviceId">キャプチャするデバイスのID。nullの場合はデフォルトデバイスを使用します</param>
/// <returns>キャプチャの開始が成功したかどうか</returns>
public async Task<bool> StartCaptureAsync(string deviceId = null)
{
    // 実装
}
```

## 3. MVVM パターンの実装

### ViewModelの規則
- CommunityToolkit.Mvvmパッケージを使用する
- `[ObservableProperty]` 属性を使用してプロパティを定義する
- `[RelayCommand]` 属性を使用してコマンドを定義する
- ViewModelはUIロジックのみを含む（ビジネスロジックは含まない）
- ViewModelクラスには `partial` キーワードを使用する

```csharp
public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _isRecording;

    [ObservableProperty]
    private string _statusMessage = "準備完了";

    [RelayCommand]
    private async Task StartRecordingAsync()
    {
        // 実装
        IsRecording = true;
        StatusMessage = "録音中...";
    }
}
```

### モデルとの分離
- ViewModelはモデルを直接参照せず、必要なデータをコピーする
- 双方向バインディングの場合は明示的な変換を行う
- コレクションはObservableCollectionを使用する

## 4. エラー処理

### 例外処理
- 予測可能な例外は適切にキャッチし、ユーザーフレンドリーなメッセージを表示する
- 低レベルな例外は上位レイヤーでキャッチし、適切にラップする
- サービスメソッドでは例外をスローせず、Result<T>パターンを使用する
- 音声認識サービスとの通信エラーは専用の例外クラス `SpeechRecognitionException` を使用する

```csharp
public async Task<Result<TranscriptionSession>> LoadSessionAsync(string id)
{
    try
    {
        // 実装
        return Result<TranscriptionSession>.Success(session);
    }
    catch (Exception ex)
    {
        _logService.LogError($"Session loading failed: {ex.Message}", ex);
        return Result<TranscriptionSession>.Failure("セッションの読み込みに失敗しました");
    }
}
```

### ロギング
- エラーとワーニングは必ず記録する
- デバッグ情報はデバッグレベルで記録する
- 個人情報やAPIキーなどの機密情報はログに記録しない
- Azure Speech Serviceとの通信ログはデバッグレベルで詳細に記録する

### 既知のエラーパターン
- 認証エラー（APIキー無効）: ユーザーに設定画面を表示
- ネットワークエラー: 再試行オプションを提供
- デバイスアクセスエラー: デバイス選択画面を表示

## 5. 非同期プログラミング

### 基本ルール
- UIをブロックする可能性のある操作は非同期にする
- 非同期メソッドには "Async" 接尾辞を付ける（例: `LoadDataAsync()`）
- `async void` は避け、代わりに `async Task` を使用する（イベントハンドラを除く）
- キャンセルトークンを適切に使用し、長時間実行される操作はキャンセル可能にする
- 音声認識処理は必ずキャンセルトークンを受け入れるようにする

### 非同期イベント
- 非同期イベントには `EventHandler<TEventArgs>` の代わりに `Func<TEventArgs, Task>` を使用する

```csharp
public event Func<AudioDataEventArgs, Task> AudioDataAvailableAsync;

// 呼び出し方
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

## 6. 依存性注入

### 規則
- コンストラクタインジェクションを使用する
- サービスはインターフェースとして注入する
- コンストラクタパラメータは必須依存関係のみを受け取る
- オプションの依存関係はプロパティインジェクションを使用する
- Microsoft.Extensions.DependencyInjectionを使用する

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

### サービス登録
- シングルトンとして登録するサービス:
  - ILogService
  - ISettingsService
  - TranscriptionManager
- トランジェントとして登録するサービス:
  - IAudioCaptureService
  - ISpeechRecognitionService
  - ViewModelクラス

## 7. テスト

### 単体テスト
- 各クラスには対応する単体テストを作成する
- テストクラス名は対象クラス名 + "Tests" とする（例: `AudioCaptureServiceTests`）
- テストメソッド名は「テスト対象_条件_期待される結果」の形式とする（例: `ProcessAudio_EmptyBuffer_ThrowsArgumentException`）
- モックフレームワーク（Moq）を使用して依存関係をモック化する
- AzureのAPI呼び出しは必ずモック化する

### 統合テスト
- 重要なユースケースに対して統合テストを作成する
- 外部依存関係（AzureなどのAPI）はモック化する
- 統合テストでは実際のUIを使用せず、ViewModelレベルでテストする

## 8. セキュリティ

### APIキー管理
- APIキーはソースコードに直接記述しない
- Windows DPAPIを使用して暗号化して保存する
- ユーザー設定ではAPIキーをマスク表示する
- 開発環境ではUser Secretsを使用する

```csharp
public class SecureStorageService
{
    public void StoreSecureData(string key, string value)
    {
        byte[] encryptedData = ProtectedData.Protect(
            Encoding.UTF8.GetBytes(value),
            null,
            DataProtectionScope.CurrentUser);
            
        // 暗号化データの保存ロジック
    }
}
```

### エラーメッセージ
- エラーメッセージにスタックトレースや技術的詳細を含めない
- 外部からのエラーメッセージは直接表示せず、ユーザーフレンドリーなメッセージに変換する
- エラーメッセージは日本語で表示する

## 9. パフォーマンス

### 音声処理
- オーディオバッファサイズは適切に設定する（既定値：2048サンプル）
- 音声キャプチャは別スレッドで実行する
- 大量のデータ処理時はメモリ使用量に注意する
- すべての処理において1フレームあたりの処理時間が16ms(60fps)を超えないようにする
- Azure Speech Serviceへの送信データは必要に応じて圧縮または間引きを行う

```csharp
private void CaptureInstance_DataAvailable(object sender, WaveInEventArgs e)
{
    if (_waveFileWriter != null)
    {
        // 音声データをファイルに書き込み
        _waveFileWriter.Write(e.Buffer, 0, e.BytesRecorded);
    }

    // Azure Speech Serviceにデータを送信
    if (_audioInputStream != null && _azureInitialized)
    {
        // 音声データをAzureに送信
        _audioInputStream.Write(e.Buffer, e.BytesRecorded);
    }
}
```

### リソース管理
- IDisposableを実装するクラスでは、リソースを適切に解放する
- using ステートメントを使用するか、try-finally パターンを適用する
- すべてのIDisposable実装クラスでDispose()メソッドを実装する
- アンマネージドリソースを保持するクラスでは、ファイナライザも実装する

```csharp
public class AudioCaptureService : IAudioCaptureService, IDisposable
{
    private WasapiLoopbackCapture _captureInstance;
    private bool _disposed = false;
    
    // 実装
    
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
                // マネージドリソースの解放
                _captureInstance?.Dispose();
            }
            
            // アンマネージドリソースの解放
            
            _disposed = true;
        }
    }
}
```

## 10. プロジェクト構造

### フォルダ構成
- 機能モジュールごとにフォルダを分ける（例: `AudioCapture`, `SpeechRecognition`）
- 共通コンポーネントは `Common` フォルダに配置する
- ViewModelは対応するViewと同じ名前空間に配置する
- モジュールごとにインターフェースと実装を分ける

```
DesktopAudioTranscriptionApp
├── Modules
│   ├── AudioCapture
│   │   ├── Interfaces
│   │   ├── Services
│   │   └── Models
│   ├── SpeechRecognition
│   │   ├── Interfaces
│   │   ├── Services
│   │   └── Models
│   └── Data
│       ├── Interfaces
│       ├── Services
│       └── Models
├── Common
│   ├── Results
│   ├── Extensions
│   └── Helpers
├── ViewModels
├── Views
└── Services
    ├── Interfaces
    └── Implementations
```

### アセンブリ参照
- 循環参照を避ける
- 必要最小限の参照のみを追加する
- NuGetパッケージは明示的なバージョン指定を行う
- 主要パッケージのバージョン:
  - NAudio: 2.2.0以上
  - Microsoft.CognitiveServices.Speech: 1.32.0以上
  - CommunityToolkit.Mvvm: 8.2.0以上

## 11. UI設計とXAMLコーディング規則

### XAML構造
- 要素の属性は1行に1つずつ記述する（多数の場合）
- 共通スタイルはリソースディクショナリに定義する
- バインディングはxamlで行い、コードビハインドでの直接的なUI操作は避ける
- 日本語リソースは全てリソースファイルに定義する

```xml
<Button 
    x:Name="startButton"
    Content="{Binding StartButtonText}" 
    Command="{Binding StartStopRecordingCommand}"
    Margin="10"
    Width="120"
    Height="30" />
```

### デザインパターン
- UserControlを使用して再利用可能なUI要素を作成する
- DataTemplateを使用してデータの表示方法を定義する
- コンバーターを使用して表示フォーマットを調整する
- ListViewの利用時はカスタムItemTemplateを定義する

### テーマとスタイル
- メインカラー: #3498db (青)
- セカンダリカラー: #2c3e50 (紺)
- アクセントカラー: #e74c3c (赤)
- フォント: メイリオ (UI)、Consolas (ログ)

## 12. コード品質管理

### 静的解析
- Microsoft.CodeAnalysis.NetAnalyzers を使用する
- 警告は解決するか、明示的に抑制理由をコメントで記述する
- コードスタイルの一貫性を保つためにEditorConfigを使用する
- 以下の警告は無視しない:
  - CS0618: 廃止予定のAPI使用
  - CS8602: null参照の可能性
  - CS8604: nullの可能性がある値の引数渡し

### コードレビュー
- すべての変更はレビューを受ける
- レビューでは機能性、セキュリティ、パフォーマンス、テスト可能性を確認する
- コミットメッセージは関連するチケット番号から始める（例: `#123: 音声認識機能の実装`）

## 13. プロジェクト固有のルール

### NAudio 関連
- WasapiLoopbackCaptureを使用してデスクトップ音声をキャプチャする
- 音声フォーマットは16kHz, 16bit, モノラルを基本とする
- オーディオデバイスのリストは定期的に更新する

### Azure Speech Service 関連
- 日本語認識を基本とする (`ja-JP`)
- 認識サービスの初期化は非同期で行う
- 認識キャンセル時は適切なイベント通知を行う
- 各認識結果にはタイムスタンプを付与する

### データ保存
- テキスト保存は UTF-8 エンコーディングを使用する
- 音声ファイルは WAV 形式で保存する
- ファイル名には日時を含める形式とする
- 自動保存の有効/無効を切り替え可能にする

### UI応答性
- 音声認識中はプログレスインジケータを表示する
- 処理中のUIのフリーズを防ぐため、重い処理は別スレッドで実行する
- タイムスタンプは見やすいように色付け表示する

## 14. GitHub Copilot 向け指示

### コード生成モデル
- NAudioを使用した音声キャプチャコードのパターン:
  ```csharp
  private void StartCapture()
  {
      _captureInstance = new WasapiLoopbackCapture();
      _captureInstance.DataAvailable += CaptureInstance_DataAvailable;
      _captureInstance.RecordingStopped += CaptureInstance_RecordingStopped;
      _captureInstance.StartRecording();
  }
  ```

- Azure Speech Service初期化パターン:
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

### 回避すべき反パターン
- APIキーのハードコード
- UIスレッドでの長時間処理
- 非同期操作の同期的な待機
- リソースの非解放
- 例外の黙殺

### 汎用的テンプレート
- Result<T>パターン:
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
