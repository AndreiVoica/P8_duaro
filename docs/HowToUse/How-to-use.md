# How to use guide

## Trying different RL models

1. In the unity project, go to `Hierarchy` in the left side, click on the arrow next to `Training area` and then select `DuaroAgent`

<img src = "../../images/Hierarchy.png" width="400">

2. Once the `DuaroAgent` is selected as presented in above image, you can go to the inspector on the right side of the screen and scroll down until where you see 2 components called `Behavior Parameters` and `Duaro Agent Complex WT`. 

<img src = "../../images/Inspector.png" width="400">

3. To try different models, make sure that Inference Device and Behavior Type are both set to `Default`.

4. Navigate to the Project window and select the folder named `TFModels`. This folder contains different trained models that can be applied to the Duaro robot. 

<img src = "../../images/TFModels.png" width="400">

5. To apply a model to the robot, click and drop the desired model in the `Behavior Parameters` under `Model`.
