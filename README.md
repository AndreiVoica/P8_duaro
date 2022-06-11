# P8_duaro

## Table of contents:

- [Purpose of Project](#Purpose-Started)
- [Project Details](#Project-Details)
- [Authors](#Authors)

## Purpose of project:

This project elaborates the use of Reinforcement Learning (RL) for the optimisation of sequences for a disassembly task with a dual-arm robot system. A digital model of the dual-arm Kawasaki duAro1 robot was created in Unity. The model was used in combination with ROS and the ROS package MoveIt to record the skills (steps) needed for a disassembly sequence. The order of the skills was then trained using Proximal Policy Optimisation (PPO). The trained model was sent to the physical robot and evaluated. The results show that it is possible to find an optimal sequence for a disassembly task using RL.

The following image shows the digital model of the robot in the Unity Environment, performing a skill with the lower arm. 

![image info](/images/greenSkill.png)

The following image shows the setup of the physical robot in the laboratory.

![image info](/images/physical_setup.jpeg)

In the following video, the trained model is performed by the digital robot, showing an optimal sequence obtained through training.

![](/videos/final_sequence.gif)

In the following video, another obtained sequence is performed on the physical robot. It can be seen that the grippers are not working properly, because of the issues described in the report. However, it can be seen that the sequence of skills is performed accurate.

![](/videos/physical_robot_test.gif)

## Project Details: 

This repository contains additional information on implementation in addition to the report.

* How to get started - [Setting up Unity and ML-Agents](docs/GettingStarted/GettingStarted.md)
* How to train on a server - [Run and train headless](docs/Run-Headless-Training/Run-Headless-Training.md)
* How to tune - [Important hyperparameters](/Unity_env/config/README.md)
* How to run the project - [Run the model](/docs/HowToUse/How-to-use.md)

## Authors:

### Who do I talk to? ###

Group number: 865

Group members e-mail:
* Adshya Vasudavan Iyer: avasud11@student.aau.dk
* Andrei Voica: avoica18@student.aau.dk
* Daniel Moreno París: dmoren21@student.aau.dk
* Lekshmi Jayakrishnan: ljayak18@student.aau.dk
* Sabrina Kern: skern21@student.aau.dk
