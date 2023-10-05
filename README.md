# UR3 Project_V8
Plataforma de abordaje endonasal con robot UR3

 ## :stop_sign: Requirements: 
  - Ubuntu 18.04 +
  - ROS Melodic +
  - URSim or Real Robot.
  - Unity 2021.3.0f1 +

## :scroll: Contents: 
  - **Intefaz_v8**:    Contiene el proyecto construido en Unity3D.
  - **Ejecutable**: Contiene el ejecutable del proyecto.
  - **Resources**: Carpeta con trayectorias endonasales + 4 videos de soporte y manual de usuario.
  - [Guia de usuario:***video***](https://youtu.be/SUEaiMpRDwI).
  - [***Abordaje endonasal***](https://youtu.be/2GFgaUTg0pg)
  
 
 

    // ## :exclamation:
 ## :exclamation: Importante!
 - Establecer conexión por medio de cable Ethernet para evitar la perdida de datos.
 - [Repositorio **Interfaz_V7** con sus pasos y requisitos:](https://github.com/sebastian775/UR3Project).
  --------------------------


 <div>
   <p align="center">
<img  src="https://github.com/alvira13/PlataformaUR3_V8/blob/main/Resources/Vista_orig.png" alt="Universal Robot e-Series family" style="width: 46%;"/>

<img  src="https://github.com/alvira13/PlataformaUR3_V8/blob/main/Resources/origejecutable.png" alt="Universal Robot e-Series family" style="width: 45%;"/>
</div>

  **Note**: It is very important to have a clean workstation (catkin_ws), especially if there are Universal Robot files in it, as this may cause a conflict when compiling the catkin_ws.

The following commands are executed consecutively in a single terminal:
This allows you to create both the workspace and install packages, drives etc, that allow you to manipulate the UR robots.

```bash
# source global ros
$ source /opt/ros/melodic/setup.bash

# create a catkin workspace
$ mkdir UR3e && cd UR3e && mkdir -p catkin_ws/src && cd catkin_ws

# clone the driver
$ git clone https://github.com/UniversalRobots/Universal_Robots_ROS_Driver.git src/Universal_Robots_ROS_Driver

# clone the description. Currently, it is necessary to use the melodic-devel-staging branch.
$ git clone -b melodic-devel-staging https://github.com/ros-industrial/universal_robot.git src/universal_robot

# clone  the ur control cartesian
$ git clone https://github.com/UniversalRobots/Universal_Robots_ROS_controllers_cartesian.git src/Universal_Robots_ROS_controllers_cartesian

# install dependencies
$ sudo apt update -qq
$ rosdep update
$ rosdep install --from-paths src --ignore-src -y

# build the workspace
$ catkin_make

# activate the workspace (ie: source it)
$ source devel/setup.bash
```

## Rosbridge installation:
Rosbridge proporciona una API JSON para comunicar programas con ROS.

```bash
$ sudo apt-get install ros-melodic-rosbridge-server
```

 --------------------------

 ### Download v8 executable:
El siguiente ejecutable es construido en Unity3D, se puede descargar de [ejecutable].(https://github.com/alvira13/PlataformaUR3_V8/releases/download/v8.0/Ejecutable.zip)

###  Uso del ejecutable:
**Nota**:Para el correcto uso del ejecutable se deben seguir las indicaciones como muestra el [manual del Usuario].(https://github.com/alvira13/PlataformaUR3_V8/blob/main/Resources/Gu%C3%ADa%20de%20usuario/Manual%20de%20Usuario.pdf)

Desde una teminal ejecutar lo siguiente:

```bash
# Se inicializa el controlador del robot

$ cd cd UR3e/catkin_ws
$ source devel/setup.bash
# "Comando rápido", esto ejecuta el controlador del robot
$ roslaunch ur_robot_driver ur3e_bringup.launch robot_ip:=<YOUR_IP> limited:=true
```
En una nueva terminal ejecutar:

```bash
$ roslaunch rosbridge_server rosbridge_websocket.launch
```
En la carpeta ***Ejecutable*** que se descarga se encuentra un archivo de python llamado ***ListarDatos.py***, se debe abrir una teminal en esta dirección y ejecutar este script con el siguiente comando:

**NOTA**: Se requiere contar con la versión de python3.

```bash
$ python3 ListarDatos.py
```
Ahora ejecutar el archivo llamado ***Ejecutable_V8.x86_64*** contenido en esta carpeta (dar doble click sobre el ícono).


## :shipit:: Autores ✒️
- Leonardo Alberto Paz Paz   (leopaz@unicauca.edu.co)
- Jan Carlos Alvira Meneses  (janalvira@unicauca.edu.co)

## Agradecimientos 🏆
***A aquellos que levantaron el proyecto desde sus inicios***

- Juan David Ruiz            (juandarf@unicauca.edu.co)
- Juan Sebastian Montenegro (exlogam@unicauca.edu.co)

  🚀 
  Tutor : PhD. Oscar Andrés Vivas Albán      (avivas@unicauca.edu.co)
