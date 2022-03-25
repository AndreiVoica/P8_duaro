#!/usr/bin/env python3

import rospy
import tty
import sys
import termios

from moveit_commander import MoveGroupCommander

orig_settings = termios.tcgetattr(sys.stdin)
tty.setcbreak(sys.stdin)
x = 0
if __name__ == '__main__':

    #init_node() [L1, L2, L3, L4, U1, U2, U3, U4]
    group = MoveGroupCommander("botharms")
    sleep_time = 0.2
    exec_vel = 1.0
    

    #Home position
    def home_pos():
        group.set_max_velocity_scaling_factor(exec_vel)
        group.set_joint_value_target([-0.78, 0.78, 0.09, 0.0, 0.78, -0.78, 0.09, 0.0]) #[-0.78, 0.78, 0.14, 0.0, 0.78, -0.78, 0.14, 0.0]
        group.go()
        rospy.sleep(sleep_time)

    def upper_plate():
        group.set_max_velocity_scaling_factor(exec_vel)

        group.set_joint_value_target([0.0, 0.0, 0.09, 0.0, 0.78, -0.78, 0.09, 0.0]) # goes to upper plate location
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([0.0, 0.0, 0.0, 0.0, 0.78, -0.78, 0.09, 0.0]) # down J3 (grab the upper plate)
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([0.0, 0.0, 0.09, 0.0, 0.78, -0.78, 0.09, 0.0]) # up J3
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-1.18, 0.85, 0.09, 0.0, 0.78, -0.78, 0.09, 0.0]) # goes to delivery location for upper plate
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-1.18, 0.85, 0.0, 0.0, 0.78, -0.78, 0.09, 0.0]) # down J3 (places the upper plate)
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-1.18, 0.85, 0.09, 0.0, 0.78, -0.78, 0.09, 0.0]) # up J3
        group.go()
        rospy.sleep(sleep_time)

    def round_round_peg():
        group.set_max_velocity_scaling_factor(exec_vel)

        group.set_joint_value_target([-0.78, 0.78, 0.09, 0.0, 0.0, 0.13, 0.09, 0.0]) # goes to upper plate location
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-0.78, 0.78, 0.09, 0.0, 0.0, 0.13, 0.0, 0.0]) # down J3 (grab the upper plate)
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-0.78, 0.78, 0.09, 0.0, 0.0, 0.13, 0.09, 0.0]) # up J3
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-0.78, 0.78, 0.09, 0.0, 1.22, -1.13, 0.09, 0.0]) # goes to delivery location for upper plate
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-0.78, 0.78, 0.09, 0.0, 1.22, -1.13, 0.0, 0.0]) # down J3 (places the upper plate)
        group.go()
        rospy.sleep(sleep_time)
        group.set_joint_value_target([-0.78, 0.78, 0.09, 0.0, 1.22, -1.13, 0.09, 0.0]) # up J3
        group.go()
        rospy.sleep(sleep_time)

    while True:
        upper_plate()
        round_round_peg()
        if x != chr(27): # ESC
            x=sys.stdin.read(1)[0]
            round_round_peg()
            x = 0
        elif x != chr(65): # A 
            x=sys.stdin.read(1)[0]
            upper_plate()
            x = 0

        termios.tcsetattr(sys.stdin, termios.TCSADRAIN, orig_settings) 