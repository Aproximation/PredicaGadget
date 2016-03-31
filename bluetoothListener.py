import serial
from RGBLed import RGBLed
import RPi.GPIO as GPIO
import sys

#expected format is: #p#02 where 02 is number of operation

#INITIALIZE
srl = serial.Serial(
        port='/dev/ttyAMA0',
        baudrate = 9600,
        parity = serial.PARITY_NONE,
        bytesize = serial.EIGHTBITS,
        stopbits = serial.STOPBITS_ONE,
        timeout = None,
	xonxoff = False,
	rtscts = False,
	dsrdtr = False
)

rgbLed = RGBLed(23,18,24)


#IMPLEMENTATION
def __ListenForCommunication():
	try:
	        print('Listening')
	        while (True):
	                value = srl.read(1)
			#print ('Check')
	                if(value == "$"):
	                        value = srl.read(1)
				#print ('Raz')
	                        if (value == 'p'):
	                                value = srl.read(1)
					#print ('Dwa')
	                                if (value == '$'):
						#print ('Trzy')
	                                        __ReadMsg()
	except KeyboardInterrupt:
		GPIO.cleanup()
		sys.exit(1)		
	except:
		__ListenForCommunication()
def __ReadMsg():
        msgNum = srl.read(2)
        if (__isNum(msgNum)):
                num = int(msgNum)
                #print num
                DoAction(num)

def __isNum(value):
        try:
                int(value)
                return True
        except ValueError:
                return False

def DoAction(actionNum):
        try:
                action = {
                        0 : lambda:rgbLed.Off(),
                        1 : lambda:rgbLed.Green(),
                        2 : lambda:rgbLed.Yellow(),
                        4 : lambda:rgbLed.Red()
                        #5 : DoThis
                }
                action[actionNum]()
        except KeyError:
		rgbLed.Off()
                print "There is not such action asshole"

def ChangeStatus(value):
        print "Status changed to: ",value

def DoThis():
        print "I'm doing this"

__ListenForCommunication()
