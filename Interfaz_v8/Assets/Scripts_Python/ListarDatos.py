# *******************
# Autores:    Juan David Ruiz (juandavidrf@unicauca.edu.co)
#             Sebastian Montenegro (exlogam@unicauca.edu.co)
# *******************

#********************* Librerias ********************#
import rospy
from trajectory_msgs.msg import JointTrajectory
from cartesian_control_msgs.msg import CartesianTrajectory
from Controladores import TrajectoryClient


#**************** Variables ******************#
cli=TrajectoryClient()

# Nombre de los topicos a suscribirse
topicSub1 = '/Articulaciones'
topicSub2 = '/Coordenadas'
topicSub3 = '/Trayectorias'

#Listas vacias para guardas los valores de los mensajes
listaPosition = []
listaPoses = []
listaTrajectory = []


#**************** Funciones ******************#
def RecibirDatos():                                         #Lee si hay mensajes nuevos en los topicos
    rospy.init_node("python")
    rospy.Subscriber(topicSub1, JointTrajectory, ControlArticular)
    rospy.Subscriber(topicSub2, CartesianTrajectory, ControlCartesiano)
    rospy.Subscriber(topicSub3, CartesianTrajectory, ControlTrayectoria)

    print('Ejecutando...')
    # spin() simply keeps python from exiting until this node is stopped
    rospy.spin()


def ControlArticular(ArrayArticular):                       #Envia los valores articulares al controlador
    mensaje = ArrayArticular
    for i in range(len(mensaje.points)):
        listaPosition.append(mensaje.points[i].positions)
        Velocidad = mensaje.points[0].velocities[0]
    
    cli.send_joint_trajectory(listaPosition, Velocidad)
    listaPosition.clear()


def ControlCartesiano(ArrayCartesiano):                     #Envia los valores cartesianos al controlador
    mensaje = ArrayCartesiano
    for i in range(len(mensaje.points)):
        listaPoses.append(mensaje.points[i].pose)
        Velocidad = mensaje.points[0].twist.linear.x
 
    cli.send_cartesian_trajectory(listaPoses,Velocidad)
    listaPoses.clear()


def ControlTrayectoria(ArrayTrayectoria):                   #Envia la trayectoria al controlador
    mensaje = ArrayTrayectoria
    for i in range(len(mensaje.points)):
        listaTrajectory.append(mensaje.points[i].pose)
        Velocidad = mensaje.points[0].twist.linear.x
    
    cli.send_other_trajectory(listaTrajectory,Velocidad)
    listaTrajectory.clear()

    
if __name__ == '__main__':
    RecibirDatos()