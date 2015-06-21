﻿using PubnubWindowsStore.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using PubNubMessaging.Core;
using Windows.UI.Core;
using Windows.UI;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace PubnubWindowsStore
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class PubnubOperation : Page
    {
        string channel = "";
        string channelGroup = "";
        PubnubConfigData data = null;
        static Pubnub pubnub = null;

        Popup publishPopup = null;
        Popup hereNowPopup = null;
        Popup whereNowPopup = null;
        Popup globalHereNowPopup = null;
        Popup userStatePopup = null;
        Popup changeUUIDPopup = null;

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }


        public PubnubOperation()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
        }

        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);

            data = e.Parameter as PubnubConfigData;

            if (data != null)
            {
                pubnub = new Pubnub(data.publishKey, data.subscribeKey, data.secretKey, data.cipherKey, data.ssl);
                pubnub.Origin = data.origin;
                pubnub.SessionUUID = data.sessionUUID;
                pubnub.SubscribeTimeout = data.subscribeTimeout;
                pubnub.NonSubscribeTimeout = data.nonSubscribeTimeout;
                pubnub.NetworkCheckMaxRetries = data.maxRetries;
                pubnub.NetworkCheckRetryInterval = data.retryInterval;
                pubnub.LocalClientHeartbeatInterval = data.localClientHeartbeatInterval;
                pubnub.EnableResumeOnReconnect = data.resumeOnReconnect;
                pubnub.AuthenticationKey = data.authKey;
                pubnub.PresenceHeartbeat = data.presenceHeartbeat;
                pubnub.PresenceHeartbeatInterval = data.presenceHeartbeatInterval;
            }
            
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void btnTime_Click(object sender, RoutedEventArgs e)
        {
            DisplayMessageInTextBox("Running Time:");
            pubnub.Time<string>(PubnubCallbackResult, PubnubDisplayErrorMessage);
        }

        /// <summary>
        /// Callback method captures the response in JSON string format for all operations
        /// </summary>
        /// <param name="result"></param>
        void PubnubCallbackResult(string result)
        {
            DisplayMessageInTextBox(result);
            //Console.WriteLine("REGULAR CALLBACK:");
            //Console.WriteLine(result);
            //Console.WriteLine();
        }

        /// <summary>
        /// Callback method for error messages
        /// </summary>
        /// <param name="result"></param>
        void PubnubDisplayErrorMessage(PubnubClientError result)
        {
            DisplayMessageInTextBox(result.Description);

            switch (result.StatusCode)
            {
                case 103:
                    //Warning: Verify origin host name and internet connectivity
                    break;
                case 104:
                    //Critical: Verify your cipher key
                    break;
                case 106:
                    //Warning: Check network/internet connection
                    break;
                case 108:
                    //Warning: Check network/internet connection
                    break;
                case 109:
                    //Warning: No network/internet connection. Please check network/internet connection
                    break;
                case 110:
                    //Informational: Network/internet connection is back. Active subscriber/presence channels will be restored.
                    break;
                case 111:
                    //Informational: Duplicate channel subscription is not allowed. Internally Pubnub API removes the duplicates before processing.
                    break;
                case 112:
                    //Informational: Channel Already Subscribed/Presence Subscribed. Duplicate channel subscription not allowed
                    break;
                case 113:
                    //Informational: Channel Already Presence-Subscribed. Duplicate channel presence-subscription not allowed
                    break;
                case 114:
                    //Warning: Please verify your cipher key
                    break;
                case 115:
                    //Warning: Protocol Error. Please contact PubNub with error details.
                    break;
                case 116:
                    //Warning: ServerProtocolViolation. Please contact PubNub with error details.
                    break;
                case 117:
                    //Informational: Input contains invalid channel name
                    break;
                case 118:
                    //Informational: Channel not subscribed yet
                    break;
                case 119:
                    //Informational: Channel not subscribed for presence yet
                    break;
                case 120:
                    //Informational: Incomplete unsubscribe. Try again for unsubscribe.
                    break;
                case 121:
                    //Informational: Incomplete presence-unsubscribe. Try again for presence-unsubscribe.
                    break;
                case 122:
                    //Informational: Network/Internet connection not available. C# client retrying again to verify connection. No action is needed from your side.
                    break;
                case 123:
                    //Informational: During non-availability of network/internet, max retries for connection were attempted. So unsubscribed the channel.
                    break;
                case 124:
                    //Informational: During non-availability of network/internet, max retries for connection were attempted. So presence-unsubscribed the channel.
                    break;
                case 125:
                    //Informational: Publish operation timeout occured.
                    break;
                case 126:
                    //Informational: HereNow operation timeout occured
                    break;
                case 127:
                    //Informational: Detailed History operation timeout occured
                    break;
                case 128:
                    //Informational: Time operation timeout occured
                    break;
                case 4000:
                    //Warning: Message too large. Your message was not sent. Try to send this again smaller sized
                    break;
                case 4001:
                    //Warning: Bad Request. Please check the entered inputs or web request URL
                    break;
                case 4002:
                    //Warning: Invalid Key. Please verify the publish key
                    break;
                case 4010:
                    //Critical: Please provide correct subscribe key. This corresponds to a 401 on the server due to a bad sub key
                    break;
                case 4020:
                    // PAM is not enabled. Please contact PubNub support
                    break;
                case 4030:
                    //Warning: Not authorized. Check the permimissions on the channel. Also verify authentication key, to check access.
                    break;
                case 4031:
                    //Warning: Incorrect public key or secret key.
                    break;
                case 4140:
                    //Warning: Length of the URL is too long. Reduce the length by reducing subscription/presence channels or grant/revoke/audit channels/auth key list
                    break;
                case 5000:
                    //Critical: Internal Server Error. Unexpected error occured at PubNub Server. Please try again. If same problem persists, please contact PubNub support
                    break;
                case 5020:
                    //Critical: Bad Gateway. Unexpected error occured at PubNub Server. Please try again. If same problem persists, please contact PubNub support
                    break;
                case 5040:
                    //Critical: Gateway Timeout. No response from server due to PubNub server timeout. Please try again. If same problem persists, please contact PubNub support
                    break;
                case 0:
                    //Undocumented error. Please contact PubNub support with full error object details for further investigation
                    break;
                default:
                    break;
            }
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            var frame = new Frame();
            frame.Navigate(typeof(PubnubDemoStart));
            Window.Current.Content = frame;
        }

        private void btnPresence_Click(object sender, RoutedEventArgs e)
        {
            channel = txtChannel.Text;
            channelGroup = txtChannelGroup.Text;    
            DisplayMessageInTextBox("Running Presence:");
            if (channelGroup != "")
            {
                pubnub.Presence<string>(channel, channelGroup, PubnubCallbackResult, PubnubConnectCallbackResult, PubnubDisplayErrorMessage);
            }
            else
            {
                pubnub.Presence<string>(channel, PubnubCallbackResult, PubnubConnectCallbackResult, PubnubDisplayErrorMessage);
            }
        }

        private void btnSubscribe_Click(object sender, RoutedEventArgs e)
        {
            channel = txtChannel.Text;
            channelGroup = txtChannelGroup.Text;    
            DisplayMessageInTextBox("Running Subscribe:");
            if (channelGroup != "")
            {
                pubnub.Subscribe<string>(channel, channelGroup, PubnubCallbackResult, PubnubConnectCallbackResult, PubnubDisplayErrorMessage);
            }
            else
            {
                pubnub.Subscribe<string>(channel, PubnubCallbackResult, PubnubConnectCallbackResult, PubnubDisplayErrorMessage);
            }
        }

        private void btnUnsubscribe_Click(object sender, RoutedEventArgs e)
        {
            channel = txtChannel.Text;
            channelGroup = txtChannelGroup.Text;    
            DisplayMessageInTextBox("Running Unsubscribe:");
            if (channelGroup != "")
            {
                pubnub.Unsubscribe<string>(channel, channelGroup, PubnubCallbackResult, PubnubConnectCallbackResult, PubnubDisconnectCallbackResult, PubnubDisplayErrorMessage);
            }
            else
            {
                pubnub.Unsubscribe<string>(channel, PubnubCallbackResult, PubnubConnectCallbackResult, PubnubDisconnectCallbackResult, PubnubDisplayErrorMessage);
            }
        }

        private void btnPresenceUnsub_Click(object sender, RoutedEventArgs e)
        {
            channel = txtChannel.Text;
            channelGroup = txtChannelGroup.Text;    
            DisplayMessageInTextBox("Running Presence-Unsubscribe:");
            if (channelGroup != "")
            {
                pubnub.PresenceUnsubscribe<string>(channel, channelGroup, PubnubCallbackResult, PubnubConnectCallbackResult, PubnubDisconnectCallbackResult, PubnubDisplayErrorMessage);
            }
            else
            {
                pubnub.PresenceUnsubscribe<string>(channel, PubnubCallbackResult, PubnubConnectCallbackResult, PubnubDisconnectCallbackResult, PubnubDisplayErrorMessage);
            }
        }

        void PubnubConnectCallbackResult(string result)
        {
            DisplayMessageInTextBox("Connect Callback:");
            DisplayMessageInTextBox(result);
        }

        private void PubnubDisconnectCallbackResult(string result)
        {
            DisplayMessageInTextBox("Disconnect Callback:");
            DisplayMessageInTextBox(result);
        }


        private void btnPublish_Click(object sender, RoutedEventArgs e)
        {
            channel = txtChannel.Text;
            this.IsEnabled = false;
            Border border = new Border();
            border.BorderBrush = new SolidColorBrush(Colors.Black);
            border.BorderThickness = new Thickness(5.0);

            StackPanel publishStackPanel = new StackPanel();
            publishStackPanel.Background = new SolidColorBrush(Colors.Blue);
            publishStackPanel.Width = 300;
            publishStackPanel.Height = 400;

            publishPopup = new Popup();
            publishPopup.Height = 400;
            publishPopup.Width = 300;
            publishPopup.VerticalOffset = 100;
            publishPopup.HorizontalOffset = 100;
            
            var point = CalcOffsets((UIElement)sender);
            publishPopup.HorizontalOffset = point.X + 50;
            publishPopup.VerticalOffset = point.Y - 100;

            PublishMessageUserControl control = new PublishMessageUserControl();
            publishStackPanel.Children.Add(control);
            border.Child = publishStackPanel;

            publishPopup.Child = border;
            publishPopup.IsOpen = true;

            publishPopup.Closed += (senderPopup, argsPopup) =>
            {
                if (control.IsOKButtonEntered)
                {
                    RadioButton radNormalPublish = control.FindName("radNormalPublish") as RadioButton;
                    if (radNormalPublish != null && radNormalPublish.IsChecked.Value)
                    {
                        TextBox txtPublish = control.FindName("txtPublish") as TextBox;
                        string publishMsg = (txtPublish != null) ? txtPublish.Text : "";

                        CheckBox chkStoreInHistory = control.FindName("chkStoreInHistory") as CheckBox;
                        bool storeInHistory = (chkStoreInHistory != null) ? chkStoreInHistory.IsChecked.Value : true;

                        if (publishMsg != "")
                        {
                            DisplayMessageInTextBox("Running Publish:");

                            double doubleData;
                            int intData;
                            if (int.TryParse(publishMsg, out intData)) //capture numeric data
                            {
                                pubnub.Publish<string>(channel, intData, storeInHistory, PubnubCallbackResult, PubnubDisplayErrorMessage);
                            }
                            else if (double.TryParse(publishMsg, out doubleData)) //capture numeric data
                            {
                                pubnub.Publish<string>(channel, doubleData, storeInHistory, PubnubCallbackResult, PubnubDisplayErrorMessage);
                            }
                            else
                            {
                                pubnub.Publish<string>(channel, publishMsg, storeInHistory, PubnubCallbackResult, PubnubDisplayErrorMessage);
                            }
                        }
                    }

                    RadioButton radToastPublish = control.FindName("radToastPublish") as RadioButton;
                    if (radToastPublish != null && radToastPublish.IsChecked.Value)
                    {
                        MpnsToastNotification toast = new MpnsToastNotification();
                        toast.text1 = "hardcode message";
                        Dictionary<string, object> dicToast = new Dictionary<string, object>();
                        dicToast.Add("pn_mpns", toast);

                        pubnub.EnableDebugForPushPublish = true;
                        pubnub.Publish<string>(channel, dicToast, PubnubCallbackResult, PubnubDisplayErrorMessage);

                    }

                    RadioButton radFlipTilePublish = control.FindName("radFlipTilePublish") as RadioButton;
                    if (radFlipTilePublish != null && radFlipTilePublish.IsChecked.Value)
                    {
                        pubnub.PushRemoteImageDomainUri.Add(new Uri("http://cdn.flaticon.com"));

                        MpnsFlipTileNotification tile = new MpnsFlipTileNotification();
                        tile.title = "front title";
                        tile.count = 6;
                        tile.back_title = "back title";
                        tile.back_content = "back message";
                        tile.back_background_image = "Assets/Tiles/pubnub3.png";
                        tile.background_image = "http://cdn.flaticon.com/png/256/37985.png";
                        Dictionary<string, object> dicTile = new Dictionary<string, object>();
                        dicTile.Add("pn_mpns", tile);

                        pubnub.EnableDebugForPushPublish = true;
                        pubnub.Publish<string>(channel, dicTile, PubnubCallbackResult, PubnubDisplayErrorMessage);
                    }

                    RadioButton radCycleTilePublish = control.FindName("radCycleTilePublish") as RadioButton;
                    if (radCycleTilePublish != null && radCycleTilePublish.IsChecked.Value)
                    {
                        MpnsCycleTileNotification tile = new MpnsCycleTileNotification();
                        tile.title = "front title";
                        tile.count = 2;
                        tile.images = new string[] { "Assets/Tiles/pubnub1.png", "Assets/Tiles/pubnub2.png", "Assets/Tiles/pubnub3.png", "Assets/Tiles/pubnub4.png" };

                        Dictionary<string, object> dicTile = new Dictionary<string, object>();
                        dicTile.Add("pn_mpns", tile);

                        pubnub.EnableDebugForPushPublish = true;
                        pubnub.Publish<string>(channel, dicTile, PubnubCallbackResult, PubnubDisplayErrorMessage);
                    }

                    RadioButton radIconicTilePublish = control.FindName("radIconicTilePublish") as RadioButton;
                    if (radIconicTilePublish != null && radIconicTilePublish.IsChecked.Value)
                    {
                        MpnsIconicTileNotification tile = new MpnsIconicTileNotification();
                        tile.title = "front title";
                        tile.count = 2;
                        tile.wide_content_1 = "my wide content";

                        Dictionary<string, object> dicTile = new Dictionary<string, object>();
                        dicTile.Add("pn_mpns", tile);

                        pubnub.EnableDebugForPushPublish = true;
                        pubnub.Publish<string>(channel, dicTile, PubnubCallbackResult, PubnubDisplayErrorMessage);
                    }

                }
                publishPopup = null;
                this.IsEnabled = true;
            };

        }

        private void btnHistory_Click(object sender, RoutedEventArgs e)
        {
            channel = txtChannel.Text;
            DisplayMessageInTextBox("Running Detailed History:");
            pubnub.DetailedHistory<string>(channel, 100, PubnubCallbackResult, PubnubDisplayErrorMessage);
        }

        private void btnGlobalHereNow_Click(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            Border border = new Border();
            border.BorderBrush = new SolidColorBrush(Colors.Black);
            border.BorderThickness = new Thickness(5.0);

            StackPanel globalHerenowStackPanel = new StackPanel();
            globalHerenowStackPanel.Background = new SolidColorBrush(Colors.Blue);
            globalHerenowStackPanel.Width = 300;
            globalHerenowStackPanel.Height = 300;

            globalHereNowPopup = new Popup();
            globalHereNowPopup.Height = 300;
            globalHereNowPopup.Width = 300;

            var point = CalcOffsets((UIElement)sender);
            globalHereNowPopup.HorizontalOffset = point.X + 50;
            globalHereNowPopup.VerticalOffset = point.Y - 70;

            HereNowOptionsUserControl control = new HereNowOptionsUserControl();
            globalHerenowStackPanel.Children.Add(control);
            border.Child = globalHerenowStackPanel;

            globalHereNowPopup.Child = border;
            globalHereNowPopup.IsOpen = true;

            globalHereNowPopup.Closed += (senderPopup, argsPopup) =>
            {
                if (control.IsOKButtonEntered)
                {
                    CheckBox chkShowUUID = control.FindName("chkHereNowShowUUID") as CheckBox;
                    bool showUUID = (chkShowUUID != null) ? chkShowUUID.IsChecked.Value : false;

                    CheckBox chkIncludeUserState = control.FindName("chkHereIncludeUserState") as CheckBox;
                    bool includeState = (chkIncludeUserState != null) ? chkIncludeUserState.IsChecked.Value : false;

                    DisplayMessageInTextBox("Running GlobalHereNow:");
                    pubnub.GlobalHereNow<string>(showUUID, includeState, PubnubCallbackResult, PubnubDisplayErrorMessage);
                }
                globalHereNowPopup = null;
                this.IsEnabled = true;
            };
            
        }

        private void btnHereNow_Click(object sender, RoutedEventArgs e)
        {
            channel = txtChannel.Text;
            this.IsEnabled = false;
            Border border = new Border();
            border.BorderBrush = new SolidColorBrush(Colors.Black);
            border.BorderThickness = new Thickness(5.0);

            StackPanel herenowStackPanel = new StackPanel();
            herenowStackPanel.Background = new SolidColorBrush(Colors.Blue);
            herenowStackPanel.Width = 300;
            herenowStackPanel.Height = 300;

            hereNowPopup = new Popup();
            hereNowPopup.Height = 300;
            hereNowPopup.Width = 300;

            var point = CalcOffsets((UIElement)sender);
            hereNowPopup.HorizontalOffset = point.X + 50;
            hereNowPopup.VerticalOffset = point.Y - 70;


            HereNowOptionsUserControl control = new HereNowOptionsUserControl();
            herenowStackPanel.Children.Add(control);
            border.Child = herenowStackPanel;

            hereNowPopup.Child = border;
            hereNowPopup.IsOpen = true;

            hereNowPopup.Closed += (senderPopup, argsPopup) =>
                {
                    if (control.IsOKButtonEntered)
                    {
                        CheckBox chkShowUUID = control.FindName("chkHereNowShowUUID") as CheckBox;
                        bool showUUID = (chkShowUUID != null) ? chkShowUUID.IsChecked.Value : false;

                        CheckBox chkIncludeUserState = control.FindName("chkHereIncludeUserState") as CheckBox;
                        bool includeState = (chkIncludeUserState != null) ? chkIncludeUserState.IsChecked.Value : false;

                        DisplayMessageInTextBox("Running HereNow:");
                        pubnub.HereNow<string>(channel, showUUID, includeState, PubnubCallbackResult, PubnubDisplayErrorMessage);
                    }
                    hereNowPopup = null;
                    this.IsEnabled = true;
                };
        }

        private void btnPAMChannel_Click(object sender, RoutedEventArgs e)
        {
            channel = txtChannel.Text;

            this.IsEnabled = false;
            Border border = new Border();
            border.BorderBrush = new SolidColorBrush(Colors.Black);
            border.BorderThickness = new Thickness(5.0);

            StackPanel pamChannelStackPanel = new StackPanel();
            pamChannelStackPanel.Background = new SolidColorBrush(Colors.Blue);
            pamChannelStackPanel.Width = 400;
            pamChannelStackPanel.Height = 400;

            Popup pamChannelPopup = new Popup();
            pamChannelPopup.Height = 300;
            pamChannelPopup.Width = 300;

            pamChannelPopup.HorizontalOffset = 10;
            pamChannelPopup.VerticalOffset = 100;

            PAMChannelUserControl control = new PAMChannelUserControl();

            TextBox txtPAMChannelName = control.FindName("txtChannelName") as TextBox;
            if (txtPAMChannelName != null)
            {
                txtPAMChannelName.Text = channel;
            }

            pamChannelStackPanel.Children.Add(control);
            border.Child = pamChannelStackPanel;

            pamChannelPopup.Child = border;
            pamChannelPopup.IsOpen = true;

            pamChannelPopup.Closed += (senderPopup, argsPopup) =>
            {
                if (control.IsOKButtonEntered)
                {
                    string pamUserChannelName = "";
                    string pamAuthKey = "";
                    txtPAMChannelName = control.FindName("txtChannelName") as TextBox;
                    if (txtPAMChannelName != null)
                    {
                        pamUserChannelName = txtPAMChannelName.Text.Trim();
                    }

                    TextBox txtAuthKey = control.FindName("txtAuthKey") as TextBox;
                    if (txtAuthKey != null)
                    {
                        pamAuthKey = txtAuthKey.Text;
                    }

                    RadioButton radGrantPAMChannel = control.FindName("radGrantChannel") as RadioButton;
                    if (radGrantPAMChannel != null && radGrantPAMChannel.IsChecked.Value)
                    {
                        DisplayMessageInTextBox("Running GrantAccess:");
                        int ttlInMinutes = 1440;
                        pubnub.GrantAccess<string>(pamUserChannelName, pamAuthKey, true, true, ttlInMinutes, PubnubCallbackResult, PubnubDisplayErrorMessage);
                    }

                    RadioButton radAuditChannel = control.FindName("radAuditChannel") as RadioButton;
                    if (radAuditChannel != null && radAuditChannel.IsChecked.Value)
                    {
                        DisplayMessageInTextBox("Running AuditAccess:");
                        pubnub.AuditAccess<string>(pamUserChannelName, pamAuthKey, PubnubCallbackResult, PubnubDisplayErrorMessage);
                    }

                    RadioButton radRevokeChannel = control.FindName("radRevokeChannel") as RadioButton;
                    if (radRevokeChannel != null && radRevokeChannel.IsChecked.Value)
                    {
                        DisplayMessageInTextBox("Running Revoke Access:");
                        pubnub.GrantAccess<string>(pamUserChannelName, pamAuthKey, false, false, PubnubCallbackResult, PubnubDisplayErrorMessage);
                    }

                    RadioButton radGrantPresenceChannel = control.FindName("radGrantPresenceChannel") as RadioButton;
                    if (radGrantPresenceChannel != null && radGrantPresenceChannel.IsChecked.Value)
                    {
                        DisplayMessageInTextBox("Running GrantPresenceAccess:");
                        int ttlInMinutes = 1440;
                        pubnub.GrantPresenceAccess<string>(pamUserChannelName, pamAuthKey, true, true, ttlInMinutes, PubnubCallbackResult, PubnubDisplayErrorMessage);
                    }

                    RadioButton radAuditPresenceChannel = control.FindName("radAuditPresenceChannel") as RadioButton;
                    if (radAuditPresenceChannel != null && radAuditPresenceChannel.IsChecked.Value)
                    {
                        DisplayMessageInTextBox("Running AuditPresenceAccess:");
                        pubnub.AuditPresenceAccess<string>(pamUserChannelName, pamAuthKey, PubnubCallbackResult, PubnubDisplayErrorMessage);
                    }

                    RadioButton radRevokePresenceChannel = control.FindName("radRevokePresenceChannel") as RadioButton;
                    if (radRevokePresenceChannel != null && radRevokePresenceChannel.IsChecked.Value)
                    {
                        DisplayMessageInTextBox("Running Presence Revoke Access:");
                        pubnub.GrantPresenceAccess<string>(pamUserChannelName, pamAuthKey, false, false, PubnubCallbackResult, PubnubDisplayErrorMessage);
                    }

                }
                pamChannelPopup = null;
                this.IsEnabled = true;
            };

        }

        private void btnPAMChannelGroup_Click(object sender, RoutedEventArgs e)
        {
            channelGroup = txtChannelGroup.Text;

            this.IsEnabled = false;
            Border border = new Border();
            border.BorderBrush = new SolidColorBrush(Colors.Black);
            border.BorderThickness = new Thickness(5.0);

            StackPanel pamChannelGroupStackPanel = new StackPanel();
            pamChannelGroupStackPanel.Background = new SolidColorBrush(Colors.Blue);
            pamChannelGroupStackPanel.Width = 400;
            pamChannelGroupStackPanel.Height = 400;

            Popup pamChannelGroupPopup = new Popup();
            pamChannelGroupPopup.Height = 300;
            pamChannelGroupPopup.Width = 300;

            pamChannelGroupPopup.HorizontalOffset = 10;
            pamChannelGroupPopup.VerticalOffset = 100;

            PAMChannelGroupUserControl control = new PAMChannelGroupUserControl();

            TextBox txtPAMChannelGroup = control.FindName("txtChannelGroup") as TextBox;
            if (txtPAMChannelGroup != null)
            {
                txtPAMChannelGroup.Text = channelGroup;
            }

            pamChannelGroupStackPanel.Children.Add(control);
            border.Child = pamChannelGroupStackPanel;

            pamChannelGroupPopup.Child = border;
            pamChannelGroupPopup.IsOpen = true;

            pamChannelGroupPopup.Closed += (senderPopup, argsPopup) =>
            {
                if (control.IsOKButtonEntered)
                {
                    string pamUserChannelGroup = "";
                    string pamAuthKey = "";
                    int ttlInMinutes = 1440;
                    txtPAMChannelGroup = control.FindName("txtChannelGroup") as TextBox;
                    if (txtPAMChannelGroup != null)
                    {
                        pamUserChannelGroup = txtPAMChannelGroup.Text;

                        TextBox txtAuthKey = control.FindName("txtAuthKey") as TextBox;
                        if (txtAuthKey != null)
                        {
                            pamAuthKey = txtAuthKey.Text;
                        }

                        RadioButton radGrantPAMChannelGroup = control.FindName("radGrantChannelGroup") as RadioButton;
                        if (radGrantPAMChannelGroup != null && radGrantPAMChannelGroup.IsChecked.Value)
                        {
                            DisplayMessageInTextBox("Running ChannelGroupGrantAccess:");
                            pubnub.ChannelGroupGrantAccess<string>(pamUserChannelGroup, pamAuthKey, true, true, ttlInMinutes, PubnubCallbackResult, PubnubDisplayErrorMessage);
                        }

                        RadioButton radAuditPAMChannelGroup = control.FindName("radAuditChannelGroup") as RadioButton;
                        if (radAuditPAMChannelGroup != null && radAuditPAMChannelGroup.IsChecked.Value)
                        {
                            DisplayMessageInTextBox("Running ChannelGroupAuditAccess:");
                            pubnub.ChannelGroupAuditAccess<string>(pamUserChannelGroup, pamAuthKey, PubnubCallbackResult, PubnubDisplayErrorMessage);
                        }

                        RadioButton radRevokePAMChannelGroup = control.FindName("radRevokeChannelGroup") as RadioButton;
                        if (radRevokePAMChannelGroup != null && radRevokePAMChannelGroup.IsChecked.Value)
                        {
                            DisplayMessageInTextBox("Running ChannelGroup Revoke Access:");
                            pubnub.ChannelGroupGrantAccess<string>(pamUserChannelGroup, pamAuthKey, false, false, PubnubCallbackResult, PubnubDisplayErrorMessage);
                        }

                        RadioButton radGrantPAMPresenceChannelGroup = control.FindName("radGrantPresenceChannelGroup") as RadioButton;
                        if (radGrantPAMPresenceChannelGroup != null && radGrantPAMPresenceChannelGroup.IsChecked.Value)
                        {
                            DisplayMessageInTextBox("Running ChannelGroupGrantPresenceAccess:");
                            pubnub.ChannelGroupGrantPresenceAccess<string>(pamUserChannelGroup, pamAuthKey, true, true, ttlInMinutes, PubnubCallbackResult, PubnubDisplayErrorMessage);
                        }

                        RadioButton radAuditPAMPresenceChannelGroup = control.FindName("radAuditPresenceChannelGroup") as RadioButton;
                        if (radAuditPAMPresenceChannelGroup != null && radAuditPAMPresenceChannelGroup.IsChecked.Value)
                        {
                            DisplayMessageInTextBox("Running ChannelGroupAuditAccess:");
                            pubnub.ChannelGroupAuditPresenceAccess<string>(pamUserChannelGroup, pamAuthKey, PubnubCallbackResult, PubnubDisplayErrorMessage);
                        }

                        RadioButton radRevokePAMPresenceChannelGroup = control.FindName("radRevokePresenceChannelGroup") as RadioButton;
                        if (radRevokePAMPresenceChannelGroup != null && radRevokePAMPresenceChannelGroup.IsChecked.Value)
                        {
                            DisplayMessageInTextBox("Running ChannelGroup Revoke Access:");
                            pubnub.ChannelGroupGrantPresenceAccess<string>(pamUserChannelGroup, pamAuthKey, false, false, PubnubCallbackResult, PubnubDisplayErrorMessage);
                        }
                    }
                }
                pamChannelGroupPopup = null;
                this.IsEnabled = true;
            };

        }

        //private void btnGrant_Click(object sender, RoutedEventArgs e)
        //{
        //    this.IsEnabled = false;
        //    Border border = new Border();
        //    border.BorderBrush = new SolidColorBrush(Colors.Black);
        //    border.BorderThickness = new Thickness(5.0);

        //    StackPanel pamAuthKeyStackPanel = new StackPanel();
        //    pamAuthKeyStackPanel.Background = new SolidColorBrush(Colors.Blue);
        //    pamAuthKeyStackPanel.Width = 400;
        //    pamAuthKeyStackPanel.Height = 300;

        //    Popup pamAuthKeyPopup = new Popup();
        //    pamAuthKeyPopup.Height = 300;
        //    pamAuthKeyPopup.Width = 300;

        //    var point = CalcOffsets((UIElement)sender);
        //    pamAuthKeyPopup.HorizontalOffset = point.X + 70;
        //    pamAuthKeyPopup.VerticalOffset = point.Y - 200;

        //    PAMAuthKeyUserControl control = new PAMAuthKeyUserControl();

        //    TextBox txtPAMAuthKey = control.FindName("txtPAMAuthKey") as TextBox;
        //    if (txtPAMAuthKey != null)
        //    {
        //        txtPAMAuthKey.Text = "";
        //    }

        //    pamAuthKeyStackPanel.Children.Add(control);
        //    border.Child = pamAuthKeyStackPanel;

        //    pamAuthKeyPopup.Child = border;
        //    pamAuthKeyPopup.IsOpen = true;

        //    pamAuthKeyPopup.Closed += (senderPopup, argsPopup) =>
        //    {
        //        if (control.IsOKButtonEntered)
        //        {
        //            txtPAMAuthKey = control.FindName("txtPAMAuthKey") as TextBox;
        //            if (txtPAMAuthKey != null)
        //            {
        //                string pamAuthKey = txtPAMAuthKey.Text.Trim();

        //                DisplayMessageInTextBox("Running Grant:");
        //                int ttlInMinutes = 1440;
        //                pubnub.GrantAccess<string>(channel, pamAuthKey, true, true, ttlInMinutes, PubnubCallbackResult, PubnubDisplayErrorMessage);
        //            }

        //        }
        //        pamAuthKeyPopup = null;
        //        this.IsEnabled = true;
        //    };
        //}

        //private void btnGrantPresence_Click(object sender, RoutedEventArgs e)
        //{
        //    channel = txtChannel.Text;

        //    this.IsEnabled = false;
        //    Border border = new Border();
        //    border.BorderBrush = new SolidColorBrush(Colors.Black);
        //    border.BorderThickness = new Thickness(5.0);

        //    StackPanel pamAuthKeyStackPanel = new StackPanel();
        //    pamAuthKeyStackPanel.Background = new SolidColorBrush(Colors.Blue);
        //    pamAuthKeyStackPanel.Width = 400;
        //    pamAuthKeyStackPanel.Height = 300;

        //    Popup pamAuthKeyPopup = new Popup();
        //    pamAuthKeyPopup.Height = 300;
        //    pamAuthKeyPopup.Width = 300;

        //    var point = CalcOffsets((UIElement)sender);
        //    pamAuthKeyPopup.HorizontalOffset = point.X + 70;
        //    pamAuthKeyPopup.VerticalOffset = point.Y - 200;

        //    PAMAuthKeyUserControl control = new PAMAuthKeyUserControl();

        //    TextBox txtPAMAuthKey = control.FindName("txtPAMAuthKey") as TextBox;
        //    if (txtPAMAuthKey != null)
        //    {
        //        txtPAMAuthKey.Text = "";
        //    }

        //    pamAuthKeyStackPanel.Children.Add(control);
        //    border.Child = pamAuthKeyStackPanel;

        //    pamAuthKeyPopup.Child = border;
        //    pamAuthKeyPopup.IsOpen = true;

        //    pamAuthKeyPopup.Closed += (senderPopup, argsPopup) =>
        //    {
        //        if (control.IsOKButtonEntered)
        //        {
        //            txtPAMAuthKey = control.FindName("txtPAMAuthKey") as TextBox;
        //            if (txtPAMAuthKey != null)
        //            {
        //                string pamAuthKey = txtPAMAuthKey.Text.Trim();

        //                DisplayMessageInTextBox("Running GrantPresenceAccess:");
        //                int ttlInMinutes = 1440;
        //                pubnub.GrantPresenceAccess<string>(channel, pamAuthKey, true, true, ttlInMinutes, PubnubCallbackResult, PubnubDisplayErrorMessage);
        //            }

        //        }
        //        pamAuthKeyPopup = null;
        //        this.IsEnabled = true;
        //    };
        //}

        //private void btnRevoke_Click(object sender, RoutedEventArgs e)
        //{
        //    channel = txtChannel.Text;

        //    this.IsEnabled = false;
        //    Border border = new Border();
        //    border.BorderBrush = new SolidColorBrush(Colors.Black);
        //    border.BorderThickness = new Thickness(5.0);

        //    StackPanel pamAuthKeyStackPanel = new StackPanel();
        //    pamAuthKeyStackPanel.Background = new SolidColorBrush(Colors.Blue);
        //    pamAuthKeyStackPanel.Width = 400;
        //    pamAuthKeyStackPanel.Height = 300;

        //    Popup pamAuthKeyPopup = new Popup();
        //    pamAuthKeyPopup.Height = 300;
        //    pamAuthKeyPopup.Width = 300;

        //    var point = CalcOffsets((UIElement)sender);
        //    pamAuthKeyPopup.HorizontalOffset = point.X + 70;
        //    pamAuthKeyPopup.VerticalOffset = point.Y - 200;

        //    PAMAuthKeyUserControl control = new PAMAuthKeyUserControl();

        //    TextBox txtPAMAuthKey = control.FindName("txtPAMAuthKey") as TextBox;
        //    if (txtPAMAuthKey != null)
        //    {
        //        txtPAMAuthKey.Text = "";
        //    }

        //    pamAuthKeyStackPanel.Children.Add(control);
        //    border.Child = pamAuthKeyStackPanel;

        //    pamAuthKeyPopup.Child = border;
        //    pamAuthKeyPopup.IsOpen = true;

        //    pamAuthKeyPopup.Closed += (senderPopup, argsPopup) =>
        //    {
        //        if (control.IsOKButtonEntered)
        //        {
        //            txtPAMAuthKey = control.FindName("txtPAMAuthKey") as TextBox;
        //            if (txtPAMAuthKey != null)
        //            {
        //                string pamAuthKey = txtPAMAuthKey.Text.Trim();

        //                DisplayMessageInTextBox("Running Revoke:");
        //                pubnub.GrantAccess<string>(channel, pamAuthKey, false, false, PubnubCallbackResult, PubnubDisplayErrorMessage);
        //            }

        //        }
        //        pamAuthKeyPopup = null;
        //        this.IsEnabled = true;
        //    };
        //}

        //private void btnAudit_Click(object sender, RoutedEventArgs e)
        //{
        //    channel = txtChannel.Text;

        //    this.IsEnabled = false;
        //    Border border = new Border();
        //    border.BorderBrush = new SolidColorBrush(Colors.Black);
        //    border.BorderThickness = new Thickness(5.0);

        //    StackPanel pamAuthKeyStackPanel = new StackPanel();
        //    pamAuthKeyStackPanel.Background = new SolidColorBrush(Colors.Blue);
        //    pamAuthKeyStackPanel.Width = 400;
        //    pamAuthKeyStackPanel.Height = 300;

        //    Popup pamAuthKeyPopup = new Popup();
        //    pamAuthKeyPopup.Height = 300;
        //    pamAuthKeyPopup.Width = 300;

        //    var point = CalcOffsets((UIElement)sender);
        //    pamAuthKeyPopup.HorizontalOffset = point.X + 70;
        //    pamAuthKeyPopup.VerticalOffset = point.Y - 200;

        //    PAMAuthKeyUserControl control = new PAMAuthKeyUserControl();

        //    TextBox txtPAMAuthKey = control.FindName("txtPAMAuthKey") as TextBox;
        //    if (txtPAMAuthKey != null)
        //    {
        //        txtPAMAuthKey.Text = "";
        //    }

        //    pamAuthKeyStackPanel.Children.Add(control);
        //    border.Child = pamAuthKeyStackPanel;

        //    pamAuthKeyPopup.Child = border;
        //    pamAuthKeyPopup.IsOpen = true;

        //    pamAuthKeyPopup.Closed += (senderPopup, argsPopup) =>
        //    {
        //        if (control.IsOKButtonEntered)
        //        {
        //            txtPAMAuthKey = control.FindName("txtPAMAuthKey") as TextBox;
        //            if (txtPAMAuthKey != null)
        //            {
        //                string pamAuthKey = txtPAMAuthKey.Text.Trim();

        //                DisplayMessageInTextBox("Running AuditAccess:");
        //                pubnub.AuditAccess<string>(channel, pamAuthKey, PubnubCallbackResult, PubnubDisplayErrorMessage);
        //            }

        //        }
        //        pamAuthKeyPopup = null;
        //        this.IsEnabled = true;
        //    };
        //}

        //private void btnAuditPresence_Click(object sender, RoutedEventArgs e)
        //{
        //    channel = txtChannel.Text;

        //    this.IsEnabled = false;
        //    Border border = new Border();
        //    border.BorderBrush = new SolidColorBrush(Colors.Black);
        //    border.BorderThickness = new Thickness(5.0);

        //    StackPanel pamAuthKeyStackPanel = new StackPanel();
        //    pamAuthKeyStackPanel.Background = new SolidColorBrush(Colors.Blue);
        //    pamAuthKeyStackPanel.Width = 400;
        //    pamAuthKeyStackPanel.Height = 300;

        //    Popup pamAuthKeyPopup = new Popup();
        //    pamAuthKeyPopup.Height = 300;
        //    pamAuthKeyPopup.Width = 300;

        //    var point = CalcOffsets((UIElement)sender);
        //    pamAuthKeyPopup.HorizontalOffset = point.X + 70;
        //    pamAuthKeyPopup.VerticalOffset = point.Y - 200;

        //    PAMAuthKeyUserControl control = new PAMAuthKeyUserControl();

        //    TextBox txtPAMAuthKey = control.FindName("txtPAMAuthKey") as TextBox;
        //    if (txtPAMAuthKey != null)
        //    {
        //        txtPAMAuthKey.Text = "";
        //    }

        //    pamAuthKeyStackPanel.Children.Add(control);
        //    border.Child = pamAuthKeyStackPanel;

        //    pamAuthKeyPopup.Child = border;
        //    pamAuthKeyPopup.IsOpen = true;

        //    pamAuthKeyPopup.Closed += (senderPopup, argsPopup) =>
        //    {
        //        if (control.IsOKButtonEntered)
        //        {
        //            txtPAMAuthKey = control.FindName("txtPAMAuthKey") as TextBox;
        //            if (txtPAMAuthKey != null)
        //            {
        //                string pamAuthKey = txtPAMAuthKey.Text.Trim();

        //                DisplayMessageInTextBox("Running AuditPresenceAccess:");
        //                pubnub.AuditPresenceAccess<string>(channel, pamAuthKey, PubnubCallbackResult, PubnubDisplayErrorMessage);
        //            }

        //        }
        //        pamAuthKeyPopup = null;
        //        this.IsEnabled = true;
        //    };
        //}

        private void btnUserState_Click(object sender, RoutedEventArgs e)
        {
            channel = txtChannel.Text;
            channelGroup = txtChannelGroup.Text;    
            this.IsEnabled = false;
            Border border = new Border();
            border.BorderBrush = new SolidColorBrush(Colors.Black);
            border.BorderThickness = new Thickness(5.0);

            StackPanel userStateStackPanel = new StackPanel();
            userStateStackPanel.Background = new SolidColorBrush(Colors.Blue);

            userStatePopup = new Popup();
            userStatePopup.Height = 300;
            userStatePopup.Width = 300;
            
            var point = CalcOffsets((UIElement)sender);
            userStatePopup.HorizontalOffset = point.X + 70;
            userStatePopup.VerticalOffset = point.Y - 200;
            
            UserStateUserControl control = new UserStateUserControl();
            TextBox txtGetUserStateUUID = control.FindName("txtGetUserStateUUID") as TextBox;
            if (txtGetUserStateUUID != null)
            {
                txtGetUserStateUUID.Text = data.sessionUUID;
            }

            //control.txtGetUserStateUUID.Text = uuid;
            userStateStackPanel.Children.Add(control);
            border.Child = userStateStackPanel;

            userStatePopup.Child = border;
            userStatePopup.IsOpen = true;

            userStatePopup.Closed += (senderPopup, argsPopup) =>
            {
                if (control.IsOKButtonEntered)
                {
                    if (control.IsSetUserState)
                    {
                        string userStateKey1 = "";
                        string userStateValue1 = "";
                        TextBox txtSetUserStateKey1 = control.FindName("txtKey1") as TextBox;
                        if (txtSetUserStateKey1 != null)
                        {
                            userStateKey1 = txtSetUserStateKey1.Text;
                        }
                        TextBox txtSetUserStateVal1 = control.FindName("txtValue1") as TextBox;
                        if (txtSetUserStateVal1 != null)
                        {
                            userStateValue1 = txtSetUserStateVal1.Text;
                        }

                        DisplayMessageInTextBox("Running Set User State:");

                        int valueInt;
                        double valueDouble;
                        if (Int32.TryParse(userStateValue1, out valueInt))
                        {
                            pubnub.SetUserState<string>(channel, channelGroup, new KeyValuePair<string, object>(userStateKey1, valueInt), PubnubCallbackResult, PubnubDisplayErrorMessage);
                        }
                        else if (Double.TryParse(userStateValue1, out valueDouble))
                        {
                            pubnub.SetUserState<string>(channel, channelGroup, new KeyValuePair<string, object>(userStateKey1, valueDouble), PubnubCallbackResult, PubnubDisplayErrorMessage);
                        }
                        else
                        {
                            pubnub.SetUserState<string>(channel, channelGroup, new KeyValuePair<string, object>(userStateKey1, userStateValue1), PubnubCallbackResult, PubnubDisplayErrorMessage);
                        }
                    }
                    else if (control.IsGetUserState)
                    {
                        txtGetUserStateUUID = control.FindName("txtGetUserStateUUID") as TextBox;
                        if (txtGetUserStateUUID != null)
                        {
                            DisplayMessageInTextBox("Running Get User State:");
                            string userStateUUID = txtGetUserStateUUID.Text.Trim();
                            pubnub.GetUserState<string>(channel, channelGroup, userStateUUID, PubnubCallbackResult, PubnubDisplayErrorMessage);
                        }
                    }
                }
                userStatePopup = null;
                this.IsEnabled = true;
            };
        }

        private void btnWhereNow_Click(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            Border border = new Border();
            border.BorderBrush = new SolidColorBrush(Colors.Black);
            border.BorderThickness = new Thickness(5.0);

            StackPanel whereNowStackPanel = new StackPanel();
            whereNowStackPanel.Background = new SolidColorBrush(Colors.Blue);
            whereNowStackPanel.Width = 300;
            whereNowStackPanel.Height = 300;

            whereNowPopup = new Popup();
            whereNowPopup.Height = 300;
            whereNowPopup.Width = 300;

            var point = CalcOffsets((UIElement)sender);
            whereNowPopup.HorizontalOffset = point.X + 70;
            whereNowPopup.VerticalOffset = point.Y - 100;

            WhereNowUserControl control = new WhereNowUserControl();
            TextBox txtWhereNowUUID = control.FindName("txtWhereNowUUID") as TextBox;
            if (txtWhereNowUUID != null)
            {
                txtWhereNowUUID.Text = data.sessionUUID;
            }

            whereNowStackPanel.Children.Add(control);
            border.Child = whereNowStackPanel;

            whereNowPopup.Child = border;
            whereNowPopup.IsOpen = true;
            
            whereNowPopup.Closed += (senderPopup, argsPopup) =>
            {
                if (control.IsOKButtonEntered)
                {
                    TextBox txtWhereNowUUIDConfirm = control.FindName("txtWhereNowUUID") as TextBox;
                    if (txtWhereNowUUIDConfirm != null)
                    {
                        string whereNowUUID = txtWhereNowUUIDConfirm.Text.Trim();
                        
                        DisplayMessageInTextBox("Running WhereNow:");
                        pubnub.WhereNow<string>(whereNowUUID, PubnubCallbackResult, PubnubDisplayErrorMessage);
                    }

                }
                whereNowPopup = null;
                this.IsEnabled = true;
            };
        }

        private void btnChangeUUID_Click(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            Border border = new Border();
            border.BorderBrush = new SolidColorBrush(Colors.Black);
            border.BorderThickness = new Thickness(5.0);

            StackPanel changeUUIDStackPanel = new StackPanel();
            changeUUIDStackPanel.Background = new SolidColorBrush(Colors.Blue);
            changeUUIDStackPanel.Width = 400;
            changeUUIDStackPanel.Height = 300;

            changeUUIDPopup = new Popup();
            changeUUIDPopup.Height = 300;
            changeUUIDPopup.Width = 300;

            var point = CalcOffsets((UIElement)sender);
            changeUUIDPopup.HorizontalOffset = point.X+50;
            changeUUIDPopup.VerticalOffset = point.Y-70;

            ChangeUUIDUserControl control = new ChangeUUIDUserControl();
            TextBlock tbCurrentUUID = control.FindName("lblCurrentUUID") as TextBlock;
            if (tbCurrentUUID != null)
            {
                tbCurrentUUID.Text = data.sessionUUID;
            }

            changeUUIDStackPanel.Children.Add(control);
            border.Child = changeUUIDStackPanel;

            changeUUIDPopup.Child = border;
            changeUUIDPopup.IsOpen = true;

            changeUUIDPopup.Closed += (senderPopup, argsPopup) =>
            {
                if (!control.IsCancelledButton)
                {
                    TextBox txtNewUUID = control.FindName("txtNewUUID") as TextBox;
                    if (txtNewUUID != null)
                    {
                        data.sessionUUID = txtNewUUID.Text;
                        pubnub.ChangeUUID(data.sessionUUID);
                    }
                }
                changeUUIDPopup = null;
                this.IsEnabled = true;
            };

        }

        private void btnChannelGroup_Click(object sender, RoutedEventArgs e)
        {
            channelGroup = txtChannelGroup.Text;

            this.IsEnabled = false;
            Border border = new Border();
            border.BorderBrush = new SolidColorBrush(Colors.Black);
            border.BorderThickness = new Thickness(5.0);

            StackPanel channelGroupStackPanel = new StackPanel();
            channelGroupStackPanel.Background = new SolidColorBrush(Colors.Blue);
            channelGroupStackPanel.Width = 400;
            channelGroupStackPanel.Height = 400;

            Popup channelGroupPopup = new Popup();
            channelGroupPopup.Height = 300;
            channelGroupPopup.Width = 300;

            channelGroupPopup.HorizontalOffset = 10;
            channelGroupPopup.VerticalOffset = 100;

            ChannelGroupUserControl control = new ChannelGroupUserControl();

            TextBox txtPAMChannelGroup = control.FindName("txtChannelGroup") as TextBox;
            if (txtPAMChannelGroup != null)
            {
                txtPAMChannelGroup.Text = channelGroup;
            }
            TextBox txtChannelName = control.FindName("txtChannelName") as TextBox;
            if (txtChannelName != null)
            {
                txtChannelName.Text = "ch1";
            }

            channelGroupStackPanel.Children.Add(control);
            border.Child = channelGroupStackPanel;

            channelGroupPopup.Child = border;
            channelGroupPopup.IsOpen = true;

            channelGroupPopup.Closed += (senderPopup, argsPopup) =>
            {
                if (control.IsOKButtonEntered)
                {
                    string userChannelGroup = "";
                    string userChannelName = "";

                    txtPAMChannelGroup = control.FindName("txtChannelGroup") as TextBox;
                    if (txtPAMChannelGroup != null)
                    {
                        userChannelGroup = txtPAMChannelGroup.Text;

                        txtChannelName = control.FindName("txtChannelName") as TextBox;
                        if (txtChannelName != null)
                        {
                            userChannelName = txtChannelName.Text;
                        }

                        RadioButton radGetChannelsOfChannelGroup = control.FindName("radGetChannelsOfChannelGroup") as RadioButton;
                        if (radGetChannelsOfChannelGroup != null && radGetChannelsOfChannelGroup.IsChecked.Value)
                        {
                            DisplayMessageInTextBox("Running GetChannelsForChannelGroup:");
                            pubnub.GetChannelsForChannelGroup<string>(userChannelGroup, PubnubCallbackResult, PubnubDisplayErrorMessage);
                        }

                        RadioButton radAddChannelToChannelGroup = control.FindName("radAddChannelToChannelGroup") as RadioButton;
                        if (radAddChannelToChannelGroup != null && radAddChannelToChannelGroup.IsChecked.Value)
                        {
                            DisplayMessageInTextBox("Running AddChannelsToChannelGroup:");
                            pubnub.AddChannelsToChannelGroup<string>(new string[] { userChannelName }, userChannelGroup, PubnubCallbackResult, PubnubDisplayErrorMessage);
                        }

                        RadioButton radRemoveChannelFromChannelGroup = control.FindName("radRemoveChannelFromChannelGroup") as RadioButton;
                        if (radRemoveChannelFromChannelGroup != null && radRemoveChannelFromChannelGroup.IsChecked.Value)
                        {
                            DisplayMessageInTextBox("Running RemoveChannelsFromChannelGroup:");
                            pubnub.RemoveChannelsFromChannelGroup<string>(new string[] { userChannelName }, userChannelGroup, PubnubCallbackResult, PubnubDisplayErrorMessage);
                        }

                    }
                }
                channelGroupPopup = null;
                this.IsEnabled = true;
            };

        }

        private void btnDisconnectRetry_Click(object sender, RoutedEventArgs e)
        {
            pubnub.TerminateCurrentSubscriberRequest();
        }

        private void btnDisableNetwork_Click(object sender, RoutedEventArgs e)
        {
            pubnub.EnableSimulateNetworkFailForTestingOnly();
        }

        private void btnEnableNetwork_Click(object sender, RoutedEventArgs e)
        {
            pubnub.DisableSimulateNetworkFailForTestingOnly();
        }

        private async void DisplayMessageInTextBox(string msg)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                if (txtResult.Text.Length > 400)
                {
                    txtResult.Text = string.Concat("(Truncated)..\n", txtResult.Text.Remove(0, 200));
                }
                txtResult.Text += msg + "\n";
                txtResult.Select(txtResult.Text.Length-1, 1);
            });
        }

        private Point CalcOffsets(UIElement elem)
        {
            var transform = elem.TransformToVisual(this);
            Point point = transform.TransformPoint(new Point(0, 0));

            return point;
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            PubnubCleanup();
        }

        void PubnubCleanup()
        {
            if (pubnub != null)
            {
                pubnub.EndPendingRequests();
                pubnub = null;
            }
        }

    }
}
