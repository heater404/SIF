using Prism.Events;
using SIFP.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIFP.Core.Mvvm
{
    public class WatchLogEvent : PubSubEvent<WatchLog> { }

    public class ConnectCameraReplyEvent : PubSubEvent<ConnectCameraReply> { }

    public class ConfigCameraReplyEvent : PubSubEvent<ConfigCameraReply> { }
}
