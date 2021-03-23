﻿using Prism.Events;
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

    public class ConfigAlgReplyEvent : PubSubEvent<ConfigAlgReply> { };

    public class CaptureReplyEvent : PubSubEvent<CaptureReply> { };

    public class ConfigWorkModeSuceessEvent : PubSubEvent<SubWorkModeE> { };

    public class ConfigCameraRequestEvent : PubSubEvent { };

    public class DisconnectCameraRequestEvent : PubSubEvent { };

    public class CloseWaitingDialogEvent : PubSubEvent { }

    public class OpenPointCloudEvent : PubSubEvent<string> { }

    public class ClosePointCloudEvent : PubSubEvent { }

    public class ChangeLeftDrawerRegionSizeEvent : PubSubEvent<Size> { };
}
