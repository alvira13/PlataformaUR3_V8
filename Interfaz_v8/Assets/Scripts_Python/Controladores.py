# *******************
# Autores:    Juan David Ruiz (juandavidrf@unicauca.edu.co)
#             Sebastian Montenegro (exlogam@unicauca.edu.co)
# *******************

#********************* Librerias ********************#
import sys
import math
import rospy
import actionlib
from control_msgs.msg import FollowJointTrajectoryAction, FollowJointTrajectoryGoal
from trajectory_msgs.msg import JointTrajectoryPoint
from controller_manager_msgs.srv import SwitchControllerRequest, SwitchController
from controller_manager_msgs.srv import LoadControllerRequest, LoadController
from cartesian_control_msgs.msg import (
    FollowCartesianTrajectoryAction,
    FollowCartesianTrajectoryGoal,
    CartesianTrajectoryPoint,
)


# Compatibility for python2 and python3
if sys.version_info[0] < 3:
    input = raw_input

# If your robot description is created with a tf_prefix, those would have to be adapted
JOINT_NAMES = [
    "shoulder_pan_joint",
    "shoulder_lift_joint",
    "elbow_joint",
    "wrist_1_joint",
    "wrist_2_joint",
    "wrist_3_joint",
]

# All of those controllers can be used to execute joint-based trajectories.
# The scaled versions should be preferred over the non-scaled versions.
JOINT_TRAJECTORY_CONTROLLERS = [
    "scaled_pos_joint_traj_controller",
    "scaled_vel_joint_traj_controller",
    "pos_joint_traj_controller",
    "vel_joint_traj_controller",
    "forward_joint_traj_controller",
]

# All of those controllers can be used to execute Cartesian trajectories.
# The scaled versions should be preferred over the non-scaled versions.
CARTESIAN_TRAJECTORY_CONTROLLERS = [
    "pose_based_cartesian_traj_controller",
    "joint_based_cartesian_traj_controller",
    "forward_cartesian_traj_controller",
]

# We'll have to make sure that none of these controllers are running, as they will
# be conflicting with the joint trajectory controllers
CONFLICTING_CONTROLLERS = ["joint_group_vel_controller", "twist_controller"]

