# P8_duaro

## Table of contents

- [Getting Started](#Purpose-Started)
- [Project Details](#Project-Details)
- [Authors](#Authors)
- [References](#References)

## Getting Started: 

### Unity

This project is developed and tested with Unity Version 2020.3.29f1. Install the version through the Unity Hub from here: https://unity3d.com/de/get-unity/download/archive.

### Create a workspace and clone the repository 

Clone the repository `git clone https://github.com/AndreiVoica/P8_duaro.git` and `$ catkin_make`.

### Open the Unity Project

Open [Unity_env](/Unity_env) in your Unity Hub.

### Installing the URDF plugin

1. Open the Package Manager from Unity Menu. Click `Window -> Package Manager`. A new package manager window will appear.

2. Click on the `+` sign on the top left corner of the package manager window and click on `Add Package from Git URL`. 

<img src = "images/Package_manager_add.png">

3. Enter the git URL for the URDF Importer with the latest version tag (currently v0.5.2) `https://github.com/Unity-Technologies/URDF-Importer.git?path=/com.unity.robotics.urdf-importer#v0.5.2` in the text box and press `Enter`.

For more information follow this link: https://github.com/Unity-Technologies/URDF-Importer

### Installing ROS-TCP-Connector plugin

1. Create or open a Unity project.

2. In the Package Manager window, find and click the `+` button in the upper lefthand corner of the window. Select `Add package from git URL...`.

    ![](/images/packman.png)
3. Enter `https://github.com/Unity-Technologies/ROS-TCP-Connector.git?path=/com.unity.robotics.ros-tcp-connector`.


### Setup TCP connection on your terminal (Unity-ROS)

It's easier to just follow this link --> https://github.com/Unity-Technologies/Unity-Robotics-Hub/blob/main/tutorials/ros_unity_integration/setup.md

### ML-agents

Follow this tutorial to install ML-agents --> https://alexisrozhkov.github.io/unity_rl/
Make sure the venv is running python 3.6 (or 3.7)

Follow this tutorial to setup ML-agents in Unity --> https://youtu.be/zPFU30tbyKs?t=611

### ROS-Unity connection

1. Under `Robotics` tab go to `ROS settings` and change the IP to `127.0.0.1` Port: `10000`.

    <img src = "images/rosconnection.png">
2. In the Unity scene, go to the `Ros_connection` game object and change the IP to `127.0.0.1`.

    <img src = "images/unityrosconnection.png">
    
### Build msgs in Unity

1. Under `Robotics` tab go to `Generate ROS messages`.
2. Press `Browse` add select your `src` folder location. 

    ![](/images/browse.png)
    
3. Build all the msgs, srvs and actions as shown here:

    ![](/images/buildmsgs.png)
    

### Launch Duaro

```bash
roslaunch khi_duaro_moveit_config demo.launch
```
### Launch TCP

```bash
roslaunch ros_tcp_endpoint endpoint.launch
```


## Project Details: 



## Authors:

### Who do I talk to? ###

Group number: 865

Group members e-mail:
* Adshya Vasudavan Iyer: avasud11@student.aau.dk
* Andrei Voica: avoica18@student.aau.dk
* Daniel Moreno París: dmoren21@student.aau.dk
* Lekshmi Jayakrishnan: ljayak18@student.aau.dk
* Sabrina Kern: skern21@student.aau.dk
