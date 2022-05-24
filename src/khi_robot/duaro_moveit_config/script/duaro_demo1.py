#!/usr/bin/env python3

from subprocess import list2cmdline
from xml.etree.ElementTree import ElementTree
import rospy
import tty
import sys
import csv
import pandas as pd
import numpy as np
import time


from moveit_commander import MoveGroupCommander

df = pd.read_csv("script/finalseq20.csv", usecols=[1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12])
df_ = df.astype(float)
print("test: ", df_)

if __name__ == '__main__':

    #init_node() [L1, L2, L3, L4, U1, U2, U3, U4]
    group = MoveGroupCommander("botharms")
    sleep_time = 0.5
    exec_vel = 1.0
    
    elements = len(df_.values)
    print("elements = ", elements)
    #for currentIndex in list_of_csv:
    #    currentIndex += 1
    #    time.sleep(0.1)
        
    
    #Home position
    def home_pos():
        group.set_max_velocity_scaling_factor(exec_vel)
        currentIndex = 1
        elements = len(df_.values)
        #while currentIndex >= elements:
        for x in df_.values:
            currentIndex += 10                                              ##L1, L2, L3, L4, GR, GL
            print("sTest = ", currentIndex)                                 ##U1, U2, U3, U4, GR, GL
            group.set_joint_value_target([df_.values[currentIndex-1, 4], df_.values[currentIndex-1, 5], df_.values[currentIndex-1, 6], df_.values[currentIndex-1, 7], df_.values[currentIndex-1, 8], df_.values[currentIndex-1, 9], 
                                          df_.values[currentIndex-1, 0], df_.values[currentIndex-1, 1], df_.values[currentIndex-1, 2], df_.values[currentIndex-1, 3], df_.values[currentIndex-1, 10], df_.values[currentIndex-1, 11]]) #[-0.78, 0.78, 0.14, 0.0, 0.78, -0.78, 0.14, 0.0]
            #group.set_joint_value_target([0.74, -0.84, 0.149, -0.036, -0.01, 0.01, -0.80, 0.82, 0.13, 0, -0.01, 0.01])
            group.go()
            #rospy.sleep(sleep_time)
            
        
        #currentIndex += 1
        
    def test():
        group.set_max_velocity_scaling_factor(exec_vel)
        currentIndex = 1
        elements = len(df_.values)
        #while currentIndex >= elements:
        for x in df_.values:
            currentIndex += 5
            print("sTest = ", currentIndex)
            group.set_joint_value_target([0.74, -0.84, 0.149, df_.values[currentIndex-1, 7], -0.01, 0.01, -0.80, 0.82, 0.13, df_.values[currentIndex-1, 3], -0.01, 0.01])
            group.go()   

    def upper_grip_green():
        group.set_max_velocity_scaling_factor(exec_vel)
        group.set_joint_value_target([-0.78, 0.78, 0.15, 0.0, -0.055, 0.055, 0.78, -0.78, 0.15, 0.0, -0.055, 0.055]) # home pos
        group.go()
        rospy.sleep(sleep_time)
        group.set_max_velocity_scaling_factor(exec_vel)
        group.set_joint_value_target([-1.08, 1.64, 0.15, 1.08, -0.055, 0.055, 0.78, -0.78, 0.15, 0.0, -0.055, 0.055]) #over the cube
        group.go()
        rospy.sleep(sleep_time)
        group.set_max_velocity_scaling_factor(exec_vel)
        group.set_joint_value_target([-1.08, 1.64, 0.093, 1.08, -0.055, 0.055, 0.78, -0.78, 0.15, 0.0, -0.055, 0.055]) #down to cube
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-1.08, 1.64, 0.093, 1.08, -0.016, 0.030, 0.78, -0.78, 0.15, 0.0, -0.055, 0.055]) #grab cube
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-1.08, 1.64, 0.15, 1.08, -0.016, 0.030, 0.78, -0.78, 0.15, 0.0, -0.055, 0.055]) #up
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-1.08, 0.59, 0.15, 2.11, -0.016, 0.030, 0.78, -0.78, 0.15, 0.0, -0.055, 0.055]) #over drop location
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-1.08, 0.59, 0.01, 2.11, -0.016, 0.030, 0.78, -0.78, 0.15, 0.0, -0.055, 0.055]) #drop location
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-1.08, 0.59, 0.01, 2.11, -0.055, 0.055, 0.78, -0.78, 0.15, 0.0, -0.055, 0.055]) #open grip
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-1.08, 0.59, 0.15, 2.11, -0.055, 0.055, 0.78, -0.78, 0.15, 0.0, -0.055, 0.055]) #open grip
        group.go()
        rospy.sleep(sleep_time)

    def lower_grip_green():
        group.set_max_velocity_scaling_factor(exec_vel)
        group.set_joint_value_target([-0.78, 0.78, 0.15, 0.0, -0.055, 0.055, 0.78, -0.78, 0.15, 0.0, -0.055, 0.055]) # home pos
        group.go()
        rospy.sleep(sleep_time)
        group.set_max_velocity_scaling_factor(exec_vel)
        group.set_joint_value_target([-0.78, 0.78, 0.15, 0.0, -0.055, 0.055, 0.75, -1.64, 0.15, -0.645, -0.055, 0.055]) #over the cube
        group.go()
        rospy.sleep(sleep_time)
        group.set_max_velocity_scaling_factor(exec_vel)
        group.set_joint_value_target([-0.78, 0.78, 0.15, 0.0, -0.055, 0.055, 0.75, -1.64, 0.09, -0.645, -0.055, 0.055]) #down to cube
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-0.78, 0.78, 0.15, 0.0, -0.055, 0.055, 0.75, -1.64, 0.09, -0.645, -0.016, 0.030]) #grab cube
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-0.78, 0.78, 0.15, 0.0, -0.055, 0.055, 0.75, -1.64, 0.15, -0.645, -0.016, 0.030]) #up
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-0.78, 0.78, 0.15, 0.0, -0.055, 0.055, 0.75, -0.05, 0.15, -2.18, -0.016, 0.030]) #over drop location
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-0.78, 0.78, 0.15, 0.0, -0.055, 0.055, 0.75, -0.05, 0.01, -2.18, -0.016, 0.030]) #drop location
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-0.78, 0.78, 0.15, 0.0, -0.055, 0.055, 0.75, -0.05, 0.01, -2.18, -0.055, 0.055]) #open grip
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-0.78, 0.78, 0.15, 0.0, -0.055, 0.055, 0.75, -0.05, 0.15, -2.18, -0.055, 0.055]) #up
        group.go()
        rospy.sleep(sleep_time)

    def lower_grip_black():
        group.set_max_velocity_scaling_factor(exec_vel)
        group.set_joint_value_target([-0.78, 0.78, 0.15, 0.0, -0.055, 0.055, 0.78, -0.78, 0.15, 0.0, -0.055, 0.055]) # home pos
        group.go()
        rospy.sleep(sleep_time)
        group.set_max_velocity_scaling_factor(exec_vel)
        group.set_joint_value_target([-1.1, 1.588, 0.15, 1.082, -0.055, 0.055, 0.78, -0.78, 0.15, 0.0, -0.055, 0.055]) #over the cube
        group.go()
        rospy.sleep(sleep_time)
        group.set_max_velocity_scaling_factor(exec_vel)
        group.set_joint_value_target([-1.1, 1.588, 0.03, 1.082, -0.055, 0.055, 0.78, -0.78, 0.15, 0.0, -0.055, 0.055]) #down to cube
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-1.1, 1.588, 0.03, 1.082, -0.016, 0.030, 0.78, -0.78, 0.15, 0.0, -0.055, 0.055]) #grab cube
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-1.1, 1.588, 0.15, 1.082, -0.016, 0.030, 0.78, -0.78, 0.15, 0.0, -0.055, 0.055]) #up
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-1.1, 0.296, 0.15, 2.548, -0.016, 0.030, 0.78, -0.78, 0.15, 0.0, -0.055, 0.055]) #over drop location
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-1.1, 0.296, 0.01, 2.548, -0.016, 0.030, 0.78, -0.78, 0.15, 0.0, -0.055, 0.055]) #drop location
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-1.1, 0.296, 0.01, 2.548, -0.055, 0.055, 0.78, -0.78, 0.15, 0.0, -0.055, 0.055]) #open grip
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-1.1, 0.296, 0.15, 2.548, -0.055, 0.055, 0.78, -0.78, 0.15, 0.0, -0.055, 0.055]) #up
        group.go()
        rospy.sleep(sleep_time)

    def upper_grip_black():
        group.set_max_velocity_scaling_factor(exec_vel)
        group.set_joint_value_target([-0.78, 0.78, 0.15, 0.0, -0.055, 0.055, 0.78, -0.78, 0.15, 0.0, -0.055, 0.055]) # home pos
        group.go()
        rospy.sleep(sleep_time)
        group.set_max_velocity_scaling_factor(exec_vel)
        group.set_joint_value_target([-0.78, 0.78, 0.15, 0.0, -0.055, 0.055, 0.628, -1.605, 0.15, -0.698, -0.055, 0.055]) #over the cube
        group.go()
        rospy.sleep(sleep_time)
        group.set_max_velocity_scaling_factor(exec_vel)
        group.set_joint_value_target([-0.78, 0.78, 0.15, 0.0, -0.055, 0.055, 0.628, -1.605, 0.02, -0.698, -0.055, 0.055]) #down to cube
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-0.78, 0.78, 0.15, 0.0, -0.055, 0.055, 0.628, -1.605, 0.02, -0.698, -0.016, 0.030]) #grab cube
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-0.78, 0.78, 0.15, 0.0, -0.055, 0.055, 0.628, -1.605, 0.15, -0.698, -0.016, 0.030]) #up
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-0.78, 0.78, 0.15, 0.0, -0.055, 0.055, 1.274, -0.418, 0.15, 0.453, -0.016, 0.030]) #over drop location
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-0.78, 0.78, 0.15, 0.0, -0.055, 0.055, 1.274, -0.418, 0.02, 0.453, -0.016, 0.030]) #drop location
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-0.78, 0.78, 0.15, 0.0, -0.055, 0.055, 1.274, -0.418, 0.02, 0.453, -0.055, 0.055]) #open grip
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-0.78, 0.78, 0.15, 0.0, -0.055, 0.055, 1.274, -0.418, 0.15, 0.453, -0.055, 0.055]) #up
        group.go()
        rospy.sleep(sleep_time)

    def lower_grip_blue():
        group.set_max_velocity_scaling_factor(exec_vel)
        group.set_joint_value_target([-0.78, 0.78, 0.15, 0.0, -0.055, 0.055, 0.78, -0.78, 0.15, 0.0, -0.055, 0.055]) # home pos
        group.go()
        rospy.sleep(sleep_time)
        group.set_max_velocity_scaling_factor(exec_vel)
        group.set_joint_value_target([-0.96, 1.64, 0.15, 0.872, -0.055, 0.055, 0.78, -0.78, 0.15, 0.0, -0.055, 0.055]) #over the cube
        group.go()
        rospy.sleep(sleep_time)
        group.set_max_velocity_scaling_factor(exec_vel)
        group.set_joint_value_target([-0.96, 1.64, 0.05, 0.872, -0.055, 0.055, 0.78, -0.78, 0.15, 0.0, -0.055, 0.055]) #down to cube
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-0.96, 1.64, 0.05, 0.872, -0.016, 0.030, 0.78, -0.78, 0.15, 0.0, -0.055, 0.055]) #grab cube
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-0.96, 1.64, 0.15, 0.872, -0.016, 0.030, 0.78, -0.78, 0.15, 0.0, -0.055, 0.055]) #up
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-0.488, -0.104, 0.15, 0.628, -0.016, 0.030, 0.78, -0.78, 0.15, 0.0, -0.055, 0.055]) #over drop location
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-0.488, -0.104, 0.02, 0.628, -0.016, 0.030, 0.78, -0.78, 0.15, 0.0, -0.055, 0.055]) #drop location
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-0.488, -0.104, 0.02, 0.628, -0.055, 0.055, 0.78, -0.78, 0.15, 0.0, -0.055, 0.055]) #open grip
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-0.488, -0.104, 0.15, 0.628, -0.055, 0.055, 0.78, -0.78, 0.15, 0.0, -0.055, 0.055]) #up
        group.go()
        rospy.sleep(sleep_time)
    
    def upper_grip_blue():
        group.set_max_velocity_scaling_factor(exec_vel)
        group.set_joint_value_target([-0.78, 0.78, 0.15, 0.0, -0.055, 0.055, 0.78, -0.78, 0.15, 0.0, -0.055, 0.055]) # home pos
        group.go()
        rospy.sleep(sleep_time)
        group.set_max_velocity_scaling_factor(exec_vel)
        group.set_joint_value_target([-0.78, 0.78, 0.15, 0.0, -0.055, 0.055, 0.8203, -1.640, 0.15, -0.768, -0.055, 0.055]) #over the cube
        group.go()
        rospy.sleep(sleep_time)
        group.set_max_velocity_scaling_factor(exec_vel)
        group.set_joint_value_target([-0.78, 0.78, 0.15, 0.0, -0.055, 0.055, 0.8203, -1.640, 0.05, -0.768, -0.055, 0.055]) #down to cube
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-0.78, 0.78, 0.15, 0.0, -0.055, 0.055, 0.8203, -1.640, 0.05, -0.768, -0.016, 0.030]) #grab cube
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-0.78, 0.78, 0.15, 0.0, -0.055, 0.055, 0.8203, -1.640, 0.15, -0.768, -0.016, 0.030]) #up
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-0.78, 0.78, 0.15, 0.0, -0.055, 0.055, 1.204, -1.047, 0.15, -1.884, -0.016, 0.030]) #over drop location
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-0.78, 0.78, 0.15, 0.0, -0.055, 0.055, 1.204, -1.047, 0.02, -1.884, -0.016, 0.030]) #drop location
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-0.78, 0.78, 0.15, 0.0, -0.055, 0.055, 1.204, -1.047, 0.02, -1.884, -0.055, 0.055]) #open grip
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-0.78, 0.78, 0.15, 0.0, -0.055, 0.055, 1.204, -1.047, 0.15, -1.884, -0.055, 0.055]) #up
        group.go()
        rospy.sleep(sleep_time)

    def lower_grip_red():
        group.set_max_velocity_scaling_factor(exec_vel)
        group.set_joint_value_target([-0.78, 0.78, 0.15, 0.0, -0.055, 0.055, 0.78, -0.78, 0.15, 0.0, -0.055, 0.055]) # home pos
        group.go()
        rospy.sleep(sleep_time)
        group.set_max_velocity_scaling_factor(exec_vel)
        group.set_joint_value_target([-0.750, 1.64, 0.15, 0.645, -0.055, 0.055, 0.78, -0.78, 0.15, 0.0, -0.055, 0.055]) #over the cube
        group.go()
        rospy.sleep(sleep_time)
        group.set_max_velocity_scaling_factor(exec_vel)
        group.set_joint_value_target([-0.750, 1.64, 0.05, 0.645, -0.055, 0.055, 0.78, -0.78, 0.15, 0.0, -0.055, 0.055]) #down to cube
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-0.750, 1.64, 0.05, 0.645, -0.016, 0.030, 0.78, -0.78, 0.15, 0.0, -0.055, 0.055]) #grab cube
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-0.750, 1.64, 0.15, 0.645, -0.016, 0.030, 0.78, -0.78, 0.15, 0.0, -0.055, 0.055]) #up
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-0.506, 0.436, 0.15, 0.139, -0.016, 0.030, 0.78, -0.78, 0.15, 0.0, -0.055, 0.055]) #over drop location
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-0.506, 0.436, 0.02, 0.139, -0.016, 0.030, 0.78, -0.78, 0.15, 0.0, -0.055, 0.055]) #drop location
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-0.506, 0.436, 0.02, 0.139, -0.055, 0.055, 0.78, -0.78, 0.15, 0.0, -0.055, 0.055]) #open grip
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-0.506, 0.436, 0.15, 0.139, -0.055, 0.055, 0.78, -0.78, 0.15, 0.0, -0.055, 0.055]) #up
        group.go()
        rospy.sleep(sleep_time) 

    def upper_grip_red():
        group.set_max_velocity_scaling_factor(exec_vel)
        group.set_joint_value_target([-0.78, 0.78, 0.15, 0.0, -0.055, 0.055, 0.78, -0.78, 0.15, 0.0, -0.055, 0.055]) # home pos
        group.go()
        rospy.sleep(sleep_time)
        group.set_max_velocity_scaling_factor(exec_vel)
        group.set_joint_value_target([-0.78, 0.78, 0.15, 0.0, -0.055, 0.055, 1.012, -1.64, 0.15, -0.959, -0.055, 0.055]) #over the cube
        group.go()
        rospy.sleep(sleep_time)
        group.set_max_velocity_scaling_factor(exec_vel)
        group.set_joint_value_target([-0.78, 0.78, 0.15, 0.0, -0.055, 0.055, 1.012, -1.64, 0.05, -0.959, -0.055, 0.055]) #down to cube
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-0.78, 0.78, 0.15, 0.0, -0.055, 0.055, 1.012, -1.64, 0.05, -0.959, -0.016, 0.030]) #grab cube
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-0.78, 0.78, 0.15, 0.0, -0.055, 0.055, 1.012, -1.64, 0.15, -0.959, -0.016, 0.030]) #up
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-0.78, 0.78, 0.15, 0.0, -0.055, 0.055, 0.785, -0.942, 0.15, 1.466, -0.016, 0.030]) #over drop location
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-0.78, 0.78, 0.15, 0.0, -0.055, 0.055, 0.785, -0.942, 0.02, 1.466, -0.016, 0.030]) #drop location
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-0.78, 0.78, 0.15, 0.0, -0.055, 0.055, 0.785, -0.942, 0.02, 1.466, -0.055, 0.055]) #open grip
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-0.78, 0.78, 0.15, 0.0, -0.055, 0.055, 0.785, -0.942, 0.15, 1.466, -0.055, 0.055]) #up
        group.go()
        rospy.sleep(sleep_time)

    def upper_grip_yellow():
        group.set_max_velocity_scaling_factor(exec_vel)
        group.set_joint_value_target([-0.78, 0.78, 0.15, 0.0, -0.055, 0.055, 0.78, -0.78, 0.15, 0.0, -0.055, 0.055]) # home pos
        group.go()
        rospy.sleep(sleep_time)
        group.set_max_velocity_scaling_factor(exec_vel)
        group.set_joint_value_target([-0.78, 0.78, 0.15, 0.0, -0.055, 0.055, 1.134, -1.518, 0.15, -1.204, -0.055, 0.055]) #over the cube
        group.go()
        rospy.sleep(sleep_time)
        group.set_max_velocity_scaling_factor(exec_vel)
        group.set_joint_value_target([-0.78, 0.78, 0.15, 0.0, -0.055, 0.055, 1.134, -1.518, 0.055, -1.204, -0.055, 0.055]) #down to cube
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-0.78, 0.78, 0.15, 0.0, -0.055, 0.055, 1.134, -1.518, 0.055, -1.204, -0.016, 0.030]) #grab cube
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-0.78, 0.78, 0.15, 0.0, -0.055, 0.055, 1.134, -1.518, 0.15, -1.204, -0.016, 0.030]) #up
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-0.78, 0.78, 0.15, 0.0, -0.055, 0.055, 1.553, -1.448, 0.15, -0.174, -0.016, 0.030]) #over drop location
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-0.78, 0.78, 0.15, 0.0, -0.055, 0.055, 1.553, -1.448, 0.02, -0.174, -0.016, 0.030]) #drop location
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-0.78, 0.78, 0.15, 0.0, -0.055, 0.055, 1.553, -1.448, 0.02, -0.174, -0.055, 0.055]) #open grip
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-0.78, 0.78, 0.15, 0.0, -0.055, 0.055, 1.553, -1.448, 0.15, -0.174, -0.055, 0.055]) #up
        group.go()
        rospy.sleep(sleep_time)

    def lower_grip_white():
        group.set_max_velocity_scaling_factor(exec_vel)
        group.set_joint_value_target([-0.78, 0.78, 0.15, 0.0, -0.055, 0.055, 0.78, -0.78, 0.15, 0.0, -0.055, 0.055]) # home pos
        group.go()
        rospy.sleep(sleep_time)
        group.set_max_velocity_scaling_factor(exec_vel)
        group.set_joint_value_target([-0.768, 1.623, 0.15, 0.645, -0.055, 0.055, 0.78, -0.78, 0.15, 0.0, -0.055, 0.055]) #over the cube
        group.go()
        rospy.sleep(sleep_time)
        group.set_max_velocity_scaling_factor(exec_vel)
        group.set_joint_value_target([-0.768, 1.623, 0.001, 0.645, -0.055, 0.055, 0.78, -0.78, 0.15, 0.0, -0.055, 0.055]) #down to cube
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-0.768, 1.623, 0.001, 0.645, -0.016, 0.030, 0.78, -0.78, 0.15, 0.0, -0.055, 0.055]) #grab cube
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-0.768, 1.623, 0.15, 0.645, -0.016, 0.030, 0.78, -0.78, 0.15, 0.0, -0.055, 0.055]) #up
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-0.488, 0.576, 0.15, 1.535, -0.016, 0.030, 0.78, -0.78, 0.15, 0.0, -0.055, 0.055]) #over drop location
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-0.488, 0.576, 0.001, 1.535, -0.016, 0.030, 0.78, -0.78, 0.15, 0.0, -0.055, 0.055]) #drop location
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-0.488, 0.576, 0.001, 1.535, -0.055, 0.055, 0.78, -0.78, 0.15, 0.0, -0.055, 0.055]) #open grip
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-0.488, 0.576, 0.15, 1.535, -0.055, 0.055, 0.78, -0.78, 0.15, 0.0, -0.055, 0.055]) #up
        group.go()
        rospy.sleep(sleep_time) 

    def upper_grip_white():
        group.set_max_velocity_scaling_factor(exec_vel)
        group.set_joint_value_target([-0.78, 0.78, 0.15, 0.0, -0.055, 0.055, 0.78, -0.78, 0.15, 0.0, -0.055, 0.055]) # home pos
        group.go()
        rospy.sleep(sleep_time)
        group.set_max_velocity_scaling_factor(exec_vel)
        group.set_joint_value_target([-0.78, 0.78, 0.15, 0.0, -0.055, 0.055, 0.96, -1.623, 0.15, -0.890, -0.055, 0.055]) #over the cube
        group.go()
        rospy.sleep(sleep_time)
        group.set_max_velocity_scaling_factor(exec_vel)
        group.set_joint_value_target([-0.78, 0.78, 0.15, 0.0, -0.055, 0.055, 0.96, -1.623, 0.001, -0.890, -0.055, 0.055]) #down to cube
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-0.78, 0.78, 0.15, 0.0, -0.055, 0.055, 0.96, -1.623, 0.001, -0.890, -0.016, 0.030]) #grab cube
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-0.78, 0.78, 0.15, 0.0, -0.055, 0.055, 0.96, -1.623, 0.15, -0.890, -0.016, 0.030]) #up
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-0.78, 0.78, 0.15, 0.0, -0.055, 0.055, 0.523, -0.576, 0.15, -1.518, -0.016, 0.030]) #over drop location
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-0.78, 0.78, 0.15, 0.0, -0.055, 0.055, 0.523, -0.576, 0.001, -1.518, -0.016, 0.030]) #drop location
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-0.78, 0.78, 0.15, 0.0, -0.055, 0.055, 0.523, -0.576, 0.001, -1.518, -0.055, 0.055]) #open grip
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-0.78, 0.78, 0.15, 0.0, -0.055, 0.055, 0.523, -0.576, 0.15, -1.518, -0.055, 0.055]) #up
        group.go()
        rospy.sleep(sleep_time)


    while True:
        home_pos()
