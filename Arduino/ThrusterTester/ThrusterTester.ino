#define LED 8
#define POTIN A0
#define THRUSTEROUT 9

#include<Servo.h>

Servo thruster;
void setup() {
  // put your setup code here, to run once:
  thruster.attach(THRUSTEROUT);
  pinMode(POTIN, INPUT);
  pinMode(LED, OUTPUT);
  Serial.begin(9600);
  thruster.writeMicroseconds(1500);
  delay(2500);
}

void loop() {
  // put your main code here, to run repeatedly:
  int val = analogRead(POTIN);            // reads the value of the potentiometer (value between 0 and 1023)
  val = map(val, 0, 1023, 1100, 1900);     // 
  thruster.writeMicroseconds(val);                  // sets the thruster according to the scaled value
  if(val>1475 && val<1525)
  {
    digitalWrite(LED, HIGH);
  }
  else
  {
    digitalWrite(LED, LOW);
  }
  Serial.println(val);
  delay(5); 
}
