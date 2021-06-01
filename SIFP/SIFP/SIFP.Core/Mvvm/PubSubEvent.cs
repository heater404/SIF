using Prism.Events;
using SIFP.Core.Enums;
using SIFP.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace SIFP.Core.Mvvm
{
    public class WatchLogEvent : PubSubEvent<LogModel> { }

    public class NoticeLogEvent : PubSubEvent<LogModel> { }

    public class ConnectCameraReplyEvent : PubSubEvent<ConnectCameraReply> { }

    public class ConfigCameraReplyEvent : PubSubEvent<ConfigCameraReply> { }

    public class StopStreamingReplyEvent : PubSubEvent<StopStreamingReply> { };

    public class ConfigAlgRequestEvent : PubSubEvent { };
    public class ConfigAlgReplyEvent : PubSubEvent<ConfigAlgReply> { };

    public class CaptureReplyEvent : PubSubEvent<CaptureReply> { };

    public class ConfigWorkModeSuceessEvent : PubSubEvent<SubWorkModeE> { };

    public class ConfigCameraRequestEvent : PubSubEvent { };

    public class ConfigArithParamsRequestEvent : PubSubEvent { };

    public class DisconnectCameraRequestEvent : PubSubEvent { };

    public class DisconnectCameraReplyEvent : PubSubEvent<DisconnectCameraReply> { };

    public class ReadRegisterReplyEvent : PubSubEvent<ReadRegisterReply> { };

    public class GetSysStatusReplyEvent : PubSubEvent<GetSysStatusReply> { };

    public class CloseWaitingDialogEvent : PubSubEvent { }

    public class OpenPointCloudEvent : PubSubEvent<string> { }

    public class ClosePointCloudEvent : PubSubEvent { }

    public class ChangeDrawerRegionSizeEvent : PubSubEvent<Size> { };

    public class IsDebugEvent : PubSubEvent<bool> { };

    public class IsStreamingEvent : PubSubEvent<bool> { };

    public class ConfigCorrectionAEChangedEvent : PubSubEvent<bool> { };

    public class ConfigCameraAEChangedEvent : PubSubEvent<bool> { };

    public class ConfigArithParamsReplyEvent : PubSubEvent<ConfigArithParamsReply> { };

    public class ConfigVcselDriverReplyEvent : PubSubEvent<ConfigVcselDriverReply> { };

    public class UserAccessChangedEvent : PubSubEvent<UserAccessType> { };

    public class ConfigCameraSuccessEvent : PubSubEvent<bool> { };

    public class MainWindowEnableEvent : PubSubEvent<bool> { };
    public class LensArgsReplyEvent : PubSubEvent<LensArgsReply> { };
}
