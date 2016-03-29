# PredicaGadget
#RGBLed.py
####FOR 
  RPI  +  RGB LED shared katode  + Python 2

####HOW TO TEST
1. Connect 
..* red led pin -> pin 23
..*  green led pin -> pin 18
..*  blue led pin -> pin 24
2.  run sudo python RGBLed.py

####HOW TO USE (from python)
```python
from RGBLed import RGBLed
rgbLed = RGBLed(23,18,24) #check below described constructor for details
rgbLed.Green()
```
  
####DOCUMENTATION

**Constructors:**

  RGBLed(r,g,b) #where r,g,b are pin numbers on RPi connected to proper pins on RGB LED

  
**Public Methods:**

  Yellow()  #gives yellow light with values (from 0 to 100) [100,60,0]

  Green()   #gives yellow light with values [3,100,1] 

  Red()     #gives yellow light with values [100,0,0] 

  Blue()    #gives yellow light with values [0,0,100] 

  Violet()  #gives yellow light with values [100,0,100] 

  Off()     #gives yellow light with values [0,0,0] 

  White()   #gives yellow light with values [100,100,100] 
  
