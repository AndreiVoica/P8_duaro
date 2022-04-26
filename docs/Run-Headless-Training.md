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

## Build on Docker
## Run on the cloud GPU
Getting Started: 
1. Log in here: [AAU Strato Login](https://strato-new.claaudia.aau.dk)
2. Follow the [Quick Start](https://www.strato-docs.claaudia.aau.dk/guides/quick-start/) and create a Ubunutu 20.4 Instance
