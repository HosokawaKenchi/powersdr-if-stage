Google Group for PowerSDR/IF Stage:

http://groups.google.com/group/powersdr-if-stage?hl=en&pli=1

PowerSDR/IF Stage is a modified version of FlexRadio Systems PowerSDR software. It is intended for use with Software Defined Radio (SDR) receivers, such as the Softrock, that are being used as an Intermediate Frequency (IF) stage with a radio that is capable of being computer controlled (via C.A.T. commands). The PowerSDR/IF Stage software now communicates with the Ham Radio Deluxe software to control and keep in sync with the radio being used. It also now supports direct serial connections to some radio models.

Features of the current version include:

  * Full tuning control from the PowerSDR/IF Stage software. You can change bands, modes, frequency, click to tune from the panadapter, etc. as usual from the PowerSDR software and the tuning updates will be tracked by your radio in real time
  * Full tuning control from your radio. You can spin the VFO, change modes, etc. and the tuning updates will be tracked by the PowerSDR/IF Stage software
  * PowerSDR/IF Stage automatically mutes audio output when the radio goes  into TX mode. This allows you to still see the panadapter while in TX mode - unlike using the mute line on the Softrock lite kit to mute when going into  TX mode. This feature can be disabled by clicking the "MON" button
  * New menus added to the PowerSDR setup menu for Softrock IF Stage use
    * You can select the IF frequency by mode
    * You can offset the radio's VFO frequency from the PowerSDR/IF Stage's VFO frequency by mode
    * You can enter the frequency range of the external radio
  * PowerSDR/IF Stage window title indicates the model of the radio its connected to (ex. "WU2X PowerSDR/TS-940S IF Stage")
  * Additional soundcard support - larger buffer size supported and I/Q sample offset correction

PowerSDR/IF Stage is currently under development by Scott McClements, WU2X, Chad Gatesman, W1CEG.


We are working on a new version of PowerSDR/IF Stage that will be based on the "Pretty Betty" branch of PowerSDR. We have implemented a native serial connection directly from PowerSDR to the rig (Kenwood, Elecraft, Yaesu). This allows for near real time updates from the radio. It will also allow us to expand the number of features we can provide, since we will now have direct control over the communication with the rig. Communication is optimized for use with PowerSDR/IF.

Another area we are focused on is keeping our code extensions out of the base PowerSDR source code as much as possible. This will allow us to quickly merge up the PowerSDR/IF code into future base PowerSDR code without much, if any, source code rework. On the flip side, this means we will not focus on trying to disable, remove or modify any GUI control that is not used. The reason for that is because the more we modify these items, the harder it is for us to sync our PowerSDR/IF code up with newer PowerSDR versions.


**Added Features:**

  * Direct serial port CAT Communications to Transceiver (Kenwood/Elecraft)
  * New VFO-B Control
    * VFO-B Control via Ctrl+MouseWheel or MouseWheel while cross hairs are Red
    * VFO-B Control via RIT Knob on Kenwood TS-940S when RIT and XIT not in use
    * VFO-B Control via VFO-B on K3
  * Separate Setup Dialog and Configuration File for IF Stage Settings
  * Support for TX Metering from External Power/SWR Meters (Array Solutions PowerMaster)
  * MOX Status in PowerSDR
  * RIT Controls synced to Transceiver
  * RX Meter Level Calibration Works


![http://www.chadgatesman.com/w1ceg/PowerSDR-IFStage.jpg](http://www.chadgatesman.com/w1ceg/PowerSDR-IFStage.jpg)