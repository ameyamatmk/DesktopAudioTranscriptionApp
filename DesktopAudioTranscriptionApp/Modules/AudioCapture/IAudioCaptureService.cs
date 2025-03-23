using NAudio.Wave;

namespace DesktopAudioTranscriptionApp.Modules.AudioCapture
{
    /// <summary>
    /// 音声キャプチャのための抽象インターフェース
    /// </summary>
    public interface IAudioCaptureService
    {
        /// <summary>
        /// 音声データが利用可能になったときに発生するイベント
        /// </summary>
        event EventHandler<AudioDataAvailableEventArgs> DataAvailable;

        /// <summary>
        /// 音声キャプチャが停止したときに発生するイベント
        /// </summary>
        event EventHandler<AudioCaptureStoppedEventArgs> CaptureStopped;

        /// <summary>
        /// 音声キャプチャを開始します
        /// </summary>
        void StartCapture();

        /// <summary>
        /// 音声キャプチャを停止します
        /// </summary>
        void StopCapture();

        /// <summary>
        /// 現在の音声フォーマットを取得します
        /// </summary>
        WaveFormat CurrentFormat { get; }

        /// <summary>
        /// 利用可能なオーディオデバイスのリストを取得します
        /// </summary>
        /// <returns>利用可能なオーディオデバイスのリスト</returns>
        List<AudioDevice> GetAvailableDevices();

        /// <summary>
        /// 選択されたオーディオデバイス
        /// </summary>
        AudioDevice SelectedDevice { get; set; }
    }

    /// <summary>
    /// 音声データが利用可能になったときのイベント引数
    /// </summary>
    public class AudioDataAvailableEventArgs : EventArgs
    {
        /// <summary>
        /// 音声データのバッファ
        /// </summary>
        public byte[] Buffer { get; set; }

        /// <summary>
        /// 記録されたバイト数
        /// </summary>
        public int BytesRecorded { get; set; }
    }

    /// <summary>
    /// 音声キャプチャが停止したときのイベント引数
    /// </summary>
    public class AudioCaptureStoppedEventArgs : EventArgs
    {
        /// <summary>
        /// 発生した例外（存在する場合）
        /// </summary>
        public Exception Exception { get; set; }
    }

    /// <summary>
    /// オーディオデバイスの情報
    /// </summary>
    public class AudioDevice
    {
        /// <summary>
        /// デバイスのID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// デバイスの名前
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// デフォルトデバイスかどうか
        /// </summary>
        public bool IsDefault { get; set; }
    }
}
