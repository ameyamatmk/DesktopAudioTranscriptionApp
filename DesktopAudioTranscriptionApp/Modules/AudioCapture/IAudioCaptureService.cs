using NAudio.Wave;

namespace DesktopAudioTranscriptionApp.Modules.AudioCapture
{
    /// <summary>
    /// �����L���v�`���̂��߂̒��ۃC���^�[�t�F�[�X
    /// </summary>
    public interface IAudioCaptureService
    {
        /// <summary>
        /// �����f�[�^�����p�\�ɂȂ����Ƃ��ɔ�������C�x���g
        /// </summary>
        event EventHandler<AudioDataAvailableEventArgs> DataAvailable;

        /// <summary>
        /// �����L���v�`������~�����Ƃ��ɔ�������C�x���g
        /// </summary>
        event EventHandler<AudioCaptureStoppedEventArgs> CaptureStopped;

        /// <summary>
        /// �����L���v�`�����J�n���܂�
        /// </summary>
        void StartCapture();

        /// <summary>
        /// �����L���v�`�����~���܂�
        /// </summary>
        void StopCapture();

        /// <summary>
        /// ���݂̉����t�H�[�}�b�g���擾���܂�
        /// </summary>
        WaveFormat CurrentFormat { get; }

        /// <summary>
        /// ���p�\�ȃI�[�f�B�I�f�o�C�X�̃��X�g���擾���܂�
        /// </summary>
        /// <returns>���p�\�ȃI�[�f�B�I�f�o�C�X�̃��X�g</returns>
        List<AudioDevice> GetAvailableDevices();

        /// <summary>
        /// �I�����ꂽ�I�[�f�B�I�f�o�C�X
        /// </summary>
        AudioDevice SelectedDevice { get; set; }
    }

    /// <summary>
    /// �����f�[�^�����p�\�ɂȂ����Ƃ��̃C�x���g����
    /// </summary>
    public class AudioDataAvailableEventArgs : EventArgs
    {
        /// <summary>
        /// �����f�[�^�̃o�b�t�@
        /// </summary>
        public byte[] Buffer { get; set; }

        /// <summary>
        /// �L�^���ꂽ�o�C�g��
        /// </summary>
        public int BytesRecorded { get; set; }
    }

    /// <summary>
    /// �����L���v�`������~�����Ƃ��̃C�x���g����
    /// </summary>
    public class AudioCaptureStoppedEventArgs : EventArgs
    {
        /// <summary>
        /// ����������O�i���݂���ꍇ�j
        /// </summary>
        public Exception Exception { get; set; }
    }

    /// <summary>
    /// �I�[�f�B�I�f�o�C�X�̏��
    /// </summary>
    public class AudioDevice
    {
        /// <summary>
        /// �f�o�C�X��ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// �f�o�C�X�̖��O
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// �f�t�H���g�f�o�C�X���ǂ���
        /// </summary>
        public bool IsDefault { get; set; }
    }
}
