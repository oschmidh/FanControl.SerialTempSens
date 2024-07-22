# FanControl.SerialTempSens
![build](https://github.com/oschmidh/FanControl.SerialTempSens/actions/workflows/build.yaml/badge.svg?branch=main)

[FanControl](https://getfancontrol.com/) plugin for the SerialTempSens Project.

This plugin allows using the three NTC-sensors of the SerialTempSens as sensor inputs in FanControl.

No manual configuration is required, device serial port and available sensors are automatically detected upon startup.

## Installing the plugin

First, download the latest package (FanControl.SerialTempSens_vX.X.X.zip) from the releases page.
Then, open FanControl and go to `Settings`, click on `Install plugin...` and select the downloaded .zip package.

Alternatively, the downloaded .zip package can also be manually unpacked into the `Plugins`-directory of the FanControl installation.

> [!NOTE]
> If the plugin is not detected, it might be required to manually "unlock" the downloaded dlls in Windows.
> For this, right click the dll, select `Properties`.
> In the `Attribute` section of the `General` tab, there should be a checkbox to allow execution of this dll.
> This step needs to be repeated for every dll in the .zip package.

## Building locally

To compile the solution locally, protoc (protbuf compiler) has to be available on PATH
