using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.Devices.Enumeration;

namespace Pos.Services
{
    public class Print
    {
        public async Task<bool> BluetoothPrinting(string printername, string datatoprint)
        {
            bool done = false;

            // Find the Bluetooth device
            var devices = await DeviceInformation.FindAllAsync(BluetoothDevice.GetDeviceSelector());
            var deviceInfo = devices.FirstOrDefault(d => d.Name == printername);

            if (deviceInfo == null)
            {
                throw new Exception($"{printername} device not found.");
            }

            // Connect to the Bluetooth device
            var device = await BluetoothDevice.FromIdAsync(deviceInfo.Id);
            var rfcommServices = await device.GetRfcommServicesAsync();

            // Ensure the device has Rfcomm services available
            if (rfcommServices.Services.Count == 0)
            {
                throw new Exception("No Rfcomm services available on this device.");
            }

            // Select the first available Rfcomm service
            var rfcommService = rfcommServices.Services.First();
            using (var socket = new StreamSocket())
            {
                try
                {
                    await socket.ConnectAsync(rfcommService.ConnectionHostName, rfcommService.ConnectionServiceName);

                    // Initialize printer command sequences
                    string ESC = ((char)27).ToString();
                    string GS = ((char)29).ToString();

                    string center = ESC + "a" + (char)1; //align center
                    string left = ESC + "a" + (char)0; //align left
                    string right = ESC + "a" + (char)2; //align right
                    string bold_on = ESC + "E" + (char)1; //turn on bold mode
                    string bold_off = ESC + "E" + (char)0; //turn off bold mode
                    string bigSize = ESC + "!" + (char)255;
                    string smallSize = ESC + "!" + (char)0;
                    string cut = ESC + "d" + (char)1 + GS + "V" + (char)66; //cut command

                    // Prepare data to send
                    string buffer = "";
                    buffer += "-------------------------------\n";
                    buffer += center + bigSize + bold_on + "HELLO WORLD!\n";
                    buffer += $"{datatoprint}\n";
                    buffer += bold_off + smallSize + left;
                    buffer += "********************************\n";
                    buffer += smallSize + "Jerry O. Mates Jr.\n\n\n\n";

                    // Convert the buffer to bytes and send it
                    byte[] message = Encoding.ASCII.GetBytes(buffer);
                    using (var writer = new DataWriter(socket.OutputStream))
                    {
                        writer.WriteBytes(message);
                        await writer.StoreAsync();
                        writer.WriteByte(0x0A); // newline
                        await writer.StoreAsync();
                    }

                    done = true;
                }
                catch (Exception exp)
                {
                    throw new Exception("Error in Bluetooth printing: " + exp.Message);
                }
                finally
                {
                    socket.Dispose();
                }
            }

            return done;
        }
    }
}
