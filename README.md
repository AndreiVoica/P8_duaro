# P8_duaro

# Installing the URDF plugin:

1. Open the Package Manager from Unity Menu. Click `Window -> Package Manager`. A new package manager window will appear.

2. Click on the `+` sign on the top left corner of the package manager window and click on `Add Package from Git URL`. 

<img src = "images/Package_manager_add.png">

3. Enter the git URL for the URDF Importer with the latest version tag (currently v0.5.2) `https://github.com/Unity-Technologies/URDF-Importer.git?path=/com.unity.robotics.urdf-importer#v0.5.2` in the text box and press `Enter`.

4. Click `Import URDF`.

For more info follow this link --> https://github.com/Unity-Technologies/URDF-Importer

_______________________________________________________________________________________________________________________________________________________

# Installing ROS-TCP-Connector plugin

1. Create or open a Unity project.

1. In the Package Manager window, find and click the `+` button in the upper lefthand corner of the window. Select `Add package from git URL...`.

    ![](/images/packman.png)
1. Enter `https://github.com/Unity-Technologies/ROS-TCP-Connector.git?path=/com.unity.robotics.ros-tcp-connector`.

1. Click `Add`.

# Start TCP connection on your terminal

Open a new terminal, navigate to your Catkin workspace, and run:

   ```bash
   source devel/setup.bash
   roslaunch ros_tcp_endpoint endpoint.launch
   ```
