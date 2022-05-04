# Run Headless Training
## Steps to follow
- [Create an executable environment in Unity](#Create-an-executable-environment-in-Unity)
- [Train headless](#Train-headless)
- [Build on Docker](#Build-onDocker)
- [Run on the cloud GPU](#Run-on-the-cloud-GPU)

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
4. Install mlagents as described in [Getting Started](../docs/GettingStarted/GettingStarted.md) 
    - create an virtual environment with conda
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
(mlagents_env) sabrina@sabrina-ThinkPad-T490s:~/P8_duaro$ scp -i ~/P8_duaro/Strato/Key/strato_33.pem -r Unity_env ubuntu@10.92.0.124:~/remotefolder
```
7. Start Training:
```bash
(mlagents_env) ubuntu@ubuntu:~/remotefolder/Unity_env$ mlagents-learn config/DuaroAgentComplex_servertest.yaml --env=exe-env/env1/ComplexScene_servertest --run-id=cpu-test-4 --no-graphics
```
8. Create a shared directory to get access to Tensorboard:
```bash
(mlagents_env) sabrina@sabrina-ThinkPad-T490s:~$ sudo sshfs -o allow_other -o nonempty -o IdentityFile=~/P8_duaro/Strato/Key/strato_33.pem  ubuntu@10.92.0.124:remotefolder ~/remoteDir

(mlagents_env) sabrina@sabrina-ThinkPad-T490s:~$ tensorboard --logdir remoteDir/Unity_env/results --port 6006
```
## Run on AI Cloud

