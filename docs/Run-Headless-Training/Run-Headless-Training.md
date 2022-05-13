# Training on a server
## Steps to follow
- [Create an executable environment in Unity](#Create-an-executable-environment-in-Unity)
- [Train headless](#Train-headless)
- [Running on Strato ](#Running-on-Strato)

## Create an executable environment in Unity
The tutorial to create an executable environment is on this [link](https://github.com/Unity-Technologies/ml-agents/blob/main/docs/Learning-Environment-Executable.md).

### How to create the executable file
1. Open Player Settings:
- At the top menu: `Edit` > `Project Settings` > `Player`.
- Under Resolution and Presentation:
    - Ensure that `Run in Background` is Checked.
    - Ensure that `Display Resolution Dialog` is set to Disabled. (Note: this setting may not be available in newer versions of the editor.)
2. Open the Build Settings window:
- At the top menu: `File` > `Build Settings`.
3. Choose the target platform.
- (optional) Select `Development Build` to log debug messages.
4. If any scenes are shown in the Scenes in Build list, make sure that your scene is the only one checked. (If the list is empty, then only the current scene is included in the build).
5. Click `Build`:
- In the `File` dialog, navigate to your ML-Agents directory.
- Assign a file name and click `Save`.

### Launch the executable file
- You can do it by just pressing on the executable file.
- It is possible to use the [Python API](https://github.com/Unity-Technologies/ml-agents/blob/main/docs/Python-API.md) to launch it:
```
from mlagents_envs.environment import UnityEnvironment
env = UnityEnvironment(file_name=<env_name>)
```
### Run the enviroment to train
- Select the algorithm from `config/` directory.
- In the flag `--env` add the path to the executable file (It is not necessary to add the file extension).
- Launch the following command:
`mlagents-learn config/ppo/duaro-test-ppo.yaml --env=Build/test-approx --run-id=firstHeadlessRun`

### Launch TensorBoard
1. From the another command terminal run: `tensorboard --logdir results --port 6006`
2. Open a browser window and navigate to [localhost:6006](localhost:6006).

[Using TensorBoard to Observe Training Tutorial](https://github.com/Unity-Technologies/ml-agents/blob/main/docs/Using-Tensorboard.md)

## Train headless
There are 2 options to train headless:
1. Add the `--no-graphics` when running the training, i.e:
`mlagents-learn config/ppo/duaro-test-ppo.yaml --env=Build/test-duaro --run-id=firstRun --no-graphics`

2. Build the Unity executable with `Server Build` checked. You can find this setting in Build Settings in the Unity Editor.
`mlagents-learn config/ppo/duaro-test-ppo.yaml --env=Build/test-duaro --run-id=firstRun `

## Running on Strato
1. Log in here: [AAU Strato Login](https://strato-new.claaudia.aau.dk)
2. Follow the [Quick Start](https://www.strato-docs.claaudia.aau.dk/guides/quick-start/) and create a Ubunutu 20.4 Instance. 
3. Log in to the Instance that you will work on. First, check the IP address (10.92.0.xxx) at Strato --> Compute --> Instances. Then log in with the appropriate key (strato_33.pem for example):
```bash
ssh ubuntu@10.92.0.33 -i P8_duaro/Strato/Key/strato_33.pem
```
If you have an error saying that the public key is denied, run this first:
```bash
chmod 600 strato_33.pem
```
4. Install mlagents as described in [Getting Started](../../docs/GettingStarted/GettingStarted.md) 
    - create an virtual environment `mlagents_env` with conda (install conda first, for example like [this](https://www.rosehosting.com/blog/how-to-install-anaconda-on-ubuntu-20-04/))
    - install pyhthon 3.7 
    ```bash
    $ conda install python=3.7 anaconda=custom
    ```
    - install mlagents
    ```bash
    $ python -m pip install mlagents==0.28.0
    ```    
5. Create a remote folder for sharing files and directories  
```bash
(mlagents_env) ubuntu@ubuntu:~$ mkdir remotefolder
```
6. Copy the Unity_env folder to the remotefolder:
```bash
(mlagents_env) sabrina@sabrina-ThinkPad-T490s:~/P8_duaro$ scp -i ~/P8_duaro/Strato/Key/strato_33.pem -r Unity_env ubuntu@10.92.0.33:~/remotefolder
```
To delete it, when neccessary:
```bash
$ rm -rf Unity_env
```
7. Start Training:
```bash
(mlagents_env) ubuntu@ubuntu:~/remotefolder/Unity_env$ mlagents-learn config/complex2.yaml --env=exe-env/env4/env4 --run-id=env4_cloudcpu_complex2_1 --n-envs=8 --no-graphics

```
8. Create a shared directory to get access to Tensorboard:
- First, install sshfs, for example with conda in your virtual environment
```bash
$ conda install -c conda-forge sshfs
```
- Then, run this:
```bash
(mlagents_env) sabrina@sabrina-ThinkPad-T490s:~$ mkdir remoteDir

(mlagents_env) sabrina@sabrina-ThinkPad-T490s:~$ sudo sshfs -o allow_other -o nonempty -o IdentityFile=~/P8_duaro/Strato/Key/strato_33.pem  ubuntu@10.92.0.33:remotefolder ~/remoteDir

(mlagents_env) sabrina@sabrina-ThinkPad-T490s:~$ tensorboard --logdir remoteDir/Unity_env/results --port 6006
```
### Saving Data 

* Remember to manually copy the Unity_env into to the remotefolder after every change
* If you want to save the results folder, those also have to copied manually to the git repo

### Restart a Training / Continously Training

* If you want to quite the training with <kbd>Ctrl</kbd> + <kbd>c</kbd>: Add `--resume`, for example:
```bash
(mlagents_env) ubuntu@ubuntu:~/remotefolder/Unity_env/results$ mlagents-learn config/DuaroAgentComplex_servertest.yaml --env=Build/Build/ComplexScene_servertest.x86_64 --run-id=test-8instances-2 --num-envs=8 --no-graphics --resume
```
* If you want to end the SSH connection and log out but continue running the training on the server:
    - open the ssh connnection
    - run `tmux` and run your training in the new terminal that opens
    - before ending the session type <kbd>Ctrl</kbd> + <kbd>b</kbd> and then <kbd>d</kbd>. This will detach the session
    - To reopen the session: open the ssh connection and type `tmux attach`
* For enable scrolling in the tmux terminal: type <kbd>Ctrl</kbd> + <kbd>b</kbd> and then <kbd>:</kbd>, then type `set mouse on`
* Find more useful commands for tmux [here](https://tmuxcheatsheet.com/)


### Overview of Instances
| Creator  | IP Adress | Key |
| ------------- | ------------- | ------------- |
| Andrei | 10.92.0.247  | strato_247.pem |
| Sabrina  | 10.92.0.33  | strato_33.pem |
| Daniel | 10.92.0.253  | strato_253.pem |
| Lekshmi | 10.92.0.151  | strato_151.pem |


### Naming of Trainings
* Name of environment (env1, env2,...)
* Where it was trained (localgpu, cloudcpu,...) and number of instances 
* config file (complex2, complex1, simple,...)
* ongoing number

Example: env1_cloudcpu8_complex2_1

### Use VPN to get access from home
1. Check if Java is installed with `java -version`. If not, install it: `$ sudo apt install default-jre`
2. Log in to this [link](https://ssl-vpn1.aau.dk) with you AAU Account and click on Download
3. Go to the folder where the file was downloaded and run the following command (with the downloaded file chosen):
```bash
sudo bash anyconnect-linux64-4.10.05085-core-vpn-webdeploy-k9.sh 
```
4. Launch the Cisco client: `$ /opt/cisco/anyconnect/bin/vpnui`
5. Connect to: `ssl-vpn1.aau.dk` and log in
