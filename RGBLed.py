#!/usr/bin/python
import RPi.GPIO as GPIO
import sys
from time import sleep

#default constants
FREQUENCY = 80

class RGBLed:

	_R = None	#pwm for red color
	_G = None	#pwm for green color
	_B = None	#pwm for blue color

	def __init__(self, red, green, blue):
		r = int(red)
		g = int(green)
		b = int(blue)
		GPIO.setmode(GPIO.BCM)
		GPIO.setup(r, GPIO.OUT)
		GPIO.setup(g, GPIO.OUT)
		GPIO.setup(b, GPIO.OUT)
		self._R = GPIO.PWM(r,FREQUENCY)
		self._G = GPIO.PWM(g,FREQUENCY)
		self._B = GPIO.PWM(b,FREQUENCY)	
		self.Off()

	def __ChangeColor(self,r,g,b):
		self._R.start(r)
		self._G.start(g)
		self._B.start(b)

	def Green(self):
		self.__ChangeColor(3,100,1)
	
	def Violet(self):
		self.__ChangeColor(100,0,100)
	
	def Yellow(self):
		self.__ChangeColor(100,60,0)
	
	def Red(self):
		self.__ChangeColor(100,0,0)
	
	def Blue(self):
		self.__ChangeColor(0,0,100)
	
	def White(self):
		self.__ChangeColor(100,100,100)
	
	def Off(self):
		self.__ChangeColor(0,0,0)
	
if __name__ == "__main__":
    # if run directly we'll just create an instance of the class and output 
    # the current readings
	rgbLed = RGBLed(23,18,24)
	# Check command line argument
	try:
		while True:
			rgbLed.Red()
			sleep(1)
			rgbLed.Yellow()
			sleep(1)
			rgbLed.Green()
			sleep(1)
			rgbLed.Off()
			sleep(1)
	
	except KeyboardInterrupt:
		print "Stopped"
		GPIO.cleanup()