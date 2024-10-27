# Audio Manager Application

## Overview
The Audio Manager Application is a Windows Forms application that allows users to manage audio devices and route audio from specific applications to selected output devices. Built using C# and the NAudio library, this application aims to provide an intuitive interface for controlling audio sessions and devices on your system.

## Features
- **Audio Device Management**: View and select available audio output devices.
- **Application Audio Routing**: Route audio output from running applications to the selected audio device.
- **Profile Saving**: Save and load profiles to quickly switch audio configurations.

## Technologies Used
- C#
- Windows Forms
- NAudio library

## Installation
To run this application, follow these steps:

1. **Clone the Repository**
   ```bash
   git clone https://github.com/your-username/your-repo-name.git
   cd your-repo-name
   ```

2. **Open the Solution**
   - Open the solution file (`.sln`) in Visual Studio.

3. **Install Dependencies**
   - Ensure that the NAudio library is installed. You can install it via NuGet Package Manager:
     ```bash
     Install-Package NAudio
     ```

4. **Build and Run**
   - Build the project in Visual Studio and run the application.

## Usage
1. Launch the application.
2. Select the desired audio output device from the dropdown menu.
3. Check the applications you want to route audio from in the list.
4. Click the "Apply" button to route the audio to the selected device.
5. Save your current configuration as a profile for easy access later.

## Contributing
Contributions are welcome! If you have suggestions or improvements, please fork the repository and submit a pull request.

## License
This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## Acknowledgments
- Thanks to the creators of the NAudio library for their excellent work in audio manipulation in .NET.
