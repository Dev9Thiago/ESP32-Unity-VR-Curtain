# ESP32–Unity VR Curtain Simulation

Interactive virtual reality system that integrates an **ESP32 microcontroller** with **Unity** to control the physical simulation of a virtual curtain in real time.

The project was developed for the Virtual Reality course of the Mechatronics Engineering program at **Universidad Militar Nueva Granada**.

## Project Overview

Two analog potentiometers connected to an ESP32 independently control the X and Z components of a virtual wind vector. The ESP32 acquires and filters the analog signals, maps them to the interval `[-20, 20]`, and sends the resulting values to Unity through USB serial communication.

Unity receives both components and applies them as an external acceleration vector to a `Cloth` component:

```text
a_ext = [WindX, 0, WindZ]^T
```

The resulting environment is also configured for virtual reality using **OpenXR**, **SteamVR**, and **VRidge**, allowing the scene to be visualized stereoscopically on a Xiaomi 11T smartphone with head-orientation tracking.

---

## System Architecture

```text
Potentiometer X ──> GPIO32 ──┐
                             │
Potentiometer Z ──> GPIO33 ──┤
                             v
                           ESP32
                             │
                   ADC Signal Acquisition
                             │
                   5-Sample Moving Average
                             │
                    Mapping [-20, 20]
                             │
                  USB Serial @ 115200 baud
                             │
                             v
                           Unity
                             │
                   ESP32Controller.cs
                             │
              Vector3(WindX, 0, WindZ)
                             │
                             v
                  Cloth.externalAcceleration
                             │
                             v
                 Virtual Curtain Simulation
                             │
                             v
                 OpenXR / SteamVR / VRidge
                             │
                             v
                      Xiaomi 11T
```

---

## Repository Structure

```text
ESP32-Unity-VR-Curtain/
│
├── ESP32/
│   └── wind_controller_for_unity/
│       └── wind_controller_for_unity.ino
│
├── Unity/
│   └── RV_Unity_ESP32/
│       ├── Assets/
│       ├── Packages/
│       └── ProjectSettings/
│
├── Documentation/
│   ├── Images/
│   └── Report/
│
├── .gitignore
└── README.md
```

Unity-generated directories such as `Library`, `Temp`, `Logs`, and `UserSettings` are intentionally excluded from version control.

---

## Hardware

The physical interface consists of:

- ESP32 development board
- Two analog trimmer potentiometers
- Breadboard
- Jumper wires
- USB cable
- Windows computer
- Xiaomi 11T smartphone for VR visualization

### ESP32 Connections

| Variable | ESP32 Pin | Function |
|---|---:|---|
| Wind X | GPIO32 | X-axis wind component |
| Wind Z | GPIO33 | Z-axis wind component |
| Supply | 3.3 V | Potentiometer supply |
| Reference | GND | Electrical reference |

Each potentiometer operates as a voltage divider, with its central terminal connected to the corresponding ESP32 ADC input.

---

## ESP32 Firmware

The firmware is located at:

```text
ESP32/wind_controller_for_unity/wind_controller_for_unity.ino
```

The ESP32 performs four main operations:

1. Acquisition of the two analog signals.
2. Five-sample moving-average filtering.
3. Mapping of each processed signal to `[-20, 20]`.
4. Serial transmission at `115200 baud`.

The transmitted serial frame has the format:

```text
WindX,WindZ
```

Example:

```text
15.06,14.93
```

---

## Unity Implementation

The Unity project is located at:

```text
Unity/RV_Unity_ESP32/
```

The main scene is:

```text
Assets/Scenes/Cortina.unity
```

The serial communication interface is implemented in:

```text
Assets/Scripts/ESP32Controller.cs
```

The received values are applied to the Unity `Cloth` component using:

```csharp
targetCloth.externalAcceleration =
    new Vector3(windX, 0f, windZ);
```

This allows the physical potentiometers to modify both the direction and intensity of the external acceleration applied to the virtual curtain.

---

## Cloth Simulation

The curtain is implemented using Unity's `Cloth` physics component.

The upper row of vertices is constrained to maintain the curtain attached to its support, while the remaining vertices are allowed to move according to gravity and the externally applied acceleration.

The implemented wind is therefore an **interactive approximation based on external acceleration**. It should not be interpreted as a computational fluid dynamics model or as a direct representation of meteorological wind velocity.

---

## Experimental Tests

The system was evaluated under neutral, independent-axis, opposite-direction, and combined input conditions.

| Wind X | Wind Z | Test condition |
|---:|---:|---|
| -0.01 | -0.03 | Neutral |
| +9.97 | ≈0 | Intermediate +X |
| +20 | ≈0 | Maximum +X |
| -20 | ≈0 | Maximum -X |
| ≈0 | +20 | Maximum +Z |
| ≈0 | -20 | Maximum -Z |
| +15.06 | +14.93 | Combined X-Z |

The experiments verified:

- Independent control of both axes.
- Positive and negative directional control.
- Intermediate input values.
- Simultaneous X-Z operation.
- Real-time deformation of the virtual curtain.

---

## Virtual Reality

The final VR implementation uses:

- Unity XR Plug-in Management
- OpenXR
- SteamVR
- VRidge Free
- Xiaomi 11T

The smartphone provides stereoscopic visualization and head-orientation tracking.

The ESP32 remains an independent physical interface for controlling the wind applied to the curtain, resulting in two simultaneous interaction channels:

- **ESP32:** controls the simulated physical conditions.
- **Mobile VR device:** controls the observer's viewpoint.

No VR controllers are required for the implemented interaction.

---

## Running the Project

### 1. ESP32

Open:

```text
ESP32/wind_controller_for_unity/wind_controller_for_unity.ino
```

in Arduino IDE.

Configure the appropriate ESP32 board and upload the firmware.

The serial communication speed is:

```text
115200 baud
```

Verify that both potentiometers generate values throughout approximately the complete `[-20, 20]` interval.

> Close the Arduino Serial Monitor before starting the Unity simulation. The serial port should not be simultaneously occupied by Arduino IDE and Unity.

### 2. Unity

Open the following directory as a Unity project:

```text
Unity/RV_Unity_ESP32/
```

Open the scene:

```text
Assets/Scenes/Cortina.unity
```

Select the `ESP32Controller` GameObject and verify:

```text
Port Name    = COM4
Baud Rate    = 115200
Target Cloth = Curtain
```

The COM port may need to be changed depending on the computer being used.

Enter Play Mode and modify both potentiometers.

The curtain should respond in real time to the received X and Z components.

### 3. Virtual Reality

For the final implementation:

1. Enable OpenXR in Unity XR Plug-in Management.
2. Use SteamVR as the desktop VR runtime.
3. Connect the mobile device through VRidge.
4. Start the Unity application.
5. Verify stereoscopic visualization.
6. Verify that changes in device orientation modify the virtual camera orientation.

---

## Software

The project was developed using:

- Unity 6.6 (`6000.6.2f1`)
- Arduino IDE
- C/C++ for ESP32 firmware
- C# for Unity integration
- OpenXR
- SteamVR
- VRidge Free

---

## Notes

The Unity repository intentionally contains only the files required to reconstruct the project:

```text
Assets/
Packages/
ProjectSettings/
```

When the project is opened, Unity automatically regenerates directories such as `Library/` and `Temp/`.

Unity `.meta` files must remain in the repository because they preserve references between scenes, scripts, materials, and other assets.

---

## Academic Context

**Universidad Militar Nueva Granada**  
**Mechatronics Engineering**  
**Virtual Reality**  
**Academic Period:** 2026-2

### Author

**Santiago Pérez**  
Universidad Militar Nueva Granada