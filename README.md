# ESP32-Unity VR Curtain Simulation

Interactive virtual reality project developed for the Virtual Reality
course of the Mechatronics Engineering program at Universidad Militar
Nueva Granada.

The project integrates an ESP32 microcontroller with Unity to control
the physical simulation of a virtual curtain in real time.

## System Architecture

Potentiometers -> ESP32 -> Moving Average Filter -> Mapping [-20, 20]
-> USB Serial -> Unity -> Cloth External Acceleration
-> OpenXR -> SteamVR / VRidge -> Mobile VR Display

## Repository Structure

ESP32-Unity-VR-Curtain/

- ESP32/
- Unity/
- Documentation/
  - Images/
  - Report/
- .gitignore
- README.md

## ESP32

The ESP32 directory contains the Arduino IDE firmware used for:

- Analog acquisition from two potentiometers.
- Five-sample moving-average filtering.
- Mapping of the input signals to the range [-20, 20].
- Serial communication at 115200 baud.

The two control channels correspond to:

- GPIO32 -> Wind X
- GPIO33 -> Wind Z

## Unity

The Unity directory contains the Unity project implementing:

- Virtual environment.
- Cloth simulation.
- Serial communication with the ESP32.
- Real-time external acceleration control.
- OpenXR integration.
- VR visualization.

## Virtual Reality

The final VR implementation uses:

- Unity XR Plug-in Management
- OpenXR
- SteamVR
- VRidge Free
- Xiaomi 11T

## Documentation

The Documentation directory contains supporting diagrams, images,
experimental evidence and report-related material.

## Academic Context

Universidad Militar Nueva Granada

Mechatronics Engineering

Virtual Reality

Academic period: 2026-2
