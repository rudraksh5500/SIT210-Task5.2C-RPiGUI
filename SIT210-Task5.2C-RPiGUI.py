from tkinter import *
import tkinter.font
import RPi.GPIO as GPIO
from gpiozero import LED
GPIO.setmode(GPIO.BCM)

##HARDWARE DEFINITIONS
redLED = LED(13)
blueLED = LED(19)
greenLED = LED(26)

##GUI DEFINITIONS
win = Tk()
win.title("LED Toggler")
myFont = tkinter.font.Font(family = 'Helvetica', size = 12, weight = "bold")
win.geometry("+800+300")
rad = IntVar()

##EVENT FUNCTIONS
def checkRadio():
    select = str(rad.get())

    if  select == "1":
        status = "Red LED is on"
        statusLED.config(text = status)
        redLED.on()
        blueLED.off()
        greenLED.off()
        
    elif select == "2":
        status = "Blue LED is on"
        statusLED.config(text = status)
        redLED.off()
        blueLED.on()
        greenLED.off()
        
    else:
        status = "Green LED is on"
        statusLED.config(text = status)
        redLED.off()
        blueLED.off()
        greenLED.on()

##WIDGETS
header = Label(win)
header.grid(row=1, column=2)
header.config(text="Light Colour Change Controls")
R1 = Radiobutton(win, text="Red Light", variable=rad, value=1, command = checkRadio)
R1.grid(row=2, column=2)
R2 = Radiobutton(win, text="Blue Light", variable=rad, value=2, command = checkRadio)
R2.grid(row=3, column=2)
R3 = Radiobutton(win, text="Green Light", variable=rad, value=3, command = checkRadio)
R3.grid(row=4, column=2)
statusLED = Label(win)
statusLED.grid(row=5, column=2)
statusLED.config(text="No option selected")

def close():
    GPIO.cleanup()
    win.destroy()

win.protocol("WM_DELETE_WINDOW", close)

win.mainloop()