/*
 * 
 * For Thruster Layout 1
 * 
 */

#include <ArduinoSTL.h>
#include<Servo.h>
#include<Wire.h>
#include<Ethernet.h>
#include<PID_v1.h>
#include <stdio.h>

#define PASSWORD "password" //change this to actual password
#define ROV_NAME "Innovocean X" //16 character max, please

const int MPU_addr=0x68; 

bool Authorized = false;

//PID
double Input, Output, Setpoint;
PID rollPID(&Input, &Output, &Setpoint,1,3,1, DIRECT);

//Thrusters Front Left, Front Right...
Servo FL, FR, BL, BR, VL, VR;

EthernetServer server = EthernetServer(1740);
byte mac[] = { 0xDE, 0xAD, 0xBE, 0xEF, 0xFE, 0xED };  

void setup() {
  // put your setup code here, to run once:
  //Join network
  Serial.begin(9600);
  Serial.println("Joining Network");
  while (Ethernet.begin(mac) == 0) {
    Serial.println("Failed to configure Ethernet using DHCP");
    delay(3000);
  }
  Serial.println("Network Joined. ");
  printIPAddress();

  //Setup accelerometer

  Wire.begin();
  Wire.beginTransmission(MPU_addr);
  Wire.write(0x6B);  // PWR_MGMT_1 register
  Wire.write(0);     // set to zero (wakes up the MPU-6050)
  Wire.endTransmission(true);
  /*
   * Set Range to 8g*/
  Wire.beginTransmission(MPU_addr);
  Wire.write(0x1C);
  Wire.write((byte)16);
  Wire.endTransmission(true);
  
  
  //Setup servos

  //Setup anything else

  //Setup PID
  Setpoint = 0;
  Input = 0;
  rollPID.SetOutputLimits(-255,255);
  rollPID.SetMode(AUTOMATIC);
  
  //Start Server
  server.begin();
  
  //Set-up thrusters
  //see documentation for pin assignments. https://docs.google.com/document/d/1n_Al_vgk9xdAz7ClMqiYWrTyUFkZcng_2KTTv5HQe0o/edit
  FL.attach(2);
  FR.attach(3);
  BL.attach(5);
  BR.attach(6);
  VL.attach(7);
  VR.attach(8);

  //Send Stop signal
  Stop();  
}

void loop() {
  // put your main code here, to run repeatedly:
  EthernetClient client = server.available();
  if(client)
  {
    Serial.println("Client Connected.");
    while(client.connected())
    {
      if(client.available())
      {
        //parse command from client stream
          char c;
          String commandName = "";
          String currentParameter = "";
          std::vector<String> parameters;
          while((c = client.read()) != '}' && c > -1)
          {
            if(c == '{')
            {
              parameters.clear();
              //loop until we reach the end of command name
              while((c = client.read()) != ':' && c > -1)
              {
                commandName += c;
              }
              continue;
            }

            if(c == ',')
            {
            //add currentParameter to vector
              parameters.push_back(currentParameter);
              currentParameter = "";
            }
            else
            {
              currentParameter += c;
            }
            
          }
          //run command
          pickCommand(client, commandName, parameters);
          
      }
    }
    Stop();
    Authorized = false;
  }
  
}

