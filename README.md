# OTA Flowchart

## Run Properties

* -d targeted device
* -b binary to flash
* -i the interface device

## Running directly on Device

````mermaid
flowchart TD
    Start(Device boot)
    CkAvVer[Check if new version is available on server]
    Comp{New V Available}
    DownL[Download Binary to FS]
    Write[Write Bin to Boot Sector]
    Reboot[Reboot]
    CheckState{Booting}
    FailBack[Fail back to latest Stable]
    End[TaskFinished]
    Start --> CkAvVer
    CkAvVer --> Comp
    Comp -->|NO| End
    Comp -->|Yes| DownL
    DownL --> Write
    Write --> Reboot
    Reboot --> CheckState
    CheckState -->|No| FailBack
    CheckState -->|Yes| End
    FailBack --> End
    End -->|60 Min| Start
````

## Client running on External Device(Laptop)

````mermaid
flowchart TD
    Start(Stating Client)
    DevList[List all available devices on connection]
    SelectDev[Waiting on Device Selection]
    DeviceSelected{DeviceSelected}
    CkAvVer[Check if new version is available on server]
    Comp{New V Available}
    DownL[Download Binary to FS]
    Write[Flash new Version]
    Reboot[Reboot]
    CheckState{Booting}
    FailBack[Fail back to latest Stable]
    End[TaskFinished]
    Start --> DevList
    DevList --> SelectDev
    SelectDev --> DeviceSelected
    DeviceSelected -->|No| DevList
    DeviceSelected -->|Yes| CkAvVer
    CkAvVer --> Comp
    Comp -->|NO| End
    Comp -->|Yes| DownL
    DownL --> Write
    Write --> Reboot
    Reboot --> CheckState
    CheckState -->|No| FailBack
    CheckState -->|Yes| End
    FailBack --> End
    End --> DevList
````