class TrajectoryClient:
    def __init__(self):
        rospy.init_node("python")

        timeout = rospy.Duration(5)
        self.switch_srv = rospy.ServiceProxy(
            "controller_manager/switch_controller", SwitchController
        )
        self.load_srv = rospy.ServiceProxy("controller_manager/load_controller", LoadController)
        try:
            self.switch_srv.wait_for_service(timeout.to_sec())
        except rospy.exceptions.ROSException as err:
            rospy.logerr("Could not reach controller switch service. Msg: {}".format(err))
            sys.exit(-1)

        self.joint_trajectory_controller = JOINT_TRAJECTORY_CONTROLLERS[0]
        self.cartesian_trajectory_controller = CARTESIAN_TRAJECTORY_CONTROLLERS[0]

    def send_joint_trajectory(self, listaP, velocidad):
        """Creates a trajectory and sends it using the selected action server"""

        # make sure the correct controller is loaded and activated
        self.switch_controller(JOINT_TRAJECTORY_CONTROLLERS[0])
        trajectory_client = actionlib.SimpleActionClient(
            "{}/follow_joint_trajectory".format(JOINT_TRAJECTORY_CONTROLLERS[0]),
            FollowJointTrajectoryAction,
        )

        # Wait for action server to be ready
        timeout = rospy.Duration(5)
        if not trajectory_client.wait_for_server(timeout):
            rospy.logerr("Could not reach controller action server.")
            sys.exit(-1)

        # Create and fill trajectory goal
        goal = FollowJointTrajectoryGoal()
        goal.trajectory.joint_names = JOINT_NAMES

        # The following list are arbitrary positions
        # Change to your own needs if desired
        position_list = listaP
        
        if (len(position_list)<=1):
            duration_list = [velocidad]
        else:
            duration_list = [3]

        for i, position in enumerate(position_list):
            point = JointTrajectoryPoint()
            point.positions = position
            dif = [0,0,0,0,0,0]
            if i>0:
                for j in range(6):
                    dif[j] = abs(position_list[i][j] - position_list[i-1][j])
                DistanciaMax = max(dif)
                tiempo = (DistanciaMax/(velocidad*(math.pi/180)))
                duration_list.append(duration_list[i-1]+tiempo)
            point.time_from_start = rospy.Duration(duration_list[i])
            goal.trajectory.points.append(point)

        rospy.loginfo("Executing trajectory using the {}".format(JOINT_TRAJECTORY_CONTROLLERS[0]))

        trajectory_client.send_goal(goal)
        trajectory_client.wait_for_result()

        result = trajectory_client.get_result()
        rospy.loginfo("Trajectory execution finished in state {}".format(result.error_code))    

    def send_cartesian_trajectory(self, listaP, velocidad):
        #self.choose_controller(self)
        """Creates a Cartesian trajectory and sends it using the selected action server"""
        self.switch_controller( CARTESIAN_TRAJECTORY_CONTROLLERS[0])

        # make sure the correct controller is loaded and activated
        goal = FollowCartesianTrajectoryGoal()
        trajectory_client = actionlib.SimpleActionClient(
            "{}/follow_cartesian_trajectory".format( CARTESIAN_TRAJECTORY_CONTROLLERS[0]),
            FollowCartesianTrajectoryAction,
        )

        # Wait for action server to be ready
        timeout = rospy.Duration(5)
        if not trajectory_client.wait_for_server(timeout):
            rospy.logerr("Could not reach controller action server.")
            sys.exit(-1)

        # The following list are arbitrary positions
        # Change to your own needs if desired
        pose_list =listaP
        duration_list = [3]
        
        for i, pose in enumerate(pose_list):
            point = CartesianTrajectoryPoint()
            point.pose = pose

            if i>0:
                dx = pose_list[i].position.x - pose_list[i-1].position.x
                dy = pose_list[i].position.y - pose_list[i-1].position.y
                dz = pose_list[i].position.z - pose_list[i-1].position.z
                Distancia = math.sqrt((dx**2) + (dy**2) + (dz**2))
                if Distancia>0:
                    tiempo = (Distancia/(velocidad))*1.5
                else:
                    tiempo = 1.5
                duration_list.append(duration_list[i-1]+tiempo)
            
            point.time_from_start = rospy.Duration(duration_list[i])
            goal.trajectory.points.append(point)

        rospy.loginfo(
            "Executing trajectory using the {}".format( CARTESIAN_TRAJECTORY_CONTROLLERS[0])
        )
        trajectory_client.send_goal(goal)
        trajectory_client.wait_for_result()

        result = trajectory_client.get_result()

        rospy.loginfo("Trajectory execution finished in state {}".format(result.error_code))

    def send_other_trajectory(self, listaP, velocidad):
        #self.choose_controller(self)
        """Creates a Cartesian trajectory and sends it using the selected action server"""
        self.switch_controller( CARTESIAN_TRAJECTORY_CONTROLLERS[0])

        # make sure the correct controller is loaded and activated
        goal = FollowCartesianTrajectoryGoal()
        trajectory_client = actionlib.SimpleActionClient(
            "{}/follow_cartesian_trajectory".format( CARTESIAN_TRAJECTORY_CONTROLLERS[0]),
            FollowCartesianTrajectoryAction,
        )

        # Wait for action server to be ready
        timeout = rospy.Duration(5)
        if not trajectory_client.wait_for_server(timeout):
            rospy.logerr("Could not reach controller action server.")
            sys.exit(-1)

        # The following list are arbitrary positions
        # Change to your own needs if desired
        pose_list =listaP
        duration_list = [3]

        for i, pose in enumerate(pose_list):
            point = CartesianTrajectoryPoint()
            point.pose = pose

            if i>0:
                dx = pose_list[i].position.x - pose_list[i-1].position.x
                dy = pose_list[i].position.y - pose_list[i-1].position.y
                dz = pose_list[i].position.z - pose_list[i-1].position.z
                Distancia = math.sqrt((dx**2) + (dy**2) + (dz**2))
                if Distancia>0:
                    tiempo = (Distancia/(velocidad))*1
                else:
                    tiempo = 1.5
                duration_list.append(duration_list[i-1]+tiempo)
            
            point.time_from_start = rospy.Duration(duration_list[i])
            goal.trajectory.points.append(point)

        rospy.loginfo(
            "Executing trajectory using the {}".format( CARTESIAN_TRAJECTORY_CONTROLLERS[0])
        )
        trajectory_client.send_goal(goal)
        trajectory_client.wait_for_result()

        result = trajectory_client.get_result()

        rospy.loginfo("Trajectory execution finished in state {}".format(result.error_code))

    def choose_controller(self):
        self.cartesian_trajectory_controller = CARTESIAN_TRAJECTORY_CONTROLLERS[0]

    def switch_controller(self, target_controller):
        """Activates the desired controller and stops all others from the predefined list above"""
        other_controllers = (
            JOINT_TRAJECTORY_CONTROLLERS
            + CARTESIAN_TRAJECTORY_CONTROLLERS
            + CONFLICTING_CONTROLLERS
        )

        other_controllers.remove(target_controller)

        srv = LoadControllerRequest()
        srv.name = target_controller
        self.load_srv(srv)

        srv = SwitchControllerRequest()
        srv.stop_controllers = other_controllers
        srv.start_controllers = [target_controller]
        srv.strictness = SwitchControllerRequest.BEST_EFFORT
        self.switch_srv(srv)