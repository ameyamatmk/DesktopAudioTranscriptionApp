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
