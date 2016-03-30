import serial

#expected format is: #p#02 where 02 is number of operation

srl = serial.Serial(
	port='/dev/ttyAMA0',
	baudrate = 9600,
	parity = serial.PARITY_NONE,
	bytesize = serial.EIGHTBITS,
	stopbits = serial.STOPBITS_ONE,
	timeout = None
)

def __ListenForCommunication():
        print('Listening')
	while (True):
		value = srl.read(1)
		if(value == '#'):
			value = srl.read(1)
			if (value == 'p'):
				value = srl.read(1)
				if (value == '#'):
					__ReadMsg()

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
			0 : lambda:ChangeStatus("Offline"),
			1 : lambda:ChangeStatus("Online"),
			2 : lambda:ChangeStatus("Away"),
			3 : lambda:ChangeStatus("DND"),
                        4 : DoThis
		}
		action[actionNum]()
	except KeyError:
		print "There is not such action asshole"
def ChangeStatus(value):
	print "Status changed to: ",value

def DoThis():
        print "I'm doing this"

__ListenForCommunication()