void pickCommand(EthernetClient client, String name, std::vector<String> params)
{
  Serial.println(name);
  if(name == "authorize")
  {
    if(params[0] == PASSWORD)
    {
      client.write(0x01);
      Authorized = true;
    }
    else
    {
      client.write((byte)0);
      Authorized = false;
    }
  }
  else if(name == "VerticalStabilize" && Authorized)
  {
    int vectors[2];
    for(int i = 0; i<2;i++)
    {
      char str[params[i].length()];
      params[i].toCharArray(str, sizeof(str));
      vectors[i] = atoi(str);
    }
    VerticalStabilize(vectors);
  }
  else if(name == "GetAccelerations")
  {
    int16_t accel[7];
    GetAccelerations(accel);
    client.print("{X="); client.print(accel[0]);
    client.print(";Y="); client.print(accel[1]);
    client.print(";Z="); client.print(accel[2]);
    //Gyros
    client.print(";T="); client.print(accel[3]);
    client.print(";A="); client.print(accel[4]);
    client.print(";B="); client.print(accel[5]);
    client.print(";C="); client.print(accel[6]);
    client.print("}");
  }
  else if(name == "GetName")
  {
    String rovName = ROV_NAME;
    while(rovName.length()<16)
    {
      rovName += " ";
    }
    client.print(rovName);
  }
  else if(name == "SetThruster" && Authorized)
  {
    int parameters[2];
    for(int i = 0; i<2;i++)
    {
      char str[params[i].length()];
      params[i].toCharArray(str, sizeof(str));
      parameters[i] = atoi(str);
    }
    SetThruster(parameters[0], parameters[1]);
  }
}

void Stop()
{
  for(int i = 0; i<6; i++)
  {
    SetThruster(i, 1500);
  }
}

void printIPAddress(){
  Serial.print("My IP address: ");
  for (byte thisByte = 0; thisByte < 4; thisByte++) {
    // print the value of each byte of the IP address:
    Serial.print(Ethernet.localIP()[thisByte], DEC);
    Serial.print(".");
  }

  Serial.println();
}
void SetThruster(int thruster, int msValue)
{
  switch(thruster)
  {
    case 0:
      FL.writeMicroseconds(msValue);
      break;
    case 1:
      FR.writeMicroseconds(msValue);
      break;
    case 2:
      BL.writeMicroseconds(msValue);
      break;
    case 3:
      BR.writeMicroseconds(msValue);
      break;
    case 4:
      VL.writeMicroseconds(msValue);
      break;
    case 5:
      VR.writeMicroseconds(msValue);
      break;
  }
}

void VerticalStabilize(int vectors[])
{
  int16_t accel[7];
  GetAccelerations(accel);
  Input = map(accel[1], -4096, 4096, -255, 255);
  if(abs(vectors[1])<256)
  {
    Setpoint = vectors[1];
  }
  if(rollPID.Compute())
  {
    //Serial.print("Raw = ");Serial.print(accel[1]);
    //Serial.print("| Input = ");Serial.print(Input);
    //Serial.print(" | Output = "); Serial.println(Output);
  }
  VL.writeMicroseconds(1500+vectors[0]+(Output/2));
  VR.writeMicroseconds(1500+vectors[0]+(Output/-2));

}

void GetAccelerations(int16_t values[])
{
  Wire.beginTransmission(MPU_addr);
  Wire.write(0x3B);  // starting with register 0x3B (ACCEL_XOUT_H)
  Wire.endTransmission(false);
  Wire.requestFrom(MPU_addr,14,true);  // request a total of 14 registers
  values[0]=Wire.read()<<8|Wire.read();  // 0x3B (ACCEL_XOUT_H) & 0x3C (ACCEL_XOUT_L)    
  values[1]=Wire.read()<<8|Wire.read();  // 0x3D (ACCEL_YOUT_H) & 0x3E (ACCEL_YOUT_L)
  values[2]=Wire.read()<<8|Wire.read();  // 0x3F (ACCEL_ZOUT_H) & 0x40 (ACCEL_ZOUT_L)
  values[3]=(Wire.read()<<8|Wire.read())/340.00+36.53;  // 0x41 (TEMP_OUT_H) & 0x42 (TEMP_OUT_L)
  values[4]=Wire.read()<<8|Wire.read();  // 0x43 (GYRO_XOUT_H) & 0x44 (GYRO_XOUT_L)
  values[5]=Wire.read()<<8|Wire.read();  // 0x45 (GYRO_YOUT_H) & 0x46 (GYRO_YOUT_L)
  values[6]=Wire.read()<<8|Wire.read();  // 0x47 (GYRO_ZOUT_H) & 0x48 (GYRO_ZOUT_L)
}

