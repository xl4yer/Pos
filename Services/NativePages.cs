using Microsoft.Graph.Models;
using Pos.Class;
using Windows.Devices.Bluetooth;
using Windows.Devices.Enumeration;
using Pos.Services;

namespace Pos.Services
{
    public class NativePages : INativePages
    {
        public async Task<bool> StartActivityInPrinting(string printername, string datatoprint)
        {
            var print = new Print();
            bool printSuccessful = false;

            // Find all paired Bluetooth devices
            var devices = await DeviceInformation.FindAllAsync(BluetoothDevice.GetDeviceSelector());

            BluetoothDevice targetDevice = null;
            foreach (var deviceInfo in devices)
            {
                // Check if the device matches the specified Bluetooth address
                if (deviceInfo.Id.Contains("00:11:22:33:44:55"))
                {
                    targetDevice = await BluetoothDevice.FromIdAsync(deviceInfo.Id);
                    printername = deviceInfo.Name;
                    break;
                }
            }

            if (targetDevice != null && !string.IsNullOrEmpty(printername))
            {
                // Call the printing function
                printSuccessful = await print.BluetoothPrinting(printername, datatoprint);
            }

            return printSuccessful;
        }
    }
}
