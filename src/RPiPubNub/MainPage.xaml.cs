using Newtonsoft.Json.Linq;
using PubNubMessaging.Core;
using System;
using System.Diagnostics;
using Windows.Data.Json;
using Windows.Devices.Enumeration;
using Windows.Devices.Gpio;
using Windows.Devices.I2c;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace RPiPubNub
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        // I2C Controller name
        private const string I2C_CONTROLLER_NAME = "I2C1";
        // TMP102 Address
        private const int TMP102_ADDR = 0x48;

        private class PubNubMessage
        {
            public String pin;
            public String value;
        }

        private class Pins
        {
            public Pins(int pinno, GpioPin gpiopin)
            {
                pinNumber = pinno;
                pin = gpiopin;
            }

            public int pinNumber;
            public GpioPin pin;
        }

        // Timer
        private DispatcherTimer TimerRoutine;
        // I2C Device
        private I2cDevice I2CDev;
        // GPIO 
        private static GpioController gpio = GpioController.GetDefault();

        // GPIO Pin array
        private static Pins[] gpio_pins =
        {
            new Pins(4, null ),
            new Pins(17, null ),
            new Pins(27, null ),
            new Pins(22, null ),
            new Pins(5, null ),
            new Pins(6, null ),
            new Pins(13, null ),
            new Pins(19, null ),
            new Pins(26, null ),
            new Pins(18, null ),
            new Pins(23, null ),
            new Pins(24, null ),
            new Pins(25, null ),
            new Pins(12, null ),
            new Pins(16, null ),
            new Pins(20, null ),
            new Pins(21, null )
        };

        // PubNub publish and subscribe keys
        Pubnub pubnub = new Pubnub("<<publish-key>>", "subscribe-key>>");
        Random rnd = new Random();

        public MainPage()
        {
            this.InitializeComponent();

            // Initialize I2C Device
            InitializeI2CDevice();
            // Initialize GPIO
            InitializeGPIO();
            // Subscribe to PubNub messages
            SubscribePubNubMessages();

            // Start Timer every 5 seconds
            TimerRoutine = new DispatcherTimer();
            TimerRoutine.Interval = TimeSpan.FromMilliseconds(5000);
            TimerRoutine.Tick += Timer_Tick;
            TimerRoutine.Start();

            Unloaded += MainPage_Unloaded;
        }

        private async void InitializeI2CDevice()
        {
            try
            {
                // Initialize I2C device
                var settings = new I2cConnectionSettings(TMP102_ADDR);

                settings.BusSpeed = I2cBusSpeed.FastMode;
                settings.SharingMode = I2cSharingMode.Shared;

                string aqs = I2cDevice.GetDeviceSelector(I2C_CONTROLLER_NAME);  /* Find the selector string for the I2C bus controller                   */
                var dis = await DeviceInformation.FindAllAsync(aqs);            /* Find the I2C bus controller device with our selector string           */

                I2CDev = await I2cDevice.FromIdAsync(dis[0].Id, settings);    /* Create an I2cDevice with our selected bus controller and I2C settings */
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());

                return;
            }
        }

        private void SubscribePubNubMessages()
        {
            pubnub.Subscribe<string>("rpipb-control", PubNubSubscribeSuccess, DisplaySubscribeConnectStatusMessage, PubNubError);
        }

        private void InitializeGPIO()
        {

        }

        private void MainPage_Unloaded(object sender, object args)
        {
            // Cleanup
            I2CDev.Dispose();
            // Dispose GPIO Pins
            disposeGpioPins();
        }

        // Dispose GPIO Pins
        private void disposeGpioPins()
        {
            int count = gpio_pins.Length;

            for (int index = 0; index < count; ++index)
            {
                if (null != gpio_pins[index].pin)
                {
                    gpio_pins[index].pin.Dispose();

                    gpio_pins[index].pin = null;
                }
            }
        }

        // Read the TMP102 sensor and return temperature
        private float getTemperature()
        {
            byte[] aaddr = new byte[] { (byte)(TMP102_ADDR) };
            byte[] data = new byte[2];

            I2CDev.Read(data);

            float temp = (((data[0] << 8) | (data[1])) >> 4);
            float celsius = (float)(temp * 0.0625);

            return celsius;
        }

        // Timer routine to read sensor value and send pubnub message
        private void Timer_Tick(object sender, object e)
        {
            JsonObject json = new JsonObject();
            int temperature = (int)getTemperature();

            json["Temperature"] = JsonValue.CreateNumberValue(temperature);

            String jsonString = json.Stringify();
            
            pubnub.Publish<string>("rpipb-temperature", jsonString, PubNubSucess, PubNubError);
        }

        // Receive PubNub messages
        private static void PubNubSubscribeSuccess(string publishResult)
        {
            Debug.WriteLine("Message: " + publishResult);

            JArray message = JArray.Parse(publishResult);

            Debug.WriteLine("Pin: " + message[0]["pin"] + ", Value: " + message[0]["value"]);

            // Get pin and value
            int pin = Convert.ToInt32(message[0]["pin"].ToString());
            int value = Convert.ToInt32(message[0]["value"].ToString());

            // Get index of the array from the pin number
            int index = getIndexFromPinNumber(pin);

            // If we have a valid pin number, then proceed
            if (-1 != index)
            {
                // Check whether we have already initialized the pin or not
                if (null == gpio_pins[index].pin)
                {
                    gpio_pins[index].pin = gpio.OpenPin(pin);
                }

                // Initialize pin value according to the value from message
                GpioPinValue gpiovalue = GpioPinValue.High;

                if (1 == value)
                    gpiovalue = GpioPinValue.High;
                else
                    gpiovalue = GpioPinValue.Low;

                // Set the pin value
                gpio_pins[index].pin.SetDriveMode(GpioPinDriveMode.Output);
                gpio_pins[index].pin.Write(gpiovalue);
            }
        }

        private static void DisplaySubscribeConnectStatusMessage(string publishResult)
        {
            Debug.WriteLine("Connection: " + publishResult);
        }

        // Successfully send the message, simply display it on the console
        private static void PubNubSucess(string publishResult)
        {
            Debug.WriteLine("Success: " + publishResult);
        }

        // We have some issue sending the message, simply display it on the console
        private static void PubNubError(PubnubClientError error)
        {
            Debug.WriteLine("Error: " + error.ToString());
        }

        // Return the index of the pin array from pin number
        private static int  getIndexFromPinNumber(int num)
        {
            int count = gpio_pins.Length;
            int pinNumber = -1;

            // Iterate through the array and find the pin
            for(int index=0; index<count; ++index)
            {
                if(gpio_pins[index].pinNumber == num)
                {
                    pinNumber = index;
                }
            }

            return pinNumber;
        }
    }
}