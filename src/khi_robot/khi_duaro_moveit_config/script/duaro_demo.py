#!/usr/bin/env python

import rospy

from moveit_commander import MoveGroupCommander

if __name__ == '__main__':

    #init_node()
    group = MoveGroupCommander("botharms")
    sleep_time = 0.2
    exec_vel = 1.0

    def position_one():
        group.set_max_velocity_scaling_factor(exec_vel*0.2)
        group.set_joint_value_target([-0.78, 0.78, 0.14, 0.0, 0.78, -0.78, 0.14, 0.0]) #[-0.78, 0.78, 0.14, 0.0, 0.78, -0.78, 0.14, 0.0]
        group.go()
        rospy.sleep(sleep_time)

    def position_two():
        group.set_max_velocity_scaling_factor(exec_vel)
        group.set_joint_value_target([-1.5, 1.5, 0.14, 0.0, 1.5, -1.5, 0.14, 0.0]) #[-1.5, 1.5, 0.14, 0.0, 1.5, -1.5, 0.14, 0.0]
        group.go()
        rospy.sleep(sleep_time)

    def lower_pick():
        group.set_max_velocity_scaling_factor(exec_vel)
        group.set_joint_value_target([-0.29, 0.21, 0, 0, 0.78, -0.78, 0.08, 0])
        group.go()
        rospy.sleep(sleep_time)

    while True:
        position_one()
        position_two()
        lower_pick()