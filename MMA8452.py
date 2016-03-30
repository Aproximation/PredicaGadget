import smbus
import os
import math
from time import sleep

class MMA8452:
    __address = None
    __bus = None
    
    def __init__(self, addr):
        self.__address = addr
        self.__bus = smbus.SMBus(1)
        self.__bus.write_byte_data(self.__address, 0x2A, 0x01)

    def Read_X(self):
        LX = self.__bus.read_byte_data(self.__address, 0x01)
        HX = self.__bus.read_byte_data(self.__address, 0x02)
        g = HX | (LX << 8)
        if g > 32767:
            g -= 65536
        return g

    def Read_Y(self):
        LY = self.__bus.read_byte_data(self.__address, 0x03)
        HY = self.__bus.read_byte_data(self.__address, 0x04)
        g = HY | (LY << 8)
        if g > 32767:
            g -= 65536
        return g

    def Read_Z(self):
        LZ = self.__bus.read_byte_data(self.__address, 0x05)
        HZ = self.__bus.read_byte_data(self.__address, 0x06)
        g = HZ | (LZ << 8)
        if g > 32767:
            g -= 65536
        return g

if __name__ == "__main__":
    acc = MMA8452(0x1D)

    try:
        while True:
            print "X: " + str(acc.Read_X())
            print "Y: " + str(acc.Read_Y())
            print "Z: " + str(acc.Read_Z())
            sleep(0.1)
            os.system('cls' if os.name == 'nt' else 'clear')

    except KeyboardInterrupt:
        os.system('cls' if os.name == 'nt' else 'clear')
