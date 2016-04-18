import serial
import MMA8452
from RGBLed import RGBLed

#expected format is: #p#02 where 02 is number of operation

#INITIALIZE
srl = serial.Serial(
	port='/dev/ttyAMA0',
	baudrate = 9600,
	parity = serial.PARITY_NONE,
	bytesize = serial.EIGHTBITS,
	stopbits = serial.STOPBITS_ONE,
	timeout = None
)

acc = MMA8452.MMA8452(0x1D)

print "Ready"
print "Waiting for interaction"

while True:
        if acc.Double_Tap():
                srl.write(".$p$00")
                print "Change Status"
