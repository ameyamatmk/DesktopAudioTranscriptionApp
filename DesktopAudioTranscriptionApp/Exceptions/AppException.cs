using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopAudioTranscriptionApp.Exceptions.Core;

/// <summary>
/// アプリケーション全体の基底例外クラス
/// </summary>
public abstract class AppException : Exception
{
    public AppException(string message) : base(message) { }
    public AppException(string message, Exception innerException) : base(message, innerException) { }
}