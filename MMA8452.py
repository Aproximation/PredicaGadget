import smbus
import os
import math
from time import sleep

CTRL_REG1 = 0x2A
XYZ_DATA_CFG_REG = 0x0E
CTRL_REG2 = 0x2B
PULSE_CFG_REG = 0x21
PULSE_THSX_REG = 0x23
PULSE_THSY_REG = 0x24
PULSE_THSZ_REG = 0x25
PULSE_TMLT_REG = 0x26
PULSE_LTCY_REG = 0x27
CTRL_REG4 = 0x2D
CTRL_REG5 = 0x2E
PULSE_SRC_REG = 0x22

OUT_X_MSB_REG = 0x01
OUT_X_LSB_REG = 0x02
OUT_Y_MSB_REG = 0x03
OUT_Y_LSB_REG = 0x04
OUT_Z_MSB_REG = 0x05
OUT_Z_LSB_REG = 0x06

class MMA8452:
    
    
    __address = None
    __bus = None
    __tapped = False
    
    def __init__(self, addr):
        self.__address = addr
        self.__bus = smbus.SMBus(1)
        self.__bus.write_byte_data(self.__address, CTRL_REG1, 0x0C)
        self.__bus.write_byte_data(self.__address, XYZ_DATA_CFG_REG, 0x00)
        self.__bus.write_byte_data(self.__address, CTRL_REG2, 0x02)
        
        self.__bus.write_byte_data(self.__address, PULSE_CFG_REG, 0b00101010)
        self.__bus.write_byte_data(self.__address, PULSE_THSX_REG, 0x20)
        self.__bus.write_byte_data(self.__address, PULSE_THSY_REG, 0x20)
        self.__bus.write_byte_data(self.__address, PULSE_THSZ_REG, 0x2A)
        self.__bus.write_byte_data(self.__address, PULSE_TMLT_REG, 0x28)
        self.__bus.write_byte_data(self.__address, PULSE_LTCY_REG, 0x28)
        self.__bus.write_byte_data(self.__address, CTRL_REG4, 0x08)
        self.__bus.write_byte_data(self.__address, CTRL_REG5, 0x08)

        CTRL_REG1_val = self.__bus.read_byte_data(self.__address, CTRL_REG1)
        CTRL_REG1_val |= 0x01
        self.__bus.write_byte_data(self.__address, CTRL_REG1, CTRL_REG1_val)
        
        
    def Read_X(self):
        LX = self.__bus.read_byte_data(self.__address, OUT_X_MSB_REG)
        HX = self.__bus.read_byte_data(self.__address, OUT_X_LSB_REG)
        g = HX | (LX << 8)
        if g > 32767:
            g -= 65536
        return g

    def Read_Y(self):
        LY = self.__bus.read_byte_data(self.__address, OUT_Y_MSB_REG0x03)
        HY = self.__bus.read_byte_data(self.__address, OUT_Y_LSB_REG)
        g = HY | (LY << 8)
        if g > 32767:
            g -= 65536
        return g

    def Read_Z(self):
        LZ = self.__bus.read_byte_data(self.__address, OUT_Z_MSB_REG)
        HZ = self.__bus.read_byte_data(self.__address, OUT_Z_LSB_REG)
        g = HZ | (LZ << 8)
        if g > 32767:
            g -= 65536
        return g

    def Double_Tap(self):
        result = self.__bus.read_byte_data(self.__address, PULSE_SRC_REG)
        if result != 0x00:
            if self.__tapped:
                return False
            else:
                self.__tapped = True
                return True
        else:
            self.__tapped = False
            return False

if __name__ == "__main__":
    acc = MMA8452(0x1D)

    try:
        while True:
            if acc.Double_Tap():
                print "Tapped"

    except KeyboardInterrupt:
        os.system('cls' if os.name == 'nt' else 'clear')
