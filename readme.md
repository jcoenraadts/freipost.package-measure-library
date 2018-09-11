Freipost Package Measure Library
================================

This project consists of two `.NET` projects:
* `Freipost.DemoGui`
* `Freipost.PackageMeasureLibrary`

# Library
The library project consists of a single class `Device.cs` which represents the hardware device connected via USB.
This library supports the following functions:
* Tare and reset the scales
* Calibrate the scales
* Set the machine dimensions
* Measure package dimensions and mass

## Basic Usage
```C#
// create the objects
string comport = "COM6";
Device device = new Device();

// connect the device
device.connect(comport);

// set the dimensions of the machine
int xDimension = 800; //mm
int yDimension = 800; //mm
int zDimension = 600; //mm

device.SetMaxDimensions(xDimension, yDimension, zDimension);

// tare the scales
device.Tare();

// read mass and dimensions
DeviceReading reading = device.Read();
```

## Scale Calibration
The calibration API exposes `DoOffsetCalibration` and `DoMultiplierCalibration`. The offset must always be calibrated first, and with nothing on the scales.
```C#
// create the objects
string comport = "COM6";
Device device = new Device();

// connect the device
device.connect(comport);

// == Alert the user to remove anything from the scales ==
// calibrate the offset - this will take a few seconds due to averaging
device.DoOffsetCalibration();

// == Alert the user to place a reference mass on the scales ==
// calibrate the offset - this will take a few seconds due to averaging
int referenceMassInGrams = 1000 * 10;  // for a 10kg reference
device.DoMultiplierCalibration(referenceMassInGrams);
```