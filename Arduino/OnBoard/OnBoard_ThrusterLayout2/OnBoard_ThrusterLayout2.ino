#include <ArduinoSTL.h>
#include<Servo.h>
#include<Wire.h>
#include<Ethernet.h>
#include <stdio.h>

#define PASSWORD "p" //change this to the actual password
#define ROV_NAME "Innovocean X" //16 character max, please

const int MPU_addr=0x68; 

bool Authorized = false;

//Thrusters Left, Right, VerticalFrontLeft...
Servo FL, FR, VFL, VFR, VBL, VBR;
Servo ClawOpen;
Servo ClawRotate;
int ClawOpenPos = 45;
int ClawRotatePos = 90;
int ClawOpenSpeed = 0;
int ClawRotateSpeed = 0;

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
  ClawOpen.attach(9);
  ClawRotate.attach(A3);
  ClawOpen.write(ClawOpenPos);
  ClawRotate.write(ClawRotatePos);
  //Setup anything else
  pinMode(A0, OUTPUT);
  pinMode(A1, OUTPUT);
  pinMode(A2, OUTPUT);
  digitalWrite(A2, HIGH);
  digitalWrite(A0, HIGH);
  
  //Start Server
  server.begin();
  
  //Set-up thrusters
  //see documentation for pin assignments. https://docs.google.com/document/d/1n_Al_vgk9xdAz7ClMqiYWrTyUFkZcng_2KTTv5HQe0o/edit
  FL.attach(2);
  FR.attach(3);
  VFL.attach(5);
  VFR.attach(6);
  VBL.attach(7);
  VBR.attach(8);

  //Send Stop signal. This makes the chimes sound off letting us know we are done initializing.
  Stop();  
}

void UpdateServoPositions()
{
  //delay introduced in while Client.Connected loop.
  
  //prevent position from getting "stuck" and not allow it to go over or under
  if((ClawOpenPos <= 0 && ClawOpenSpeed>0) || (ClawOpenPos >= 90 && ClawOpenSpeed < 0))
  {
    ClawOpenPos += ClawOpenSpeed;
  }
  if((ClawRotatePos <= 0 && ClawRotateSpeed>0) || (ClawRotatePos >= 180 && ClawRotateSpeed < 0))
  {
    ClawRotatePos += ClawRotateSpeed;
  }
  if(ClawOpenPos>0 && ClawOpenPos<90 && abs(ClawOpenSpeed) > 0)
  {   
    ClawOpenPos += ClawOpenSpeed;
    ClawOpen.write(ClawOpenPos);
  }
  if(ClawRotatePos>0 && ClawRotatePos<180 && abs(ClawRotateSpeed) >0)
  {
    ClawRotatePos += ClawRotateSpeed;
    ClawRotate.write(ClawRotatePos);
  }
}

void loop() {
  EthernetClient client = server.available();
  if(client)
  {
    //Serial.println("Client Connected.");
    while(client.connected())
    {
      UpdateServoPositions();   
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
      delay(10);
    }
    //Serial.println("Client disconnected.");
    Stop();
    Authorized = false;
  }
  
}

void pickCommand(EthernetClient client, String name, std::vector<String> params)
{
  //Serial.println(name);
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
      char str[params[i].length()+1];
      params[i].toCharArray(str, params[i].length()+1);
      parameters[i] = atoi(str);
    }
    SetThruster(parameters[0], parameters[1]);
  }
  else if(name == "setServoSpeed" && Authorized)
  {
    int parameters[2];
    for(int i = 0; i<2;i++)
    {
      char str[params[i].length()+1];
      params[i].toCharArray(str, params[i].length()+1);
      parameters[i] = atoi(str);
    }
    switch(parameters[0])
    {
        case 0:
          ClawOpenSpeed = parameters[1];
          break;
        case 1:
          ClawRotateSpeed = parameters[1];
          break;
    }
  }
  else if(name == "analogRead" && Authorized)
  {
      char str[params[0].length()+1];
      params[0].toCharArray(str, params[0].length()+1);
      int pin = atoi(str);
      client.println(analogRead(pin));
  }
  else if(name == "digitalWrite" && Authorized)
  {
    int parameters[2];
    for(int i = 0; i<2;i++)
    {
      char str[params[i].length()+1];
      params[i].toCharArray(str, params[i].length()+1);
      parameters[i] = atoi(str);
    }
    digitalWrite(parameters[0], parameters[1]);
  }
}

void Stop()
{
  for(int i = 0; i<10; i++)
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
      //Serial.print("Left: "); Serial.println(msValue);
      FL.writeMicroseconds(msValue);
      break;
    case 1:
      FR.writeMicroseconds(msValue);
      break;
    /*
     * Not implemented on ROV with 4 vertical thrusters.
     * case 2:
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
      */
    case 6:
      VFL.writeMicroseconds(msValue);
      break;
    case 7:
      VFR.writeMicroseconds(msValue);
      break;
    case 8:
      VBL.writeMicroseconds(msValue);
      break;
    case 9:
      VBR.writeMicroseconds(msValue);
      break;
  }
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

