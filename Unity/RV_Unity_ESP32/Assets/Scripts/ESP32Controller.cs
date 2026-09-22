using UnityEngine;
using System.IO.Ports;
using System.Globalization;

public class ESP32Controller : MonoBehaviour
{
    [Header("Serial Communication")]
    public string portName = "COM4";
    public int baudRate = 115200;

    [Header("Curtain")]
    public Cloth targetCloth;

    [Header("Wind Values")]
    public float windX = 0f;
    public float windZ = 0f;

    private SerialPort serialPort;

    void Start()
    {
        try
        {
            serialPort = new SerialPort(portName, baudRate);

            // Prevent Unity from waiting indefinitely for serial data
            serialPort.ReadTimeout = 50;

            serialPort.Open();

            Debug.Log("ESP32 connected successfully on " + portName);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Could not open ESP32 serial port: " + e.Message);
        }
    }

    void Update()
    {
        // =====================================================
        // 1. READ DATA FROM ESP32
        // =====================================================

        if (serialPort != null && serialPort.IsOpen)
        {
            try
            {
                // Expected format:
                // windX,windZ
                //
                // Example:
                // -8.09,-4.57

                string data = serialPort.ReadLine().Trim();

                string[] values = data.Split(',');

                if (values.Length == 2)
                {
                    bool validX = float.TryParse(
                        values[0],
                        NumberStyles.Float,
                        CultureInfo.InvariantCulture,
                        out float receivedX
                    );

                    bool validZ = float.TryParse(
                        values[1],
                        NumberStyles.Float,
                        CultureInfo.InvariantCulture,
                        out float receivedZ
                    );

                    if (validX && validZ)
                    {
                        windX = receivedX;
                        windZ = receivedZ;
                    }
                }
            }
            catch (System.TimeoutException)
            {
                // No complete serial line available this frame.
                // This is normal.
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("Serial reading error: " + e.Message);
            }
        }


        // =====================================================
        // 2. APPLY WIND TO THE CURTAIN
        // =====================================================

        if (targetCloth != null)
        {
            targetCloth.externalAcceleration =
                new Vector3(windX, 0f, windZ);
        }
    }

    void OnApplicationQuit()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }
}